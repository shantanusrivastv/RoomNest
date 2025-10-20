namespace RoomNest.Services.Interfaces
{
    public interface IDbSeedService
    {
        Task ResetAsync();
        Task SeedAsync();
    }
}