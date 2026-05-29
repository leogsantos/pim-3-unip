using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarberShop.Data;
using BarberShop.Models;
using BarberShop.Utils;

namespace BarberShop.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            AppLogger.Info("Listagem de usuários acessada.");
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                AppLogger.Warning("Details chamado sem ID.");
                return NotFound();
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                AppLogger.Warning($"Usuário não encontrado: ID {id}");
                return NotFound();
            }

            AppLogger.Info($"Detalhes exibidos: ID {id} ({usuario.Email})");
            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Email,Telefone,Senha")] Usuario usuario, string confirmarSenha)
        {
            usuario.TipoUsuario  = "Cliente";
            usuario.Ativo        = true;
            usuario.DataCadastro = DateTime.Now;

            if (usuario.Senha != confirmarSenha)
                ModelState.AddModelError("confirmarSenha", "As senhas não coincidem.");

            var emailJaExiste = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);
            if (emailJaExiste)
                ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(usuario);
                    await _context.SaveChangesAsync();
                    AppLogger.Info($"Usuário criado via CRUD: {usuario.Email}");
                    return RedirectToAction("Login", "Account");
                }
                catch (Exception ex)
                {
                    AppLogger.Error($"Erro ao criar usuário via CRUD: {usuario.Email}", ex);
                    ModelState.AddModelError("", "Erro interno ao salvar. Tente novamente.");
                }
            }

            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                AppLogger.Warning("Edit chamado sem ID.");
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                AppLogger.Warning($"Edição: usuário não encontrado: ID {id}");
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,Nome,Email,Telefone,Senha,TipoUsuario,DataCadastro,Ativo")] Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                AppLogger.Warning($"Edição: ID da rota ({id}) difere do model ({usuario.UsuarioId}).");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                    AppLogger.Info($"Usuário editado: ID {id} ({usuario.Email})");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!UsuarioExists(usuario.UsuarioId))
                    {
                        AppLogger.Warning($"Edição: usuário não encontrado ao salvar: ID {id}");
                        return NotFound();
                    }
                    AppLogger.Error($"Conflito de concorrência ao editar usuário: ID {id}", ex);
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                AppLogger.Warning("Delete chamado sem ID.");
                return NotFound();
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                AppLogger.Warning($"Exclusão: usuário não encontrado: ID {id}");
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                AppLogger.Info($"Usuário excluído: ID {id} ({usuario.Email})");
            }
            else
            {
                AppLogger.Warning($"Exclusão: usuário ID {id} não encontrado.");
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
    }
}
