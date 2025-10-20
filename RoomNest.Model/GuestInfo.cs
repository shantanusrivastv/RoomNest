using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomNest.DTO
{
    /// <summary>
    /// Guest information associated with a booking
    /// </summary>
    public class GuestInfo
    {
        /// <summary>
        /// Full name of the guest
        /// </summary>
        /// <example>John Smith</example>
        [Required(ErrorMessage = "Guest name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Guest name must be between 2 and 100 characters")]
        public string GuestName { get; set; }

        /// <summary>
        /// Email address of the guest for communication
        /// </summary>
        /// <example>john.smith@example.com</example>
        [Required(ErrorMessage = "Guest email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string GuestEmail { get; set; }

        /// <summary>
        /// Contact phone number of the guest
        /// </summary>
        /// <example>+44-20-7946-0958</example>
        [Required(ErrorMessage = "Guest phone is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string GuestPhone { get; set; }
    }
}
