# Guia de CRUD — Barbearia JLMGG

> O banco já existe, os Models já existem, o DbContext já está configurado.
> Seu trabalho é criar os Controllers e as Views para cada tela do sistema.

---

## O que já está pronto (não mexa, a não ser que tenha mudança nas tabelas do banco)

| O que é | Onde fica | Para que serve |
|---|---|---|
| Modelos das tabelas | `BarberShop/Models/` | Representam os dados do banco |
| Conexão com o banco | `BarberShop/Data/AppDbContext.cs` | "Ponte" entre o código e o SQL Server |
| Configuração da conexão | `BarberShop/appsettings.json` | String de conexão com o banco local |
| Logger | `BarberShop/Utils/AppLogger.cs` | Para registrar erros e eventos |

---

## Regra importante: o banco JÁ EXISTE

Como o banco foi criado com o script SQL (`BancoSQL/CreateDB.sql`), **não use**:
- `Add-Migration`
- `Update-Database`

Esses comandos são só para quando o banco ainda não existe. No nosso caso, o banco já tem todas as tabelas — basta mapear com `[Table]` nos Models, o que **já foi feito**.

---

## Como criar um Controller com CRUD automático (Scaffolding)

O Visual Studio consegue gerar o Controller + as Views (telas) automaticamente a partir de um Model. Isso se chama **scaffolding**.

**Passos:**
1. Clique com o **botão direito** na pasta `Controllers`
2. Clique em **Adicionar → Controlador...**
3. Escolha **"Controlador MVC com exibições, usando o Entity Framework"**
4. Na janela que abrir:
   - **Classe do modelo:** escolha o model que quer (ex: `Agendamento`)
   - **Classe DbContext:** escolha `AppDbContext`
   - Deixe as 3 caixas de exibição marcadas
5. Clique em **Adicionar**

O Visual Studio vai criar automaticamente:
- `Controllers/AgendamentosController.cs` — com as actions Index, Create, Edit, Delete, Details
- `Views/Agendamentos/` — com as páginas prontas para cada action

> **Dica:** use scaffolding como ponto de partida. Depois você ajusta o código gerado para a lógica do projeto.

---

## Sobre os comentários `// GET:` e `// POST:` nos exemplos de código

Nos exemplos abaixo você vai ver comentários como:
```csharp
// GET: /Auth/Login
// POST: /Auth/Login
```

Isso **não tem nada a ver com publicação**. O projeto roda só no localhost mesmo.

Esses comentários são só documentação — é a convenção que o próprio Visual Studio gera no scaffolding. Eles indicam o **método HTTP** da requisição:

- `// GET: /Auth/Login` → quando o usuário **abre a página** no navegador. A URL completa seria `http://localhost:5173/Auth/Login`
- `// POST: /Auth/Login` → quando o usuário **clica em Enviar** num formulário. O navegador manda os dados para o mesmo endereço, mas pelo método POST

O `/Auth/Login` é só o caminho depois do `localhost:porta` — nada publicado, nada externo.

---

## Telas do sistema e o que fazer em cada uma

### 1. Login

**Tela:** formulário com e-mail e senha.

**O que precisa acontecer:**
- Buscar no banco um `Usuario` com aquele e-mail
- Verificar se a senha bate
- Se bater: salvar o usuário na sessão e redirecionar para a tela principal
- Se não bater: mostrar mensagem de erro

**Onde criar:**
- Controller: `Controllers/AuthController.cs` (criar manualmente, não é scaffolding)
- View: `Views/Auth/Login.cshtml`

**Exemplo do que vai dentro do Controller:**
```csharp
// GET: /Auth/Login
public IActionResult Login() => View();

// POST: /Auth/Login
[HttpPost]
public IActionResult Login(string email, string senha)
{
    try
    {
        var usuario = _context.Usuarios
            .FirstOrDefault(u => u.Email == email && u.Ativo);

        if (usuario == null || usuario.Senha != senha)
        {
            // senha errada ou usuário não encontrado
            ViewBag.Erro = "E-mail ou senha incorretos.";
            return View();
        }

        // salvar na sessão (implementar depois com HttpContext.Session)
        AppLogger.Info($"Login: {email}");
        return RedirectToAction("Index", "Home");
    }
    catch (Exception ex)
    {
        AppLogger.Error("Erro no login.", ex);
        return View();
    }
    finally
    {
        // bloco finally obrigatório
    }
}
```

