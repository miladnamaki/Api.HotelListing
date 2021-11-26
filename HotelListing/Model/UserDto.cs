using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Model
{
    public class UserDto:LoginDto
    {
        public string FirstName { get; set; }
        public string  LastName { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(LoginDto.Password))]
        public string ConfrimPassword { get; set; }
     
        public ICollection<string> Roles { get; set; }

       

        
    }
}
