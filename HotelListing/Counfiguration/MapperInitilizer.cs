using AutoMapper;
using HotelListing.CountryDto;
using HotelListing.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Counfiguration
{
    public class MapperInitilizer:Profile
    {
        public MapperInitilizer()
        {
            CreateMap<Country, HotelListing.CountryDto.CountryDto>().ReverseMap();
            CreateMap<Country, HotelListing.CountryDto.CreateCountryDto>().ReverseMap();
            CreateMap<Hotel, HotelListing.CountryDto.HotelDto>().ReverseMap();
            CreateMap<Hotel, HotelListing.CountryDto.CreateHoTelDto>().ReverseMap();
        }

    }
}
