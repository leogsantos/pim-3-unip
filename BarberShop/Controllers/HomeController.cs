using BarberShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace BarberShop.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CancelarAgendamento(int id)
        {
            // Lógica de conexão com o banco de dados 
            bool exclusaoComSucesso = ExecutarExclusaoNoBanco(id);

            if (exclusaoComSucesso)
            {
                return Json(new { sucesso = true });
            }
            return Json(new { sucesso = false });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Adicionando a função para o Visual Studio não dar erro
        private bool ExecutarExclusaoNoBanco(int id)
        {
            // Futuramente aqui entrará o código do Entity Framework para deletar no banco
            return true; // Retorna true simulando que deu certo
        }
    }
}
