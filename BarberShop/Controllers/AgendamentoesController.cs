using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BarberShop.Data;
using BarberShop.Models;

namespace BarberShop.Controllers
{
    public class AgendamentoesController : Controller
    {
        private readonly AppDbContext _context;

        public AgendamentoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Agendamentoes
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Agendamentos.Include(a => a.Barbeiro).Include(a => a.Servico).Include(a => a.Usuario);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Agendamentoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agendamento = await _context.Agendamentos
                .Include(a => a.Barbeiro)
                .Include(a => a.Servico)
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(m => m.AgendamentoId == id);
            if (agendamento == null)
            {
                return NotFound();
            }

            return View(agendamento);
        }

        // GET: Agendamentoes/Create
        public IActionResult Create()
        {
            ViewData["BarbeiroId"] = new SelectList(_context.Barbeiros, "BarbeiroId", "Iniciais");
            ViewData["ServicoId"] = new SelectList(_context.Servicos, "ServicoId", "Nome");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Email");
            return View();
        }

        // POST: Agendamentoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AgendamentoId,UsuarioId,BarbeiroId,ServicoId,DataHora,Status,ValorTotal,NomeCliente,TelefoneCliente,Observacoes,DataCriacao")] Agendamento agendamento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(agendamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BarbeiroId"] = new SelectList(_context.Barbeiros, "BarbeiroId", "Iniciais", agendamento.BarbeiroId);
            ViewData["ServicoId"] = new SelectList(_context.Servicos, "ServicoId", "Nome", agendamento.ServicoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Email", agendamento.UsuarioId);
            return View(agendamento);
        }

        // GET: Agendamentoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento == null)
            {
                return NotFound();
            }
            ViewData["BarbeiroId"] = new SelectList(_context.Barbeiros, "BarbeiroId", "Iniciais", agendamento.BarbeiroId);
            ViewData["ServicoId"] = new SelectList(_context.Servicos, "ServicoId", "Nome", agendamento.ServicoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Email", agendamento.UsuarioId);
            return View(agendamento);
        }

        // POST: Agendamentoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AgendamentoId,UsuarioId,BarbeiroId,ServicoId,DataHora,Status,ValorTotal,NomeCliente,TelefoneCliente,Observacoes,DataCriacao")] Agendamento agendamento)
        {
            if (id != agendamento.AgendamentoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(agendamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgendamentoExists(agendamento.AgendamentoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BarbeiroId"] = new SelectList(_context.Barbeiros, "BarbeiroId", "Iniciais", agendamento.BarbeiroId);
            ViewData["ServicoId"] = new SelectList(_context.Servicos, "ServicoId", "Nome", agendamento.ServicoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Email", agendamento.UsuarioId);
            return View(agendamento);
        }

        // GET: Agendamentoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agendamento = await _context.Agendamentos
                .Include(a => a.Barbeiro)
                .Include(a => a.Servico)
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(m => m.AgendamentoId == id);
            if (agendamento == null)
            {
                return NotFound();
            }

            return View(agendamento);
        }

        // POST: Agendamentoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento != null)
            {
                _context.Agendamentos.Remove(agendamento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgendamentoExists(int id)
        {
            return _context.Agendamentos.Any(e => e.AgendamentoId == id);
        }
    }
}
