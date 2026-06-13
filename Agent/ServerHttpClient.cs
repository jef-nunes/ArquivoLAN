using System.Text;
using System.Text.Json;
using Contracts;
using Contracts.Dtos.Request;

namespace Agent;

public class ServerHttpClient
{
    private readonly HttpClient _httpClient = new();
    private readonly ServerConnectionContract _contract;
    
    public ServerHttpClient()
    {
        _contract = ServerConnectionContractFactory.Create(
            AgentEnvironmentService.GetServerUrl());
    }

    public async Task<bool> IsServerAvailableAsync()
    {
        var url = new Uri(
            new Uri(_contract.ServerUrl),
            _contract.HealthEndpoint);

        var response = await _httpClient.GetAsync(url);

        return response.IsSuccessStatusCode;
    }

    public async Task PostGatheringAsync(
        GatheringRequestDto payload)
    {
        var url = new Uri(
            new Uri(_contract.ServerUrl),
            _contract.GatheringEndpoint);

        var json = JsonSerializer.Serialize(payload);

        var content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync(url, content);

        response.EnsureSuccessStatusCode();
    }
}