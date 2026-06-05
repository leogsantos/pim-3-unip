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
        [HttpGet]
        public async Task<IActionResult> MeusAgendamentos()
        {
            var lista = await _context.Agendamentos
                .Include(a => a.Barbeiro)
                .Include(a => a.Servico)
                .OrderByDescending(a => a.DataHora)
                .Select(a => new
                {
                    id = a.AgendamentoId,
                    service = a.Servico.Nome,
                    barber = a.Barbeiro.Nome,
                    date = a.DataHora.ToString("dd/MM/yyyy"),
                    time = a.DataHora.ToString("HH:mm"),
                    price = a.ValorTotal,
                    name = a.NomeCliente,
                    phone = a.TelefoneCliente
                })
                .ToListAsync();

            return Json(lista);
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
        [HttpPost]
        public async Task<IActionResult> DeleteApi(int id)
        {
            var agendamento =
                await _context.Agendamentos.FindAsync(id);

            if (agendamento == null)
                return NotFound();

            _context.Agendamentos.Remove(agendamento);

            await _context.SaveChangesAsync();

            return Json(new { ok = true });
        }
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

        [HttpPost]
        public async Task<IActionResult> CriarAgendamento([FromBody] Agendamento agendamento)
        {

            var usuarioIdClaim = User.FindFirst("UsuarioId")?.Value;
            


            
            try
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.UsuarioId == int.Parse(usuarioIdClaim));

                agendamento.UsuarioId = usuario.UsuarioId;
                    

                agendamento.DataCriacao = DateTime.Now;

                var servico = await _context.Servicos
                    .FirstOrDefaultAsync(s => s.ServicoId == agendamento.ServicoId);

                if (servico == null)
                {
                    return Json(new
                    {
                        ok = false,
                        erro = "Serviço não encontrado"
                    });
                }

                agendamento.ValorTotal = servico.Preco;

                _context.Agendamentos.Add(agendamento);

                await _context.SaveChangesAsync();

                return Json(new { ok = true });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    ok = false,
                    erro = ex.ToString()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarAgendamento([FromBody] Agendamento agendamento)
        {
            try
            {
                var existente = await _context.Agendamentos
                    .FirstOrDefaultAsync(a => a.AgendamentoId == agendamento.AgendamentoId);

                if (existente == null)
                    return Json(new { ok = false, erro = "Agendamento não encontrado" });

                var servico = await _context.Servicos
                    .FirstOrDefaultAsync(s => s.ServicoId == agendamento.ServicoId);

                existente.ServicoId = agendamento.ServicoId;
                existente.ValorTotal = servico?.Preco;
                existente.ServicoId = agendamento.ServicoId;
                existente.BarbeiroId = agendamento.BarbeiroId;
                existente.DataHora = agendamento.DataHora;
                existente.NomeCliente = agendamento.NomeCliente;
                existente.TelefoneCliente = agendamento.TelefoneCliente;
                existente.Observacoes = agendamento.Observacoes;
                existente.ValorTotal = agendamento.ValorTotal;

                await _context.SaveChangesAsync();

                return Json(new { ok = true });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    ok = false,
                    erro = ex.Message
                });
            }
        }

        private bool AgendamentoExists(int id)
        {
            return _context.Agendamentos.Any(e => e.AgendamentoId == id);
        }
        //Faz uma lista dos barbeiros atuais
        [HttpGet]
        public async Task<IActionResult> ListarBarbeiros()
        {
            var barbeiros = await _context.Barbeiros
                .Where(b => b.Ativo)
                .Select(b => new
                {
                    id = b.BarbeiroId,
                    nome = b.Nome,
                    iniciais = b.Iniciais
                })
                .ToListAsync();

            return Json(barbeiros);
        }
        //Faz uma lista dos servicos atuais
        [HttpGet]
        public async Task<IActionResult> ListarServicos()
        {
            var servicos = await _context.Servicos
                .Where(s => s.Ativo)
                .Select(s => new
                {
                    id = s.ServicoId,
                    nome = s.Nome,
                    preco = s.Preco
                })
                .ToListAsync();

            return Json(servicos);
        }
    }
}
