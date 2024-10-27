using Models;

namespace DataAccess;

public class ShareRepository : IShareRepository
{
    private readonly ShareLot[] _shareLots;

    public ShareRepository()
    {
        var january = new DateTime(2024, 1, 1);

        _shareLots =
        [
            new ShareLot(100, 20, january),
            new ShareLot(150, 30, january.AddMonths(1)),
            new ShareLot(120, 10, january.AddMonths(2))
        ];
    }

    public Task<ShareLot[]> GetShares()
    {
        return Task.FromResult(_shareLots);
    }
}
