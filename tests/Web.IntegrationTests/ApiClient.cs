using Models;

namespace Web.IntegrationTests;

public class ApiClient(HttpClient client) : IDisposable
{
    public async Task<HttpResponseMessage> GetShares()
    {
        var message = await client.GetAsync("api/shares");

        return message;
    }

    public async Task<HttpResponseMessage> CalculateCostDetails(int sharesToSell, decimal salePrice,
        CostStrategy costStrategy)
    {
        var message = await client.GetAsync(
            $"api/Shares/costDetails?sharesToSell={sharesToSell}&salePrice={salePrice}&costStrategy={costStrategy}");

        return message;
    }

    public void Dispose()
    {
        client?.Dispose();
    }
}
