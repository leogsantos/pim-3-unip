using Microsoft.EntityFrameworkCore;
using BarberShop.Models;

namespace BarberShop.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Barbeiro> Barbeiros { get; set; }
        public DbSet<CategoriaServico> CategoriaServicos { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }
    }
}
