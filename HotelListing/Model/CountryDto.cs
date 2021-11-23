using HotelListing.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.CountryDto
{
    public class CountryDto:CreateCountryDto
    {
        public int Id { get; set; }
        public   IList<HotelDto> Hotels { get; set; }

    }
}
