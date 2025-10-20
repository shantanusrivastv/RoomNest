namespace RoomNest.Services.Interfaces
{
    public interface IDBSeedService
    {
        Task ResetAsync();
        Task SeedAsync();
    }
}