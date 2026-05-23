using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShop.Models
{
    [Table("Barbeiro")]
    public class Barbeiro
    {
        public int BarbeiroId { get; set; }

        [Column("Nome")]
        private string nome = string.Empty;
        [Required, MaxLength(100)]
        [Display(Name = "Nome")]
        public string Nome { get => nome; set => nome = value; }

        // Sigla curta exibida na grade de agendamentos (ex: "JV")
        [Column("Iniciais")]
        private string iniciais = string.Empty;
        [Required, MaxLength(5)]
        public string Iniciais { get => iniciais; set => iniciais = value; }

        // Nullable — telefone é opcional no cadastro do barbeiro
        [Column("Telefone")]
        private string? telefone;
        [MaxLength(20)]
        [Display(Name = "Telefone")]
        public string? Telefone { get => telefone; set => telefone = value; }

        // Nullable — e-mail é opcional no cadastro do barbeiro
        [Column("Email")]
        private string? email;
        [MaxLength(150), EmailAddress]
        [Display(Name = "E-mail")]
        public string? Email { get => email; set => email = value; }

        public bool Ativo { get; set; } = true;
    }
}
