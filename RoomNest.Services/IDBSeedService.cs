
namespace RoomNest.Services
{
    public interface IDBSeedService
    {
        Task ResetAsync();
        Task SeedAsync();
    }
}