using System.ComponentModel.DataAnnotations;

namespace NotasApi.Models
{
    public class Nota
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Aluno { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Disciplina { get; set; } = string.Empty;

        [Range(0, 10)]
        public decimal Valor { get; set; }

        public DateTime DataLancamento { get; set; } = DateTime.UtcNow;
    }
}
