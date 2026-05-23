using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShop.Models
{
    [Table("CategoriaServico")]
    public class CategoriaServico
    {
        public int CategoriaServicoId { get; set; }

        [Column("Nome")]
        private string nome = string.Empty;
        [Required, MaxLength(50)]
        [Display(Name = "Categoria")]
        public string Nome { get => nome; set => nome = value; }

        // Nome da classe de ícone Tabler (ex: "ti-scissors") — nullable, exibição opcional
        [Column("Icone")]
        private string? icone;
        [MaxLength(50)]
        public string? Icone { get => icone; set => icone = value; }

        public bool Ativo { get; set; } = true;

        public ICollection<Servico> Servicos { get; set; } = new List<Servico>();
    }
}
