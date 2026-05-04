# task-manager
Sistema de Gestão de Tarefas versão 1.0

## Descrição
O Task Manager é um sistema de gestão de tarefas desenvolvido em C# utilizando o .NET 8.0. 
Ele permite aos usuários criar, organizar e acompanhar suas tarefas de forma eficiente. 
O sistema oferece funcionalidades como criação de tarefas, definição de prazos, atribuição de data de vencimento e acompanhamento do progresso.

## Funcionalidades
- Criação de tarefas: Os usuários podem criar novas tarefas, fornecendo um título, descrição e data de vencimento.
- Listagem de tarefas: Os usuários podem visualizar uma lista de todas as tarefas criadas, com informações como título, descrição, status e data de vencimento.
- Edição de tarefas: Os usuários podem editar as informações das tarefas existentes, como título, descrição, status e data de vencimento.
- Exclusão de tarefas: Os usuários podem excluir tarefas que não são mais necessárias.

## Tecnologias e padrões Utilizadas
- .NET 8.0
- C# 12.0
- Entity Framework Core
- SQLite (banco de dados em memória para aplicação e testes unitários)
- XUnit (para testes unitários)
- Swagger (para documentação da API)
- DTO (Data Transfer Object) para transferência de dados entre camadas de apresentação e lógica de negócios
- Arquitetura hexagonal (organização do código em camadas para facilitar a manutenção e escalabilidade)
- SOLID (princípios de design de software, incluindo responsabilidade única, aberto/fechado, substituição de Liskov, segregação de interface e inversão de dependência)
- Repository Pattern (para abstração de acesso a dados)

## Como executar a aplicação
1. Clone o repositório para sua máquina local.
2. Abra o projeto em uma IDE compatível com .NET, como Visual Studio ou Visual Studio Code.
3. Restaure as dependências do projeto utilizando o comando `dotnet restore`.
4. Execute a aplicação utilizando o comando `dotnet run` ou através da IDE.
5. A aplicação estará disponível em `https://localhost:7243` e http://localhost:5083 (ou outra porta configurada) para acesso.
6. Utilize a interface do Swagger (https://localhost:7243/swagger) para criar, listar, editar e excluir tarefas conforme necessário.