# Integrador Azure DevOps

Este projeto automatiza a criação de PBIs e tarefas entre diferentes projetos no Azure DevOps, utilizando uma API em .NET, frontend em Angular e banco de dados MariaDB.

## Objetivo

Substituir uma aplicação console por uma aplicação web com interface amigável, escalável e reutilizável por outras equipes.

## Tecnologias
- .NET Core API
- Angular
- MariaDB via Docker

## Banco de Dados

A conexao possui os seguintes dados para configuração com o dbaver

jdbc:mysql://localhost:3308/integrador?allowPublicKeyRetrieval=true&useSSL=false
Usuario: root
senha123
<img width="590" height="580" alt="image" src="https://github.com/user-attachments/assets/e46b0fc5-0eb3-4f3f-a1b6-e53b1c918af2" />

Necessário criar o banco de dados chamado integrador e com a tabela Parametros abaixo.

CREATE TABLE `Parametros` (
  `id` int NOT NULL AUTO_INCREMENT,
  `chave` varchar(100) NOT NULL,
  `valor` text NOT NULL,
  `atualizado_em` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `chave` (`chave`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


<img width="492" height="398" alt="image" src="https://github.com/user-attachments/assets/413df7d0-c00e-4e25-b618-56110391e855" />



## DOCKER

Onde está localizado o arquivo docker-compose.yml, rode no bash os comandos abaixo.

docker-compose build
docker-compose up -d

ℹ️ Requisitos prévios

Antes de rodar os comandos acima, a pessoa precisa:
Ter o Docker e Docker Compose instalados
Estar na pasta raiz do projeto onde está localizado o docker-compose.yml
