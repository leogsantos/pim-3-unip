using System.ComponentModel.DataAnnotations;

namespace BarberShop.Models
{
    public class CategoriaServico
    {
        public int CategoriaServicoId { get; set; }

        [Required, MaxLength(50)]
        [Display(Name = "Categoria")]
        public string Nome { get; set; } = string.Empty;

        // Tabler icon class name (e.g. "ti-scissors")
        [MaxLength(50)]
        public string? Icone { get; set; }

        public bool Ativo { get; set; } = true;

        public ICollection<Servico> Servicos { get; set; } = new List<Servico>();
    }
}
