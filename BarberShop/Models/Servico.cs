using System.ComponentModel.DataAnnotations;

namespace BarberShop.Models
{
    public class Servico
    {
        public int ServicoId { get; set; }

        public int CategoriaServicoId { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "Serviço")]
        public string Nome { get; set; } = string.Empty;

        // null means "A consultar" — price shown only after contact
        [Display(Name = "Preço")]
        public decimal? Preco { get; set; }

        [Display(Name = "Duração (min)")]
        public int DuracaoMinutos { get; set; } = 30;

        public bool Ativo { get; set; } = true;

        public CategoriaServico? Categoria { get; set; }
    }
}
