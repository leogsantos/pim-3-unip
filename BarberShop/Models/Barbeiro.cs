using System.ComponentModel.DataAnnotations;

namespace BarberShop.Models
{
    public class Barbeiro
    {
        public int BarbeiroId { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        // Short initials shown on the schedule grid (e.g. "JV")
        [Required, MaxLength(5)]
        public string Iniciais { get; set; } = string.Empty;

        [MaxLength(20)]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [MaxLength(150), EmailAddress]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        public bool Ativo { get; set; } = true;
    }
}
