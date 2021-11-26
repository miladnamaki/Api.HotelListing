using AutoMapper;
using HotelListing.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/Country")]
    [ApiController]
    public class CountryV2Controller : ControllerBase
    {
        private  readonly  IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CountryV2Controller(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("GetCountries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries()
        {
            var Countries = await _unitOfWork.Countries.GetAll();
            var result = _mapper.Map<List<CountryDto.CountryDto>>(Countries);
            return Ok(result);
        }
    }
}
