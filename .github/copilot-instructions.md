# Copilot Instructions — Barbearia JLMGG (PIM III UNIP)

## Visão geral

Sistema web acadêmico para uma barbearia fictícia. Roda apenas em **localhost**.
Não há requisitos de escalabilidade, microserviços ou infraestrutura de produção.
Prioridade: código simples, legível e que demonstre os conceitos exigidos pela disciplina.

## Stack

| Camada | Tecnologia |
|---|---|
| Backend | ASP.NET Core MVC (.NET 10), C# |
| Banco de dados | SQL Server (Express), schema em `BancoSQL/CreateDB.sql` |
| ORM | Entity Framework Core (a ser configurado) |
| Frontend | Razor Views (`.cshtml`), Bootstrap 5, jQuery |
| Estático | `wwwroot/` — CSS, JS, lib |

## Estrutura do projeto

```
pim-3-unip/
├── BarberShop/                  # Projeto ASP.NET Core MVC
│   ├── Controllers/             # Controllers MVC
│   ├── Models/                  # Entidades que espelham as tabelas do banco
│   ├── Utils/                   # Utilitários transversais (AppLogger, etc.)
│   ├── Views/                   # Razor Views por controller
│   │   └── Shared/              # _Layout.cshtml, Error.cshtml
│   ├── wwwroot/                 # Ativos estáticos (CSS, JS, lib)
│   ├── Program.cs               # Entry point, DI, middleware
│   └── appsettings.json         # Configurações (conn string, etc.)
├── BancoSQL/
│   ├── CreateDB.sql             # DDL completo — fonte de verdade do schema
│   └── popular_banco.py         # Script Python para seed de dados de teste
└── .github/
    └── copilot-instructions.md  # Este arquivo
```

## Modelos de domínio

Todos em `BarberShop/Models/`, namespace `BarberShop.Models`.
São POCOs com `System.ComponentModel.DataAnnotations`. Espelham exatamente as tabelas do banco.

| Classe | Tabela | Observações |
|---|---|---|
| `Usuario` | `Usuario` | `TipoUsuario`: `"Cliente"` ou `"Admin"` |
| `Barbeiro` | `Barbeiro` | `Telefone` e `Email` são nullable |
| `CategoriaServico` | `CategoriaServico` | Tem `ICollection<Servico> Servicos` |
| `Servico` | `Servico` | `Preco` nullable = "A consultar" |
| `Agendamento` | `Agendamento` | FK para `Usuario`, `Barbeiro`, `Servico` |

## Logger

`AppLogger` em `BarberShop/Utils/AppLogger.cs`, namespace `BarberShop.Utils`.
Classe estática, thread-safe. Gera `logs/app-YYYY-MM-DD.log` (rotação diária).
A pasta `logs/` está no `.gitignore`.

```csharp
using BarberShop.Utils;

AppLogger.Info("mensagem informativa");
AppLogger.Warning("algo suspeito aconteceu");
AppLogger.Error("mensagem de erro");
AppLogger.Error("falha na operação", ex); // inclui stack trace
```

**Regra:** use `AppLogger` em todo bloco `catch` de operações com banco, I/O ou lógica crítica.
Nunca use `Console.WriteLine` para logging — use `AppLogger`.

## Requisitos acadêmicos obrigatórios

- **`try-catch-finally`** em operações com banco de dados e I/O. O `finally` deve estar presente.
- **Comentários estratégicos**: apenas onde o "porquê" não é óbvio. Sem comentários que repetem o que o código já diz.
- **Encapsulamento, herança e polimorfismo** devem aparecer no código (requisito POO).
- **Separação de responsabilidades**: controllers finos, lógica nos services/repositories.

Exemplo de padrão esperado:

```csharp
public IActionResult Create(Agendamento model)
{
    try
    {
        // ... salvar no banco
        AppLogger.Info($"Agendamento criado para {model.NomeCliente}.");
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        AppLogger.Error("Erro ao criar agendamento.", ex);
        ModelState.AddModelError("", "Erro ao salvar. Tente novamente.");
        return View(model);
    }
    finally
    {
        // liberar recursos se necessário
    }
}
```

## Convenções de código

- **PascalCase** para classes, métodos, propriedades e nomes de tabela/coluna
- **camelCase** para variáveis locais e parâmetros
- Prefixo `I` para interfaces (`IAgendamentoRepository`)
- Sufixos: `Controller`, `Repository`, `Service`, `ViewModel`
- Português para nomes de domínio (`Agendamento`, `Barbeiro`); inglês para infraestrutura e comentários no código
- Sem `Console.WriteLine` no código de produção
- Sem lógica de negócio em views (`.cshtml`)

## Banco de dados

- SGBD: SQL Server (Express), banco `BarbeariaJLMGG`
- Schema completo em `BancoSQL/CreateDB.sql` — consultar antes de qualquer migração EF
- Abordagem: **Database-First** (banco já existe) com modelos manuais; EF Core para queries
- `Preco` em `Servico` e `ValorTotal` em `Agendamento` são `decimal?` — `NULL` no banco significa "A consultar"
- Status de agendamento: `"Confirmado"`, `"Cancelado"`, `"Concluido"`

## O que não fazer

- Não adicionar camadas de abstração desnecessárias (ex: generic repository, mediator, CQRS)
- Não usar Docker, Redis, filas ou qualquer infra além de SQL Server local
- Não criar APIs REST separadas — tudo é MVC com Razor
- Não implementar autenticação JWT — usar ASP.NET Identity (cookies) quando chegar nessa fase
- Não commitar a pasta `logs/`, arquivos `appsettings.Development.json` ou `*.bak`
