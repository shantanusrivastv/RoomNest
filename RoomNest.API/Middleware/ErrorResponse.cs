namespace RoomNest.API.Middleware
{
    public class ErrorResponse
    {
        public required int status { get; set; }
        public required string error { get; set; }
    }
}