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
