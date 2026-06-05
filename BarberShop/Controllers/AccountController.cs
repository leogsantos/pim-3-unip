using BarberShop.Data;
using BarberShop.Models;
using BarberShop.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace BarberShop.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;

        public AccountController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EsqueceuSenha()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Autenticar(string usuario, string senha)
        {
            AppLogger.Info($"Tentativa de login: {usuario}");

            var user = await _db.Usuarios
                .FirstOrDefaultAsync(u => u.Email == usuario && u.Senha == senha && u.Ativo);

            if (user != null)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Nome),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.TipoUsuario),
                new Claim("UsuarioId", user.UsuarioId.ToString()),
                new Claim("Telefone", user.Telefone ?? "")
            };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);

                AppLogger.Info($"Login bem-sucedido: {usuario}");

                return RedirectToAction("Index", "Home");
            }

            AppLogger.Warning($"Login falhou (e-mail ou senha incorretos): {usuario}");
            ViewBag.Erro = "E-mail ou senha incorretos.";
            return View("Login");
        }

        // Chamado pelo JS (fetch) na tela de Registro ao clicar em CADASTRAR
        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] JsonElement body)
        {
            var nome     = body.GetProperty("nome").GetString() ?? "";
            var telefone = body.GetProperty("telefone").GetString() ?? "";
            var email    = body.GetProperty("email").GetString() ?? "";
            var senha    = body.GetProperty("senha").GetString() ?? "";

            AppLogger.Info($"Tentativa de cadastro: {email}");

            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                AppLogger.Warning($"Cadastro rejeitado — dados incompletos: {email}");
                return Json(new { ok = false, erro = "Preencha todos os campos obrigatórios." });
            }

            var emailJaExiste = await _db.Usuarios.AnyAsync(u => u.Email == email);
            if (emailJaExiste)
            {
                AppLogger.Warning($"Cadastro rejeitado — e-mail já cadastrado: {email}");
                return Json(new { ok = false, erro = "Este e-mail já está cadastrado." });
            }

            try
            {
                var usuario = new Usuario
                {
                    Nome          = nome,
                    Telefone      = telefone,
                    Email         = email,
                    Senha         = senha,
                    TipoUsuario   = "Cliente",
                    Ativo         = true,
                    DataCadastro  = DateTime.Now
                };

                _db.Usuarios.Add(usuario);
                await _db.SaveChangesAsync();

                AppLogger.Info($"Usuário cadastrado com sucesso: {email}");
                return Json(new { ok = true });
            }
            catch (Exception ex)
            {
                AppLogger.Error($"Erro ao cadastrar usuário: {email}", ex);
                return Json(new { ok = false, erro = "Erro interno ao salvar. Tente novamente." });
            }
        }

        // Chamado pelo JS na tela EsqueceuSenha — verifica se o e-mail existe no banco
        [HttpPost]
        public async Task<IActionResult> VerificarEmail([FromBody] JsonElement body)
        {
            var email = body.GetProperty("email").GetString() ?? "";
            AppLogger.Info($"Verificação de e-mail para redefinição de senha: {email}");

            var existe = await _db.Usuarios.AnyAsync(u => u.Email == email && u.Ativo);

            if (!existe)
                AppLogger.Warning($"Redefinição de senha: e-mail não encontrado: {email}");

            return Json(new { ok = existe });
        }

        // Chamado pelo JS na tela EsqueceuSenha — salva a nova senha no banco
        [HttpPost]
        public async Task<IActionResult> RedefinirSenha([FromBody] JsonElement body)
        {
            var email     = body.GetProperty("email").GetString() ?? "";
            var novaSenha = body.GetProperty("novaSenha").GetString() ?? "";

            AppLogger.Info($"Redefinição de senha solicitada: {email}");

            var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == email && u.Ativo);
            if (usuario == null)
            {
                AppLogger.Warning($"Redefinição de senha: usuário não encontrado: {email}");
                return Json(new { ok = false, erro = "Usuário não encontrado." });
            }

            try
            {
                usuario.Senha = novaSenha;
                await _db.SaveChangesAsync();
                AppLogger.Info($"Senha redefinida com sucesso: {email}");
                return Json(new { ok = true });
            }
            catch (Exception ex)
            {
                AppLogger.Error($"Erro ao redefinir senha: {email}", ex);
                return Json(new { ok = false, erro = "Erro interno ao salvar. Tente novamente." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            AppLogger.Info("Usuário realizou logout.");

            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public async Task<IActionResult> UsuarioLogado() // pega id do usuario para os campos de agendamento
        {
            var usuarioIdClaim = User.FindFirst("UsuarioId")?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim))
                return Unauthorized();

            var usuario = await _db.Usuarios
                .FirstOrDefaultAsync(u => u.UsuarioId == int.Parse(usuarioIdClaim));

            if (usuario == null)
                return NotFound();

            return Json(new
            {
                nome = usuario.Nome,
                telefone = usuario.Telefone
            });
        }
    }

}
