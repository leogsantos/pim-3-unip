# pim-3-unip

### Stack Tecnologica - SQL Server (Express)

# O que esse projeto faz?
- Cadastro de clientes - Tela de login e cadastro
- Agendamento de cortes de cabelo, tratamentos quimicos, etc
- Visualização dos proprios agendamentos

## Separação de pastas e responsabilidades
UI e UX: João -> criação de telas, design, etc.
- [ ] Tela de login
- [ ] Tela de cadastro
- [ ] Tela de erro de login
- [ ] Tela de esqueci minha senha
- [ ] Readequar para visualização WEB
- [ ] Possibilidade de Cancelar e excluir agendamentos
- [ ] Adequar Libras em algum local do site (acessibilidade) -> https://vlibras.gov.br/doc/widget/installation/webpageintegration.html?_ga=2.205222480.1595640842.1682445746-816840059.1655413110

Frontend: Guilherme (pasta *wwwroot* e *Views*)
- [ ] Construção das páginas cshtml de acordo com o que vier do UI/UX
- [ ] Home page
- [ ] Modularização de arquivos (js, css, etc)
- [ ] Responsividade - focar apenas no desenvolvimento pra tela de PC

Backend: Marcos
- [ ] CRUD de agendamentos
- [ ] CRUD cadastro e logins (recuperar senha e etc)
- [ ] Utilizar Razor

Banco de dados: Leo, Gabriel
- [ ] Criar banco de dados
- [ ] Documentar banco
- [ ] Criar pasta "Data" para dar acesso as classes

---

## Configurando o Banco de Dados (faça isso antes de rodar o projeto)

> Siga esses passos **uma única vez** na sua máquina. Depois disso o projeto conecta sozinho.

### Passo 1 — Instale o SQL Server Express

Se ainda não tiver, baixe e instale gratuitamente:
- https://www.microsoft.com/pt-br/sql-server/sql-server-downloads (escolha a versão **Express**)

Instale também o **SQL Server Management Studio (SSMS)** para gerenciar o banco:
- https://aka.ms/ssmsfullsetup

### Passo 2 — Crie o banco de dados

1. Abra o **SSMS** e conecte no servidor `.\SQLEXPRESS`
  
   <img width="503" height="598" alt="image" src="https://github.com/user-attachments/assets/1018f8b6-abfd-49b4-bd52-836cf95b7340" />


4. Clique em **New Query** (botão no canto superior esquerdo)
5. Abra o arquivo `BancoSQL/CreateDB.sql` deste repositório (pode arrastar pra dentro do SSMS)
6. Clique em **Execute** (ou aperte `F5`)
7. O banco `BarbeariaJLMGG` será criado automaticamente com todas as tabelas e dados de exemplo

### Passo 3 — Verifique a connection string

O arquivo `appsettings.json` já vem configurado para SQL Server Express:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=BarbeariaJLMGG;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

**Se der erro de conexão**, o nome do seu servidor pode ser diferente. Para descobrir o nome correto:
1. Abra o SSMS
2. O nome que aparece no campo "Server name" é o que você deve usar no lugar de `.\SQLEXPRESS`

Exemplos comuns:
| Situação | Troque por |
|---|---|
| Instalou o SQL Server Express padrão | `.\SQLEXPRESS` já está certo |
| Aparece `(localdb)\MSSQLLocalDB` no SSMS | `(localdb)\\MSSQLLocalDB` |
| Aparece só o nome do PC | `.\` ou `NomeDoSeuPC` |

---

## Logger

A classe `AppLogger` (`BarberShop/Utils/AppLogger.cs`) centraliza os logs do projeto.
Os arquivos são gerados automaticamente em `logs/app-YYYY-MM-DD.log` (rotação diária).
A pasta `logs/` é ignorada pelo git.

### Como usar

```csharp
using BarberShop.Utils;

// Informação geral
AppLogger.Info("Agendamento criado com sucesso.");

// Aviso
AppLogger.Warning("Horário solicitado está próximo de outro agendamento.");

// Erro simples
AppLogger.Error("Falha ao salvar agendamento.");

// Erro com exceção capturada — recomendado no try-catch
try
{
    // ... operação que pode falhar
}
catch (Exception ex)
{
    AppLogger.Error("Erro ao processar agendamento.", ex);
    throw; // re-lança se necessário
}
finally
{
    AppLogger.Info("Bloco finally executado.");
}
```

### Formato do arquivo de log

```
[2026-05-23 14:30:00] [INFO ] Agendamento criado com sucesso.
[2026-05-23 14:30:05] [WARN ] Horário solicitado está próximo de outro agendamento.
[2026-05-23 14:30:10] [ERROR] Erro ao processar agendamento. | System.Exception: ...
```
