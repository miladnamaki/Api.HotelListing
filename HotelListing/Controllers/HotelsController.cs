using AutoMapper;
using HotelListing.CountryDto;
using HotelListing.Data;
using HotelListing.IRepository;
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
        [Authorize]
        [HttpGet("{id:int}",Name = "GetHotel")]
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
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route("CreateHotel")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> CreateHotel([FromBody] CreateHoTelDto model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid Post Attemp In {nameof(CreateHotel)}");
                return BadRequest(ModelState);

            }
            try
            {

                var hotel = _mapper.Map<Hotel>(model);
                await _unitOfWork.Hotels.Insert(hotel);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetHotel", new { id = hotel.Id },hotel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Somting went worng is the  {nameof(CreateHotel)}");
                return StatusCode(500, "internal server error , please try again later .");

            }
        }
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id:int}", Name = "UpdateHotel")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDto hoTelDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid update Attemp In {nameof(UpdateHotel)}");
                return BadRequest(ModelState);

            }
            try
            {
                var hotel = await _unitOfWork.Hotels.Get(q => q.Id.Equals(id));
                if (hotel is null)
                {
                    _logger.LogError($"Invalid update Attemp In {nameof(UpdateHotel)}");
                    return BadRequest("submitted date is invalid ");
                }
                _mapper.Map(hoTelDto, hotel);
                _unitOfWork.Hotels.Update(hotel);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Somting went worng is the  {nameof(CreateHotel)}");
                return StatusCode(500, "internal server error , please try again later .");

            }
        }
    }
}
