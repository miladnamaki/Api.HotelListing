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
        }

    }
}
