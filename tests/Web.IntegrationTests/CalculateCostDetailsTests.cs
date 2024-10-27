using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Models;

namespace Web.IntegrationTests;

public class CalculateCostDetailsTests
{
    private readonly ShareLot[] _shareLots;
    private ApiClient _apiClient;

    public CalculateCostDetailsTests()
    {
        var january = new DateTime(2024, 1, 1);

        _shareLots =
        [
            new ShareLot(100, 20, january),
            new ShareLot(150, 30, january.AddMonths(1)),
            new ShareLot(120, 10, january.AddMonths(2))
        ];
    }

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
        shares.Should().BeEquivalentTo(_shareLots);
    }

    [Test]
    public async Task CalculateCostDetails_ShouldReturnAllValues()
    {
        var response = await _apiClient.CalculateCostDetails(30, 20, CostStrategy.FIFO);

        response.Should().BeSuccessful();
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public async Task CalculateCostDetails_ShouldReturnAllValues_WhenInvalidShares(int sharesToSell)
    {
        var response = await _apiClient.CalculateCostDetails(sharesToSell, 20, CostStrategy.FIFO);

        response.Should().HaveClientError();

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Detail.Should().Be("sharesToSell must be greater than 0.");
    }

    [Test]
    [TestCase(CostStrategy.Unknown)]
    [TestCase(CostStrategy.LIFO)]
    [TestCase(CostStrategy.AverageCost)]
    [TestCase(CostStrategy.LowestTaxExposure)]
    [TestCase(CostStrategy.HighestTaxExposure)]
    [TestCase(CostStrategy.LotBased)]
    public async Task CalculateCostDetails_ShouldReturnAllValues_WhenUnsupportedStrategy(CostStrategy strategy)
    {
        var response = await _apiClient.CalculateCostDetails(10, 20, strategy);

        response.Should().HaveClientError();

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Detail.Should().Be($"Only {CostStrategy.FIFO} strategy is supported.");
    }

    [Test]
    public async Task CalculateCostDetails_ShouldReturnAllValues_WhenExceedsAvailableShares()
    {
        int sharesToSell = _shareLots.Sum(shareLot => shareLot.Quantity);
        var response = await _apiClient.CalculateCostDetails(sharesToSell + 1, 20, CostStrategy.FIFO);

        response.Should().HaveClientError();

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Detail.Should().Be("Requested shares to sell exceeds available shares.");
    }

    [TearDown]
    public void TearDown()
    {
        _apiClient.Dispose();
    }
}
