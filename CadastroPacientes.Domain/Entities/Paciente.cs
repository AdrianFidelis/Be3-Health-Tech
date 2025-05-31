using CadastroPacientes.Domain.Enums;
using System;

namespace CadastroPacientes.Domain.Entities
{
    public class Paciente
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Sobrenome { get; set; } = null!;
        public DateTime DataNascimento { get; set; }
        public Genero Genero { get; set; }
        public string? CPF { get; set; }
        public string RG { get; set; } = null!;
        public UF UfRg { get; set; }
        public string Email { get; set; } = null!;
        public string? Celular { get; set; }
        public string? TelefoneFixo { get; set; }
        public int ConvenioId { get; set; }
        public string NumeroCarteirinha { get; set; } = null!;
        public string ValidadeCarteirinha { get; set; } = null!; 
        public bool Ativo { get; set; } = true;
    }
}
