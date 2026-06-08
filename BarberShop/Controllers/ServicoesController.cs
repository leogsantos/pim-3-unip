using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BarberShop.Data;
using BarberShop.Models;
using BarberShop.Utils;

namespace BarberShop.Controllers
{
    public class ServicoesController : Controller
    {
        private readonly AppDbContext _context;

        public ServicoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Servicoes
        public async Task<IActionResult> Index()
        {
            AppLogger.Info("Listagem de serviços acessada.");
            var appDbContext = _context.Servicos.Include(s => s.Categoria);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Servicoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servico = await _context.Servicos
                .Include(s => s.Categoria)
                .FirstOrDefaultAsync(m => m.ServicoId == id);
            if (servico == null)
            {
                return NotFound();
            }

            return View(servico);
        }

        // GET: Servicoes/Create
        public IActionResult Create()
        {
            ViewData["CategoriaServicoId"] = new SelectList(_context.CategoriaServicos, "CategoriaServicoId", "Nome");
            return View();
        }

        // POST: Servicoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServicoId,CategoriaServicoId,Nome,Preco,DuracaoMinutos,Ativo")] Servico servico)
        {
            if (ModelState.IsValid)
            {
                _context.Add(servico);
                await _context.SaveChangesAsync();
                AppLogger.Info($"Serviço cadastrado: {servico.Nome} (R$ {servico.Preco})");
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaServicoId"] = new SelectList(_context.CategoriaServicos, "CategoriaServicoId", "Nome", servico.CategoriaServicoId);
            return View(servico);
        }

        // GET: Servicoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servico = await _context.Servicos.FindAsync(id);
            if (servico == null)
            {
                return NotFound();
            }
            ViewData["CategoriaServicoId"] = new SelectList(_context.CategoriaServicos, "CategoriaServicoId", "Nome", servico.CategoriaServicoId);
            return View(servico);
        }

        // POST: Servicoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServicoId,CategoriaServicoId,Nome,Preco,DuracaoMinutos,Ativo")] Servico servico)
        {
            if (id != servico.ServicoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servico);
                    await _context.SaveChangesAsync();
                    AppLogger.Info($"Serviço editado: ID {id} ({servico.Nome})");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicoExists(servico.ServicoId))
                    {
                        AppLogger.Warning($"Edição: serviço não encontrado ao salvar: ID {id}");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaServicoId"] = new SelectList(_context.CategoriaServicos, "CategoriaServicoId", "Nome", servico.CategoriaServicoId);
            return View(servico);
        }

        // GET: Servicoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servico = await _context.Servicos
                .Include(s => s.Categoria)
                .FirstOrDefaultAsync(m => m.ServicoId == id);
            if (servico == null)
            {
                return NotFound();
            }

            return View(servico);
        }

        // POST: Servicoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servico = await _context.Servicos.FindAsync(id);
            if (servico != null)
            {
                _context.Servicos.Remove(servico);
                await _context.SaveChangesAsync();
                AppLogger.Warning($"Serviço excluído: ID {id} ({servico.Nome})");
            }
            else
            {
                AppLogger.Warning($"Tentativa de excluir serviço não encontrado: ID {id}");
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ServicoExists(int id)
        {
            return _context.Servicos.Any(e => e.ServicoId == id);
        }
    }
}
