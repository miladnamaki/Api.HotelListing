using HotelListing.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.CountryDto
{
    public class CreateHoTelDto
    {
        [Required]
        [StringLength(maximumLength:150,
            ErrorMessage ="NameHotel Is to Long")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 250,
            ErrorMessage = "Address Hotel Is to Long")]
        public string Address { get; set; }
        [Required]
        [Range(1,5)]
        public double Rating { get; set; }
        [Required]
        public int CountryId { get; set; }
    }
    public class HotelDto : CreateHoTelDto
    {
        public int Id { get; set; }
        public CountryDto Country { get; set; }


    }
}
