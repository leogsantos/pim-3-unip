using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShop.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        public int UsuarioId { get; set; }

        [Column("Nome")]
        private string nome = string.Empty;
        [Required, MaxLength(100)]
        [Display(Name = "Nome")]
        public string Nome { get => nome; set => nome = value; }

        [Column("Email")]
        private string email = string.Empty;
        [Required, MaxLength(150), EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get => email; set => email = value; }

        [Column("Telefone")]
        private string telefone = string.Empty;
        [Required, MaxLength(20)]
        [Display(Name = "Telefone")]
        public string Telefone { get => telefone; set => telefone = value; }

        [Column("Senha")]
        private string senha = string.Empty;
        [Required, MaxLength(255)]
        public string Senha { get => senha; set => senha = value; }

        // Valores possíveis: "Cliente" ou "Admin"
        [Column("TipoUsuario")]
        private string tipoUsuario = "Cliente";
        [MaxLength(20)]
        public string TipoUsuario { get => tipoUsuario; set => tipoUsuario = value; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public bool Ativo { get; set; } = true;
    }
}
