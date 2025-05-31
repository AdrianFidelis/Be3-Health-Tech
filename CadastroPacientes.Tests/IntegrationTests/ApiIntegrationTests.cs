// CadastroPacientes.Tests/IntegrationTests/ApiIntegrationTests.cs

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
        public async Task Post_Get_Delete_PacienteFlow()
        {
            // 1) Criar novo paciente
            var createDto = new CreatePacienteDto
            {
                Nome = "Integracao",
                Sobrenome = "Teste",
                DataNascimento = DateTime.Today.AddYears(-25),
                Genero = Genero.Outro,
                CPF = "999.888.777-66",
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

            var postResponse = await _client.PostAsync("/api/paciente", jsonContent);
            postResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            // Extrair ID retornado
            var postBody = await postResponse.Content.ReadAsStringAsync();
            using var postDoc = JsonDocument.Parse(postBody);
            var novoId = postDoc.RootElement.GetProperty("id").GetGuid();

            // 2) GET por ID
            var getResponse = await _client.GetAsync($"/api/paciente/{novoId}");
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getBody = await getResponse.Content.ReadAsStringAsync();

            // ==== OPÇÃO A: Comparar valor numérico do enum ====
            using var getDoc = JsonDocument.Parse(getBody);
            // Lê o campo "genero" como inteiro
            int generoNoJson = getDoc.RootElement.GetProperty("genero").GetInt32();
            generoNoJson.Should().Be((int)Genero.Outro);

            // Também verificar se o nome está presente no JSON
            getBody.Should().Contain("Integracao");

            // 3) DELETE/inativar paciente
            var deleteResponse = await _client.DeleteAsync($"/api/paciente/{novoId}");
            deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            // 4) Verificar que o paciente não aparece em listagem
            var listResponse = await _client.GetAsync("/api/paciente");
            listResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var listBody = await listResponse.Content.ReadAsStringAsync();
            listBody.Should().NotContain(novoId.ToString());
        }
    }
}
