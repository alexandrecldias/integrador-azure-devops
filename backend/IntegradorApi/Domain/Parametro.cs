using System.ComponentModel.DataAnnotations.Schema;

namespace IntegradorApi.Domain;

public class Parametro
{
    public int Id { get; set; }
    public string Chave { get; set; } = null!;
    public string Valor { get; set; } = null!;

    [Column("atualizado_em")]
    public DateTime AtualizadoEm { get; set; }
}
