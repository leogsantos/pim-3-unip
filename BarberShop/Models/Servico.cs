using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShop.Models
{
    [Table("Servico")]
    public class Servico
    {
        public int ServicoId { get; set; }

        public int CategoriaServicoId { get; set; }

        [Column("Nome")]
        private string nome = string.Empty;
        [Required, MaxLength(100)]
        [Display(Name = "Serviço")]
        public string Nome { get => nome; set => nome = value; }

        // null = "A consultar" — preço divulgado apenas após contato
        public decimal? Preco { get; set; }

        [Display(Name = "Duração (min)")]
        public int DuracaoMinutos { get; set; } = 30;

        public bool Ativo { get; set; } = true;

        public CategoriaServico? Categoria { get; set; }
    }
}
