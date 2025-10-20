namespace RoomNest.API.OperationFilter
{
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class RequestExamplesFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.RequestBody == null)
                return;

            // Get the method being documented
            var method = context.MethodInfo;

            // Check if this is a booking creation endpoint
            if (method.Name == "CreateBooking" || method.Name.Contains("Booking"))
            {
                var mediaType = operation.RequestBody.Content["application/json"];
                if (mediaType == null)
                    return;

                // Set the example
                mediaType.Example = new Microsoft.OpenApi.Any.OpenApiObject
                {
                    ["hotelId"] = new Microsoft.OpenApi.Any.OpenApiInteger(1),
                    ["roomIds"] = new Microsoft.OpenApi.Any.OpenApiArray
                    {
                        new Microsoft.OpenApi.Any.OpenApiInteger(101),
                        new Microsoft.OpenApi.Any.OpenApiInteger(102)
                    },
                    ["checkInDate"] = new Microsoft.OpenApi.Any.OpenApiString("2025-10-25"),
                    ["checkOutDate"] = new Microsoft.OpenApi.Any.OpenApiString("2025-10-28"),
                    ["numberOfGuests"] = new Microsoft.OpenApi.Any.OpenApiInteger(2),
                    ["guest"] = new Microsoft.OpenApi.Any.OpenApiObject
                    {
                        ["guestName"] = new Microsoft.OpenApi.Any.OpenApiString("John Smith"),
                        ["guestEmail"] = new Microsoft.OpenApi.Any.OpenApiString("john.smith@example.com"),
                        ["guestPhone"] = new Microsoft.OpenApi.Any.OpenApiString("+44-123-456-7890")
                    }
                };
            }

            if (method.Name == "CheckAvailability")
            {
                var mediaType = operation.RequestBody.Content["application/json"];
                if (mediaType == null)
                    return;

                // Set default example
                mediaType.Example = new OpenApiObject
                {
                    ["hotelId"] = new OpenApiInteger(1),
                    ["checkInDate"] = new OpenApiString("2025-10-25T14:00:00Z"),
                    ["checkOutDate"] = new OpenApiString("2025-10-28T11:00:00Z"),
                    ["guestCount"] = new OpenApiInteger(2)
                };

                // Add multiple example
                mediaType.Examples ??= new Dictionary<string, OpenApiExample>();

                // Example 1: Couple's short stay
                mediaType.Examples["couple-weekend"] = new OpenApiExample
                {
                    Summary = "Couple's weekend getaway",
                    Description = "Weekend availability check for 2 guests (Friday to Sunday)",
                    Value = new OpenApiObject
                    {
                        ["hotelId"] = new OpenApiInteger(1),
                        ["checkInDate"] = new OpenApiString("2025-10-25T14:00:00Z"),
                        ["checkOutDate"] = new OpenApiString("2025-10-26T11:00:00Z"),
                        ["guestCount"] = new OpenApiInteger(2)
                    }
                };

                // Example 2: Family week-long stay
                mediaType.Examples["family-week"] = new OpenApiExample
                {
                    Summary = "Family week-long holiday",
                    Description = "Full week availability for a family of 4",
                    Value = new OpenApiObject
                    {
                        ["hotelId"] = new OpenApiInteger(2),
                        ["checkInDate"] = new OpenApiString("2025-11-01T14:00:00Z"),
                        ["checkOutDate"] = new OpenApiString("2025-11-08T11:00:00Z"),
                        ["guestCount"] = new OpenApiInteger(4)
                    }
                };
            }
        }
    }
}