using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using MachineManagement.API.Entities;
using MachineManagement.API.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace MachineManagement.Tests.Integration;

public class MachineIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;
    
    public MachineIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task CreateMachine_InvalidName_ShouldReturnBadRequest(string invalidName)
    {
        // Arrange
        var createDto = new CreateMachineDto { Name = invalidName };
        var content = new StringContent(
            JsonSerializer.Serialize(createDto, _jsonOptions),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/api/machine", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateMachine_DuplicateName_ShouldReturnConflict()
    {
        // Arrange
        var uniqueName = $"CNC masina_134";
        var createDto = new CreateMachineDto { Name = uniqueName };
            
        var content = new StringContent(
            JsonSerializer.Serialize(createDto, _jsonOptions),
            Encoding.UTF8,
            "application/json");

        // Act
        var firstResponse = await _client.PostAsync("/api/machine", content);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var duplicateContent = new StringContent(
            JsonSerializer.Serialize(createDto, _jsonOptions),
            Encoding.UTF8,
            "application/json");

        var duplicateResponse = await _client.PostAsync("/api/machine", duplicateContent);

        // Assert
        duplicateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            
        var errorMessage = await duplicateResponse.Content.ReadAsStringAsync();
        errorMessage.Should().Contain("already exists");

        // Cleanup
        var firstMachineJson = await firstResponse.Content.ReadAsStringAsync();
        var firstMachine = JsonSerializer.Deserialize<Machine>(firstMachineJson, _jsonOptions);
        await _client.DeleteAsync($"/api/machine/{firstMachine!.Id}");
    }
}