---

### 2. Cadastro de usuário

**Tela:** formulário com nome, telefone, e-mail e senha (3 etapas no HTML de referência).

**O que precisa acontecer:**
- Verificar se o e-mail já está cadastrado
- Se não estiver: criar novo registro em `Usuario`
- Redirecionar para login (ou já logar)

**Onde criar:**
- Adicionar action `Cadastro` no mesmo `AuthController.cs`
- View: `Views/Auth/Cadastro.cshtml`

**Exemplo:**
```csharp
// POST: /Auth/Cadastro
[HttpPost]
public IActionResult Cadastro(Usuario model)
{
    try
    {
        bool jaExiste = _context.Usuarios.Any(u => u.Email == model.Email);
        if (jaExiste)
        {
            ViewBag.Erro = "Este e-mail já está cadastrado.";
            return View(model);
        }

        // ATENÇÃO: a senha deve ser armazenada com hash (não em texto puro)
        // Isso será implementado depois com BCrypt ou similar
        _context.Usuarios.Add(model);
        _context.SaveChanges();

        AppLogger.Info($"Novo usuário cadastrado: {model.Email}");
        return RedirectToAction("Login");
    }
    catch (Exception ex)
    {
        AppLogger.Error("Erro no cadastro.", ex);
        return View(model);
    }
    finally
    {
        // bloco finally obrigatório
    }
}
```

---

### 3. Redefinir senha

**Tela:** e-mail → código de verificação → nova senha (3 etapas no HTML de referência).

**O que precisa acontecer:**
- Etapa 1: receber o e-mail, verificar se existe no banco
- Etapa 2: verificar o código (por enquanto pode ser simulado/fixo)
- Etapa 3: atualizar o campo `Senha` do usuário no banco

**Onde criar:**
- Actions no `AuthController.cs`: `EsqueciSenha`, `VerificarCodigo`, `NovaSenha`
- Views em `Views/Auth/`

**Exemplo da etapa final (salvar nova senha):**
```csharp
// POST: /Auth/NovaSenha
[HttpPost]
public IActionResult NovaSenha(string email, string novaSenha)
{
    try
    {
        var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);
        if (usuario == null) return RedirectToAction("Login");

        usuario.Senha = novaSenha; // hash depois
        _context.SaveChanges();

        AppLogger.Info($"Senha redefinida para: {email}");
        return RedirectToAction("Login");
    }
    catch (Exception ex)
    {
        AppLogger.Error("Erro ao redefinir senha.", ex);
        return View();
    }
    finally
    {
        // bloco finally obrigatório
    }
}
```

---

### 4. Listagem de serviços e categorias

**Tela:** grade de categorias na home + lista de serviços ao clicar.

**O que precisa acontecer:**
- Buscar `CategoriaServico` ativos no banco
- Ao selecionar uma categoria, buscar `Servico` daquela categoria

**Onde criar:**
- Use scaffolding em `Servico` → gera `ServicosController` + Views
- Ou crie uma action no `HomeController` que já existe

**Consulta de exemplo:**
```csharp
// Buscar serviços de uma categoria
var servicos = _context.Servicos
    .Where(s => s.CategoriaServicoId == categoriaId && s.Ativo)
    .Include(s => s.Categoria)
    .ToList();
```

---

### 5. Agendamento (a tela mais importante)

**Tela:** selecionar serviço → calendário + barbeiro + horário → confirmar dados → sucesso.

**O que precisa acontecer:**
- Buscar barbeiros disponíveis (`Barbeiro` ativos)
- Verificar horários disponíveis (comparar com agendamentos já existentes naquela data)
- Salvar novo `Agendamento` no banco com todos os dados

**Onde criar:**
- Use scaffolding em `Agendamento` → gera `AgendamentosController` + Views base
- Vai precisar personalizar bastante o `Create`

