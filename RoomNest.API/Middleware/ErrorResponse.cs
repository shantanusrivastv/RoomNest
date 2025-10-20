namespace RoomNest.API.Middleware
{
    public class ErrorResponse
    {
        public required int Status { get; set; }
        public required string Error { get; set; }
    }
}