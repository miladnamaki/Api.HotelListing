using AutoMapper;
using HotelListing.CountryDto;
using HotelListing.Data;
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
    public class HotelsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<HotelsController> _logger;

        public HotelsController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<HotelsController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var hotels = await _unitOfWork.Hotels.GetAll();
                var result = _mapper.Map<IList<HotelDto>>(hotels);
                return Ok(result);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"SomeThing Went Wrong in the {nameof(GetHotels)}");
                return StatusCode(500);
            }
        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetHotel( int id )
        {
            try
            {
                var hotel = await _unitOfWork.Hotels.Get(p=>p.Id==id , new List<string> { "Country" });
                var result = _mapper.Map<HotelDto>(hotel);
                return Ok(result);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"SomeThing Went Wrong in the {nameof(GetHotel)}");
                return StatusCode(500);
            }
        }
    }
}
