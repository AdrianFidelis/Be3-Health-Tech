using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using FluentAssertions;
using CadastroPacientes.Application.Services;
using CadastroPacientes.Application.DTOs;
using CadastroPacientes.Domain.Entities;
using CadastroPacientes.Domain.Interfaces;
using System.Collections.Generic;
using CadastroPacientes.Domain.Enums;

namespace CadastroPacientes.Tests.UnitTests
{
    public class PacienteServiceTests
    {
        private readonly Mock<IPacienteRepository> _repoMock;
        private readonly PacienteService _service;

        public PacienteServiceTests()
        {
            _repoMock = new Mock<IPacienteRepository>();
            _service = new PacienteService(_repoMock.Object);
        }

        [Fact]
        public async Task CriarAsync_ShouldReturnFalse_WhenCpfDuplicate()
        {
            // Arrange
            var dto = new CreatePacienteDto
            {
                Nome = "Teste",
                Sobrenome = "Unitario",
                DataNascimento = DateTime.Today.AddYears(-30),
                Genero = Genero.Masculino,
                CPF = "123.456.789-00",
                RG = "MG-12.345.678",
                UfRg = UF.MG,
                Email = "teste@unitario.com",
                Celular = null,
                TelefoneFixo = null,
                ConvenioId = 1,
                NumeroCarteirinha = "ABC123",
                ValidadeCarteirinha = "12/2025"
            };
            _repoMock.Setup(r => r.ExisteCpfAsync(dto.CPF)).ReturnsAsync(true);

            // Act
            var (sucesso, novoId) = await _service.CriarAsync(dto);

            // Assert
            sucesso.Should().BeFalse();
            novoId.Should().Be(Guid.Empty);
            _repoMock.Verify(r => r.AdicionarAsync(It.IsAny<Paciente>()), Times.Never);
        }

        [Fact]
        public async Task CriarAsync_ShouldCreate_WhenValidDto()
        {
            // Arrange
            var dto = new CreatePacienteDto
            {
                Nome = "Teste",
                Sobrenome = "Unitario",
                DataNascimento = DateTime.Today.AddYears(-30),
                Genero = Genero.Masculino,
                CPF = "123.456.789-00",
                RG = "MG-12.345.678",
                UfRg = UF.MG,
                Email = "teste@unitario.com",
                Celular = null,
                TelefoneFixo = null,
                ConvenioId = 1,
                NumeroCarteirinha = "ABC123",
                ValidadeCarteirinha = "12/2025"
            };
            _repoMock.Setup(r => r.ExisteCpfAsync(dto.CPF)).ReturnsAsync(false);
            _repoMock.Setup(r => r.AdicionarAsync(It.IsAny<Paciente>())).Returns(Task.CompletedTask);

            // Act
            var (sucesso, novoId) = await _service.CriarAsync(dto);

            // Assert
            sucesso.Should().BeTrue();
            novoId.Should().NotBe(Guid.Empty);
            _repoMock.Verify(r => r.AdicionarAsync(It.Is<Paciente>(p =>
                p.Nome == dto.Nome &&
                p.CPF == dto.CPF
            )), Times.Once);
        }

        [Fact]
        public async Task AtualizarAsync_ShouldReturnFalse_WhenNotFound()
        {
            // Arrange
            var dto = new UpdatePacienteDto { Id = Guid.NewGuid() };
            _repoMock.Setup(r => r.ObterPorIdAsync(dto.Id)).ReturnsAsync((Paciente)null);

            // Act
            var result = await _service.AtualizarAsync(dto);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task AtualizarAsync_ShouldReturnFalse_WhenCpfDuplicate()
        {
            // Arrange
            var existing = new Paciente { Id = Guid.NewGuid(), CPF = "111.111.111-11" };
            var dto = new UpdatePacienteDto
            {
                Id = existing.Id,
                Nome = "Teste",
                Sobrenome = "Unitario",
                DataNascimento = DateTime.Today.AddYears(-30),
                Genero = Genero.Masculino,
                CPF = "222.222.222-22", // novo CPF
                RG = "MG-12.345.678",
                UfRg = UF.MG,
                Email = "teste@unitario.com",
                Celular = null,
                TelefoneFixo = null,
                ConvenioId = 1,
                NumeroCarteirinha = "ABC123",
                ValidadeCarteirinha = "12/2025",
                Ativo = true
            };
            _repoMock.Setup(r => r.ObterPorIdAsync(dto.Id)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.ExisteCpfAsync(dto.CPF)).ReturnsAsync(true);

            // Act
            var result = await _service.AtualizarAsync(dto);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task AtualizarAsync_ShouldUpdate_WhenValidDto()
        {
            // Arrange
            var existing = new Paciente
            {
                Id = Guid.NewGuid(),
                Nome = "Old",
                Sobrenome = "Paciente",
                DataNascimento = DateTime.Today.AddYears(-30),
                Genero = Genero.Masculino,
                CPF = "111.111.111-11",
                RG = "MG-12.345.678",
                UfRg = UF.MG,
                Email = "old@unitario.com",
                Celular = null,
                TelefoneFixo = null,
                ConvenioId = 1,
                NumeroCarteirinha = "ABC123",
                ValidadeCarteirinha = "12/2025",
                Ativo = true
            };
            var dto = new UpdatePacienteDto
            {
                Id = existing.Id,
                Nome = "New",
                Sobrenome = "Paciente",
                DataNascimento = DateTime.Today.AddYears(-30),
                Genero = Genero.Masculino,
                CPF = "111.111.111-11", // mesmo CPF
                RG = "MG-12.345.678",
                UfRg = UF.MG,
                Email = "new@unitario.com",
                Celular = null,
                TelefoneFixo = null,
                ConvenioId = 1,
                NumeroCarteirinha = "ABC123",
                ValidadeCarteirinha = "12/2025",
                Ativo = true
            };
            _repoMock.Setup(r => r.ObterPorIdAsync(dto.Id)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.ExisteCpfAsync(dto.CPF)).ReturnsAsync(false);
            _repoMock.Setup(r => r.AtualizarAsync(existing)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.AtualizarAsync(dto);

            // Assert
            result.Should().BeTrue();
            existing.Nome.Should().Be("New");
            _repoMock.Verify(r => r.AtualizarAsync(existing), Times.Once);
        }

        [Fact]
        public async Task InativarAsync_ShouldReturnFalse_WhenNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _repoMock.Setup(r => r.ObterPorIdAsync(id)).ReturnsAsync((Paciente)null);

            // Act
            var result = await _service.InativarAsync(id);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task InativarAsync_ShouldCallRepository_WhenFound()
        {
            // Arrange
            var existing = new Paciente { Id = Guid.NewGuid(), Ativo = true };
            _repoMock.Setup(r => r.ObterPorIdAsync(existing.Id)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.InativarAsync(existing.Id)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.InativarAsync(existing.Id);

            // Assert
            result.Should().BeTrue();
            _repoMock.Verify(r => r.InativarAsync(existing.Id), Times.Once);
        }
    }
}
