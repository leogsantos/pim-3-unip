using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShop.Models
{
    [Table("CategoriaServico")]
    public class CategoriaServico
    {
        public int CategoriaServicoId { get; set; }

        [Required, MaxLength(50)]
        [Display(Name = "Categoria")]
        public string Nome { get; set; } = string.Empty;

        // Nome da classe de ícone Tabler (ex: "ti-scissors")
        [MaxLength(50)]
        public string? Icone { get; set; }

        public bool Ativo { get; set; } = true;

        public ICollection<Servico> Servicos { get; set; } = new List<Servico>();
    }
}
