using AutoMapper;
using HotelListing.CountryDto;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Model;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
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
        [Route("GetCountries")]
        [HttpCacheExpiration(CacheLocation =CacheLocation.Public,MaxAge =60)]
        [HttpCacheValidation(MustRevalidate =false)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ResponseCache(CacheProfileName = "120SecondsDuration")]  
        public async Task<IActionResult> GetCountries([FromQuery] RequestParams request)
        {
                var Countries = await _unitOfWork.Countries.GetPageList(null,request);
                var result = _mapper.Map<List<CountryDto.CountryDto>>(Countries);
                return Ok(result);
        }
        [HttpGet("{id:int}",Name = "GetCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id )
        {
                var country = await _unitOfWork.Countries.Get(p=>p.Id==id,new List<string> { "Hotels"});
                var result = _mapper.Map<CountryDto.CountryDto>(country);
                return Ok(result);
           
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route("CreateCountry")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDto model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid Post Attemp In {nameof(CreateCountry)}");
                return BadRequest(ModelState);

            }

                var country= _mapper.Map<Country>(model);
                await _unitOfWork.Countries.Insert(country);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
        }
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id:int}", Name = "UpdateCountry")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountry CountryDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid update Attemp In {nameof(UpdateCountry)}");
                return BadRequest(ModelState);

            }
                var country = await _unitOfWork.Countries.Get(q => q.Id.Equals(id));
                if (country is null)
                {
                    _logger.LogError($"Invalid update Attemp In {nameof(UpdateCountry)}");
                    return BadRequest("submitted date is invalid ");
                }
                _mapper.Map(CountryDto, country);
                _unitOfWork.Countries.Update(country);
                await _unitOfWork.Save();
                return NoContent();

        }
    }
}
