using System.ComponentModel.DataAnnotations;

namespace BarberShop.Models
{
    public class Agendamento
    {
        public int AgendamentoId { get; set; }

        public int UsuarioId { get; set; }
        public int BarbeiroId { get; set; }
        public int ServicoId { get; set; }

        [Required]
        [Display(Name = "Data e Hora")]
        public DateTime DataHora { get; set; }

        // Possible values: Confirmado, Cancelado, Concluido
        [MaxLength(20)]
        public string Status { get; set; } = "Confirmado";

        // null when service price is "A consultar"
        [Display(Name = "Valor Total")]
        public decimal? ValorTotal { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "Nome do Cliente")]
        public string NomeCliente { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        [Display(Name = "Telefone")]
        public string TelefoneCliente { get; set; } = string.Empty;

        [MaxLength(500)]
        [Display(Name = "Observações")]
        public string? Observacoes { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public Usuario? Usuario { get; set; }
        public Barbeiro? Barbeiro { get; set; }
        public Servico? Servico { get; set; }
    }
}
