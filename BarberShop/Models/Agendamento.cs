using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShop.Models
{
    [Table("Agendamento")]
    public class Agendamento
    {
        public int AgendamentoId { get; set; }

        public int UsuarioId { get; set; }
        public int BarbeiroId { get; set; }
        public int ServicoId { get; set; }

        [Required]
        [Display(Name = "Data e Hora")]
        public DateTime DataHora { get; set; }

        // Valores possíveis: Confirmado, Cancelado, Concluido
        [Column("Status")]
        private string status = "Confirmado";
        [MaxLength(20)]
        public string Status { get => status; set => status = value; }

        // null quando o preço do serviço é "A consultar"
        [Display(Name = "Valor Total")]
        public decimal? ValorTotal { get; set; }

        [Column("NomeCliente")]
        private string nomeCliente = string.Empty;
        [Required, MaxLength(100)]
        [Display(Name = "Nome do Cliente")]
        public string NomeCliente { get => nomeCliente; set => nomeCliente = value; }

        [Column("TelefoneCliente")]
        private string telefoneCliente = string.Empty;
        [Required, MaxLength(20)]
        [Display(Name = "Telefone")]
        public string TelefoneCliente { get => telefoneCliente; set => telefoneCliente = value; }

        // Nullable — preenchimento opcional pelo cliente
        [Column("Observacoes")]
        private string? observacoes;
        [MaxLength(500)]
        [Display(Name = "Observações")]
        public string? Observacoes { get => observacoes; set => observacoes = value; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public Usuario? Usuario { get; set; }
        public Barbeiro? Barbeiro { get; set; }
        public Servico? Servico { get; set; }
    }
}