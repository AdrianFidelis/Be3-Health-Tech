using System;
using System.ComponentModel.DataAnnotations;
using CadastroPacientes.Domain.Enums;

namespace CadastroPacientes.Application.DTOs
{
    public class UpdatePacienteDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Nome { get; set; } = null!;

        [Required]
        public string Sobrenome { get; set; } = null!;

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        public Genero Genero { get; set; }

        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}\-\d{2}$",
            ErrorMessage = "CPF deve estar no formato 000.000.000-00")]
        public string? CPF { get; set; }

        [Required]
        public string RG { get; set; } = null!;

        [Required]
        public UF UfRg { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Phone]
        public string? Celular { get; set; }

        [Phone]
        public string? TelefoneFixo { get; set; }

        [Required]
        public int ConvenioId { get; set; }

        [Required]
        public string NumeroCarteirinha { get; set; } = null!;

        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/(20|21)\d{2}$",
            ErrorMessage = "Validade deve estar no formato MM/yyyy")]
        public string ValidadeCarteirinha { get; set; } = null!;

        [Required]
        public bool Ativo { get; set; }
    }
}
