using Models;

namespace DataAccess;

public interface IShareRepository
{
    Task<ShareLot[]> GetShares();
}

public class ShareRepository : IShareRepository
{
    private readonly ShareLot[] _shareLots;

    public ShareRepository()
    {
        var january = new DateTime(2024, 1, 1);

        _shareLots =
        [
            new ShareLot(100, 20, january),
            new ShareLot(100, 20, january.AddMonths(1))
        ];
    }

    public Task<ShareLot[]> GetShares()
    {
        return Task.FromResult(_shareLots);
    }
}
