using System.ComponentModel.DataAnnotations;

namespace HotelListing.Model
{
    public class LoginDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]

        public string Email { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "your Password IS limited To {2} to {1} charecter ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
