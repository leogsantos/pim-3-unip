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
    public class BarbeiroesController : Controller
    {
        private readonly AppDbContext _context;

        public BarbeiroesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Barbeiroes
        public async Task<IActionResult> Index()
        {
            AppLogger.Info("Listagem de barbeiros acessada.");
            return View(await _context.Barbeiros.ToListAsync());
        }

        // GET: Barbeiroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var barbeiro = await _context.Barbeiros
                .FirstOrDefaultAsync(m => m.BarbeiroId == id);
            if (barbeiro == null)
            {
                return NotFound();
            }

            return View(barbeiro);
        }

        // GET: Barbeiroes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Barbeiroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BarbeiroId,Nome,Iniciais,Telefone,Email,Ativo")] Barbeiro barbeiro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(barbeiro);
                await _context.SaveChangesAsync();
                AppLogger.Info($"Barbeiro cadastrado: {barbeiro.Nome} (e-mail: {barbeiro.Email})");
                return RedirectToAction(nameof(Index));
            }
            return View(barbeiro);
        }

        // GET: Barbeiroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var barbeiro = await _context.Barbeiros.FindAsync(id);
            if (barbeiro == null)
            {
                return NotFound();
            }
            return View(barbeiro);
        }

        // POST: Barbeiroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BarbeiroId,Nome,Iniciais,Telefone,Email,Ativo")] Barbeiro barbeiro)
        {
            if (id != barbeiro.BarbeiroId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(barbeiro);
                    await _context.SaveChangesAsync();
                    AppLogger.Info($"Barbeiro editado: ID {id} ({barbeiro.Nome})");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BarbeiroExists(barbeiro.BarbeiroId))
                    {
                        AppLogger.Warning($"Edição: barbeiro não encontrado ao salvar: ID {id}");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(barbeiro);
        }

        // GET: Barbeiroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var barbeiro = await _context.Barbeiros
                .FirstOrDefaultAsync(m => m.BarbeiroId == id);
            if (barbeiro == null)
            {
                return NotFound();
            }

            return View(barbeiro);
        }

        // POST: Barbeiroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var barbeiro = await _context.Barbeiros.FindAsync(id);
            if (barbeiro != null)
            {
                _context.Barbeiros.Remove(barbeiro);
                await _context.SaveChangesAsync();
                AppLogger.Warning($"Barbeiro excluído: ID {id} ({barbeiro.Nome})");
            }
            else
            {
                AppLogger.Warning($"Tentativa de excluir barbeiro não encontrado: ID {id}");
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BarbeiroExists(int id)
        {
            return _context.Barbeiros.Any(e => e.BarbeiroId == id);
        }
    }
}
