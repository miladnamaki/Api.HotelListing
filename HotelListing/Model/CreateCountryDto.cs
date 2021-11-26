using System.ComponentModel.DataAnnotations;

namespace HotelListing.CountryDto
{
    public class CreateCountryDto
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Country Name Is To Long")]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 2, ErrorMessage = "ShortName Is To Long")]
        public string NameShort { get; set; }
    }
    public class UpdateCountry : CreateCountryDto
    {

    }
}
