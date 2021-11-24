using AutoMapper;
using HotelListing.CountryDto;
using HotelListing.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper  _mapper;
        private readonly ILogger<CountryController> _logger;
        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
         
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var Countries = await _unitOfWork.Countries.GetAll();
                var result = _mapper.Map<List<CountryDto.CountryDto>>(Countries);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"SomeThing Went Wrong in the {nameof(GetCountries)}");
                return StatusCode(500,"Internal server error . pleas try again later .");

            }
        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id )
        {
            try
            {
                var country = await _unitOfWork.Countries.Get(p=>p.Id==id,new List<string> { "Hotels"});
                var result = _mapper.Map<CountryDto.CountryDto>(country);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"SomeThing Went Wrong in the {nameof(GetCountry)}"); 
                return StatusCode(500, "Internal server error . pleas try again later .");

            }
        }
    }
}
