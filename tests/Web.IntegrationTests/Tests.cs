using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Models;

namespace Web.IntegrationTests;

public class Tests
{
    private ApiClient _apiClient;

    [SetUp]
    public void Setup()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>();
        var httpClient = webApplicationFactory.CreateClient();

        _apiClient = new ApiClient(httpClient);
    }

    [Test]
    public async Task GetShares_ShouldReturnAllValues()
    {
        var response = await _apiClient.GetShares();

        response.Should().BeSuccessful();

        var shares = await response.Content.ReadFromJsonAsync<ShareLot[]>();
        shares.Should().HaveCount(3);
    }

    [Test]
    public async Task CalculateRemainingShares_ShouldReturnAllValues()
    {
        var response = await _apiClient.CalculateRemainingShares(30, 20, CostStrategy.FIFO);

        response.Should().BeSuccessful();
    }
    
    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(1000000)]
    public async Task CalculateRemainingShares_ShouldReturnAllValues_WhenInvalidShares(int sharesToSell)
    {
        var response = await _apiClient.CalculateRemainingShares(sharesToSell, 20, CostStrategy.FIFO);

        response.Should().HaveClientError();
    }    
    
    [Test]
    [TestCase(CostStrategy.Unknown)]
    [TestCase(CostStrategy.LIFO)]
    [TestCase(CostStrategy.AverageCost)]
    [TestCase(CostStrategy.LowestTaxExposure)]
    [TestCase(CostStrategy.HighestTaxExposure)]
    [TestCase(CostStrategy.LotBased)]
    public async Task CalculateRemainingShares_ShouldReturnAllValues_WhenUnsupportedStrategy(CostStrategy strategy)
    {
        var response = await _apiClient.CalculateRemainingShares(10, 20, strategy);

        response.Should().HaveClientError();
    }
    
    [TearDown]
    public void TearDown()
    {
        _apiClient.Dispose();
    }
}
