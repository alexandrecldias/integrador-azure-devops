﻿using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using IntegradorApi.Infrastructure.Interfaces;
using Newtonsoft.Json.Linq;

namespace IntegradorApi.Infrastructure.AzureDevOps
{
    public class AzureDevOpsClient : IAzureDevOpsClient
    {
        public async Task<int> ClonarWorkItemAsync(
            string origemUrl, string patOrigem,
            string destinoUrl, string patDestino,
            string projetoDestino, string iterationPath,
            string atividade, string responsavel,
            int idWorkItemOrigem)
        {
            var workItem = await ObterWorkItem(origemUrl, patOrigem, idWorkItemOrigem);

            // Descobrir ID da US associada
            int idUS = 0;
            var relations = workItem["relations"];
            string hashCommit = string.Empty;
            if (relations != null)
            {
                // Procura a US associada
                foreach (var relation in relations)
                {
                    var rel = relation["rel"]?.ToString();
                    var url = relation["url"]?.ToString();
                    if (rel == "System.LinkTypes.Hierarchy-Reverse" && url.Contains("/workItems/"))
                    {
                        var idStr = url.Split("/").Last();
                        idUS = int.Parse(idStr);
                        break;
                    }
                }
                // Procura novamente pelo commit associado
                foreach (var relation in relations)
                {
                    var rel = relation["rel"]?.ToString();
                    var url = relation["url"]?.ToString();
                    // Se for um link de commit
                    if (rel == "ArtifactLink" && url != null && url.StartsWith("vstfs:///Git/Commit/"))
                    {
                        // Divide a URL com base em %2F e pega a última parte como commit hash
                        var partes = url.Split(new[] { "%2F" }, StringSplitOptions.RemoveEmptyEntries);
                        if (partes.Length > 0)
                        {
                            hashCommit = partes.Last(); // Última parte é o hash real
                            break;
                        }
                    }
                }
            }

            if (idUS == 0)
                return 0;

            var usWorkItem = await ObterWorkItem(origemUrl, patOrigem, idUS);
            string tituloUS = usWorkItem["fields"]["System.Title"]?.ToString();
            string tituloPbiEsperado = idUS.ToString();

            int idPbiDestino = await BuscarPbiDestino(destinoUrl, patDestino, projetoDestino, tituloPbiEsperado);

            if (idPbiDestino == 0)
            {
                string responsavelUSOrigem = usWorkItem["fields"]["System.AssignedTo"]?.ToString();
                string tituloCompleto = $"{idUS} - {tituloUS}";
                string descricaoUS = usWorkItem["fields"]["System.Description"]?.ToString();
                string criteriosAceite = usWorkItem["fields"]["Microsoft.VSTS.Common.AcceptanceCriteria"]?.ToString();
                string horasEstimadasUS = usWorkItem["fields"]["Microsoft.VSTS.Scheduling.OriginalEstimate"]?.ToString();

                double.TryParse(horasEstimadasUS, out double horas);
                string complexidade = ObterComplexidadePorHora(horas);


                idPbiDestino = await CriarPbiDestino(
                    destinoUrl, patDestino, projetoDestino,
                    tituloCompleto, descricaoUS, criteriosAceite,
                    horasEstimadasUS, responsavel, iterationPath, complexidade
                );

                if (idPbiDestino == 0)
                    return 0;
            }

            // Dados da task
            string titulo = workItem["fields"]["System.Title"]?.ToString();
            string descricao = workItem["fields"]["System.Description"]?.ToString();
            string horasEstimadas = workItem["fields"]["Microsoft.VSTS.Scheduling.OriginalEstimate"]?.ToString();
            string horasRestantes = workItem["fields"]["Microsoft.VSTS.Scheduling.RemainingWork"]?.ToString();
            string horasCompletadas = workItem["fields"]["Microsoft.VSTS.Scheduling.CompletedWork"]?.ToString();
            string prioridade = workItem["fields"]["Microsoft.VSTS.Common.Priority"]?.ToString();
            string tipoTask = ObterTipoTaskPorDescricao(descricao);
 
            return await CriarTaskDestino(
                destinoUrl, patDestino, projetoDestino, titulo, descricao,
                horasEstimadas, horasRestantes, horasCompletadas,
                prioridade, tipoTask, idPbiDestino, iterationPath,
                atividade, responsavel, idWorkItemOrigem, hashCommit
            );
        }

