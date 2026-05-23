using System.ComponentModel.DataAnnotations;

namespace BarberShop.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required, MaxLength(150), EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string Senha { get; set; } = string.Empty;

        // "Cliente" or "Admin"
        [MaxLength(20)]
        public string TipoUsuario { get; set; } = "Cliente";

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public bool Ativo { get; set; } = true;
    }
}
