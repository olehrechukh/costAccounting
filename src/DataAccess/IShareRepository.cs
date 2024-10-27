using Models;

namespace DataAccess;

public interface IShareRepository
{
    Task<ShareLot[]> GetShares();
}