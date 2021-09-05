# Projeto RssStore
    - Projeto desenvolvido durante o curso modelando dominios ricos desenvolvedor.io

## Pacotes instalados:
    EntityFramework 3.1.17 - Gerenciar conexão com o banco de dados
    EntityFramework.SqlServer 3.1.17 - Utilizar o provider o SQLServer adotado como banco de dados
    EntityFramework.Tools 3.1.17 - Trabalhar com comandos ef
    EntityFramework.Design 3.1.17 - Gerar migrations
    FluentValidation (estável recente) - Validações das classes de comandos
    MediatR - Utilizar notificações e eventos para comunicação entre contextos
    AutoMapper - Mapeamentos de entidades e modelos
    EventStore.Client - para conectar ao banco de eventos

## Comandos úteis para incializar os contextos
    - dotnet restore
    - dotnet build
    - dotnet ef add-migration "Initial" -Context [nomeContexto] -Verbose (caso não tenha sido gerado)
    - dotnet ef update-database -Context [nomeContexto] -Verbose (caso já tenha sido gerada)
    - docker run --name esdb-node -it -p 2113:2113 -p 1113:1113 \eventstore/eventstore:latest --insecure --run-projections=All (rodar EventStore no docker)