**Exemplo do POST de criar agendamento:**
```csharp
// POST: /Agendamentos/Create
[HttpPost]
public IActionResult Create(Agendamento model)
{
    try
    {
        if (!ModelState.IsValid) return View(model);

        _context.Agendamentos.Add(model);
        _context.SaveChanges();

        AppLogger.Info($"Agendamento criado: {model.NomeCliente} em {model.DataHora}");
        return RedirectToAction("MeusAgendamentos");
    }
    catch (Exception ex)
    {
        AppLogger.Error("Erro ao criar agendamento.", ex);
        return View(model);
    }
    finally
    {
        // bloco finally obrigatório
    }
}
```

---

### 6. Meus Agendamentos

**Tela:** lista de agendamentos do usuário logado, com opção de editar e cancelar.

**O que precisa acontecer:**
- Buscar agendamentos do usuário logado (pelo `UsuarioId` da sessão)
- Cancelar = atualizar o campo `Status` para `"Cancelado"` (não deletar do banco)
- Editar = abrir o fluxo de agendamento novamente com os dados preenchidos

**Exemplo de cancelamento:**
```csharp
// POST: /Agendamentos/Cancelar/5
[HttpPost]
public IActionResult Cancelar(int id)
{
    try
    {
        var agendamento = _context.Agendamentos.Find(id);
        if (agendamento == null) return NotFound();

        agendamento.Status = "Cancelado";
        _context.SaveChanges();

        AppLogger.Warning($"Agendamento {id} cancelado.");
        return RedirectToAction("MeusAgendamentos");
    }
    catch (Exception ex)
    {
        AppLogger.Error("Erro ao cancelar agendamento.", ex);
        return RedirectToAction("MeusAgendamentos");
    }
    finally
    {
        // bloco finally obrigatório
    }
}
```

---

## Onde cada arquivo vai ficar

```
BarberShop/
├── Controllers/
│   ├── HomeController.cs          ← já existe, adicionar actions de home
│   ├── AuthController.cs          ← criar: login, cadastro, redefinir senha
│   ├── AgendamentosController.cs  ← scaffolding em Agendamento
│   ├── ServicosController.cs      ← scaffolding em Servico (opcional)
│   └── BarbeirosController.cs     ← scaffolding em Barbeiro (se precisar)
│
├── Views/
│   ├── Auth/
│   │   ├── Login.cshtml
│   │   ├── Cadastro.cshtml
│   │   └── EsqueciSenha.cshtml
│   ├── Agendamentos/
│   │   ├── Index.cshtml           ← gerado pelo scaffolding
│   │   ├── Create.cshtml          ← gerado pelo scaffolding
│   │   └── Edit.cshtml            ← gerado pelo scaffolding
│   └── Home/
│       └── Index.cshtml           ← tela principal com as categorias
```

---

## Injetando o DbContext no Controller

Todo controller que precisa acessar o banco deve receber o `AppDbContext` pelo construtor:

```csharp
using BarberShop.Data;
using BarberShop.Models;
using BarberShop.Utils;
using Microsoft.EntityFrameworkCore;

public class AuthController : Controller
{
    private readonly AppDbContext _context;

    // O ASP.NET injeta o AppDbContext automaticamente (configurado no Program.cs)
    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    // suas actions aqui...
}
```

> O scaffolding faz isso automaticamente. Se criar o controller na mão, copie esse padrão.

---

## Checklist de implementação

### Autenticação
- [ ] `AuthController` com Login, Cadastro, EsqueciSenha
- [ ] Views correspondentes em `Views/Auth/`
- [ ] Sessão do usuário (salvar `UsuarioId` após login)

### Agendamento
- [ ] Scaffolding de `Agendamento` → `AgendamentosController` + Views
- [ ] Adaptar tela de calendário com datas do banco
- [ ] Adaptar listagem de barbeiros (`_context.Barbeiros.Where(b => b.Ativo)`)
- [ ] Lógica de horários disponíveis (checar conflitos)

### Serviços
- [ ] Buscar categorias e serviços do banco na home
- [ ] Filtrar por categoria no clique

### Meus Agendamentos
- [ ] Listar agendamentos do usuário logado
- [ ] Cancelar (Status = "Cancelado", não deletar)
- [ ] Editar (redirecionar para o fluxo de agendamento)