        private async Task<JObject> ObterWorkItem(string baseUrl, string pat, int id)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($":{pat}")));

            var response = await client.GetAsync($"_apis/wit/workitems/{id}?$expand=relations&api-version=6.0");
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            return JObject.Parse(result);
        }

        private async Task<int> BuscarPbiDestino(string baseUrl, string pat, string projeto, string tituloEsperado)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($":{pat}")));

            var wiql = new
            {
                query = $"SELECT [System.Id] FROM WorkItems WHERE [System.TeamProject] = '{projeto}' AND [System.WorkItemType] = 'Product Backlog Item' AND [System.Title] CONTAINS '{tituloEsperado}'"
            };

            var content = new StringContent(JObject.FromObject(wiql).ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("_apis/wit/wiql?api-version=6.0", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return 0;

            var json = JObject.Parse(responseContent);
            var workItems = json["workItems"] as JArray;
            return workItems != null && workItems.Count > 0 ? (int)workItems[0]["id"] : 0;
        }

        private async Task<int> CriarPbiDestino(string baseUrl, string pat, string projeto,
            string titulo, string descricao, string criteriosAceite, string horasEstimadas,
            string responsavel, string iterationPath, string complexidade)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($":{pat}")));

            var json = new JArray
            {
                new JObject { { "op", "add" }, { "path", "/fields/System.Title" }, { "value", titulo } },
                new JObject { { "op", "add" }, { "path", "/fields/System.Description" }, { "value", descricao } },
                new JObject { { "op", "add" }, { "path", "/fields/System.AssignedTo" }, { "value", responsavel } },
                new JObject { { "op", "add" }, { "path", "/fields/System.IterationPath" }, { "value", iterationPath } },
                new JObject { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Scheduling.OriginalEstimate" }, { "value", "0" } },
                new JObject { { "op", "add" }, { "path", "/fields/Custom.Complexity" }, { "value", complexidade } },
            };

            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json-patch+json");
            var response = await client.PostAsync($"{projeto}/_apis/wit/workitems/$Product%20Backlog%20Item?api-version=6.0", content);
            if (!response.IsSuccessStatusCode)
                return 0;

            var jsonResponse = JObject.Parse(await response.Content.ReadAsStringAsync());
            return (int)jsonResponse["id"];
        }

        private async Task<int> CriarTaskDestino(string baseUrl, string pat, string projeto,
            string titulo, string descricao, string horasEstimadas, string horasRestantes,
            string horasCompletadas, string prioridade, string tipoTask, int idPbi,
            string iterationPath, string atividade, string responsavel, int idWorkItemOrigem, string hashCommit)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($":{pat}")));

            //var htmlSeguro = WebUtility.HtmlEncode(SanitizeDescricao(descricao));


            var json = new JArray
                {
                    new JObject { { "op", "add" }, { "path", "/fields/System.Title" }, { "value", $"{idWorkItemOrigem}-{titulo}" } },
                    new JObject { { "op", "add" }, { "path", "/fields/System.Description" }, { "value", descricao } },
                    new JObject { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Scheduling.OriginalEstimate" }, { "value", horasEstimadas } },
                    new JObject { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Scheduling.Effort" }, { "value", horasCompletadas } },
                    new JObject { { "op", "add" }, { "path", "/fields/Custom.DateWork" }, { "value", DateTime.UtcNow.ToString("o") } },
                    new JObject { { "op", "add" }, { "path", "/fields/System.IterationPath" }, { "value", iterationPath } },
                    new JObject { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Common.Activity" }, { "value", atividade } },
                    new JObject { { "op", "add" }, { "path", "/fields/System.AssignedTo" }, { "value", responsavel } },
                    new JObject { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Scheduling.CompletedWork" }, { "value", horasCompletadas } },
                    new JObject { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Common.Priority" }, { "value", prioridade } },
                    new JObject { { "op", "add" }, { "path", "/fields/Custom.TypeTask" }, { "value", tipoTask } },
                    new JObject { { "op", "add" }, { "path", "/fields/Custom.Commit" }, { "value", hashCommit } },

                    new JObject
                    {
                        { "op", "add" },
                        { "path", "/relations/-" },
                        { "value", new JObject
                            {
                                { "rel", "System.LinkTypes.Hierarchy-Reverse" },
                                { "url", $"{baseUrl}_apis/wit/workItems/{idPbi}" },
                                { "attributes", new JObject { { "comment", "Vinculado ao PBI" } } }
                            }
                        }
                    }
                };


            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json-patch+json");
            var response = await client.PostAsync($"{projeto}/_apis/wit/workitems/$Task?api-version=6.0", content);
            if (!response.IsSuccessStatusCode)
            {
                string content2 = await response.Content.ReadAsStringAsync();
                throw new Exception($"Sistema apresentou o seguinte erro {content2} ");
            }

            var jsonResponse = JObject.Parse(await response.Content.ReadAsStringAsync());
            return (int)jsonResponse["id"];
        }

        private string ObterComplexidadePorHora(double horas)
        {
            return horas switch
            {
                < 8 => "Muito Simples",
                < 16 => "Simples",
                _ => "Complexa"
            };
        }

        public static string SanitizeDescricao(string html)
        {
            // Remove inputs
            html = Regex.Replace(html, "<input[^>]*>", string.Empty, RegexOptions.IgnoreCase);

            // Substitui &nbsp; por espaço normal
            html = html.Replace("&nbsp;", " ");

            // Substitui "->" por símbolo → ou traço simples
            html = html.Replace("-&gt;", "→");

            return html;
        }


        private string ObterTipoTaskPorDescricao(string descricao)
        {
            if (descricao.Contains("commit", StringComparison.OrdinalIgnoreCase))
                return "Desenvolvimento";

            if (descricao.Contains("reunião", StringComparison.OrdinalIgnoreCase))
                return "Reuniao";

            // Comandos SQL que indicam alteração no banco de dados
            string[] comandosSql = { "update", "delete", "insert", "alter", "drop", "create", "truncate", "merge" };

            foreach (var comando in comandosSql)
            {
                if (descricao.Contains(comando, StringComparison.OrdinalIgnoreCase))
                    return "Banco de dados";
            }

            return "Analise";
        }

        private string ExtrairHashCommit(string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                return string.Empty;

            var linhas = descricao.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < linhas.Length - 1; i++)
            {
                if (linhas[i].Trim().Equals("Commit", StringComparison.OrdinalIgnoreCase))
                {
                    var hashPossivel = linhas[i + 1].Trim();

                    // Verifica se o hash tem entre 7 e 40 caracteres e contém apenas a-f e 0-9
                    if (Regex.IsMatch(hashPossivel, @"\b[a-fA-F0-9]{7,40}\b"))
                    {
                        return hashPossivel;
                    }
                }
            }

            return string.Empty;
        }




    }
}
