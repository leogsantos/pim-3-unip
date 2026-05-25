using Microsoft.AspNetCore.Mvc;

namespace BarberShop.Controllers
{
    public class AccountController : Controller
    {
        // 1. Exibe a tela de login que você acabou de criar
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

        // 2. Recebe os dados do formulário quando clica em ENTRAR
        [HttpPost]
        public IActionResult Autenticar(string usuario, string senha)
        {
            // Validação provisória (depois vamos ligar isso ao banco de dados)
            if (usuario == "admin" && senha == "123")
            {
                // Se estiver correto, redireciona para a tela inicial (Painel da Barbearia)
                return RedirectToAction("Index", "Home");
            }

            // Se errar a senha, recarrega a tela com uma mensagem de erro
            ViewBag.Erro = "Usuário ou senha incorretos. Tente: admin / 123";
            return View("Login");
        }
    }
}