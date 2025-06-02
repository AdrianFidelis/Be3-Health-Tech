using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Text;
using CadastroPacientes.API;
using CadastroPacientes.Application.DTOs;
using CadastroPacientes.Domain.Enums;

namespace CadastroPacientes.Tests.IntegrationTests
{
    public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Deve_Criar_Consultar_Inativar_Paciente_Com_Sucesso()
        {

            //var cpfUnico = $"{Guid.NewGuid().ToString().Substring(0, 3)}.{Guid.NewGuid().ToString().Substring(4, 3)}.{Guid.NewGuid().ToString().Substring(8, 3)}-00";
            var cpfUnico = $"{Random.Shared.Next(100, 999)}.{Random.Shared.Next(100, 999)}.{Random.Shared.Next(100, 999)}-00";
            // -------- Arrange --------
            var createDto = new CreatePacienteDto
            {
                Nome = "Integracao",
                Sobrenome = "Teste",
                DataNascimento = DateTime.Today.AddYears(-25),
                Genero = Genero.Outro,
                CPF = cpfUnico,
                RG = "SP-12.345.678",
                UfRg = UF.SP,
                Email = "integ@teste.com",
                Celular = "(11)98888-7777",
                TelefoneFixo = null,
                ConvenioId = 1,
                NumeroCarteirinha = "INT123",
                ValidadeCarteirinha = "12/2026"
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(createDto),
                Encoding.UTF8,
                "application/json");

            // -------- Act - POST --------
            var postResponse = await _client.PostAsync("/api/paciente", jsonContent);
            postResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            var postBody = await postResponse.Content.ReadAsStringAsync();
            using var postDoc = JsonDocument.Parse(postBody);
            var novoId = postDoc.RootElement.GetProperty("id").GetGuid();

            // -------- Assert - GET (deve retornar Ativo = true) --------
            var getResponse = await _client.GetAsync($"/api/paciente/{novoId}");
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getBody = await getResponse.Content.ReadAsStringAsync();
            var pacienteJson = JsonDocument.Parse(getBody).RootElement;
            pacienteJson.GetProperty("ativo").GetBoolean().Should().BeTrue();

            // -------- Act - INATIVAR (DELETE lógico) --------
            var deleteResponse = await _client.DeleteAsync($"/api/paciente/{novoId}");
            deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            // -------- Assert - GET após inativar (deve retornar Ativo = false) --------
            var getDeletedResponse = await _client.GetAsync($"/api/paciente/{novoId}");
            getDeletedResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var deletedBody = await getDeletedResponse.Content.ReadAsStringAsync();
            var pacienteInativoJson = JsonDocument.Parse(deletedBody).RootElement;
            pacienteInativoJson.GetProperty("ativo").GetBoolean().Should().BeFalse();            
        }


        [Fact]
        public async Task Nao_Deve_Permitir_Cadastrar_Paciente_Com_CPF_Ja_Existente()
        {
            // Arrange
            var cpfUnico = $"{Random.Shared.Next(100, 999)}.{Random.Shared.Next(100, 999)}.{Random.Shared.Next(100, 999)}-00";
            var createDto = new CreatePacienteDto
            {
                Nome = "Primeiro",
                Sobrenome = "Teste",
                DataNascimento = DateTime.Today.AddYears(-30),
                Genero = Genero.Masculino,
                CPF = cpfUnico,
                RG = "SP-11.222.333",
                UfRg = UF.SP,
                Email = "primeiro@teste.com",
                Celular = "(11)91111-2222",
                TelefoneFixo = null,
                ConvenioId = 1,
                NumeroCarteirinha = "CARD001",
                ValidadeCarteirinha = "12/2026"
            };
            var content1 = new StringContent(JsonSerializer.Serialize(createDto), Encoding.UTF8, "application/json");
            var content2 = new StringContent(JsonSerializer.Serialize(createDto), Encoding.UTF8, "application/json"); // Mesmo CPF!

            // Act - 1º cadastro deve passar
            var post1 = await _client.PostAsync("/api/paciente", content1);
            post1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            // Act - 2º cadastro deve falhar por CPF duplicado
            var post2 = await _client.PostAsync("/api/paciente", content2);

            // Assert
            post2.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict); // 409 Conflict é padrão para duplicidade, mas veja se sua API retorna isso.
            var respostaErro = await post2.Content.ReadAsStringAsync();
            respostaErro.Should().Contain("CPF já cadastrado");
        }

    }
}
