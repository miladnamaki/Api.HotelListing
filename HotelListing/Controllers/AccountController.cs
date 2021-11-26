using AutoMapper;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Model;
using HotelListing.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class AccountController : ControllerBase

    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly SignInManager<ApiUser>  _signInManager;
        //chon singinmanager coockie mehvar hast nemitonim to api azash estefade konim baraye hamin mirim soraghe token 

        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;
        private readonly IAuthManager _authManager;

        public AccountController(UserManager<ApiUser> userManager, SignInManager<ApiUser> signInManager, IMapper mapper, ILogger<AccountController> logger, IAuthManager authManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
            _authManager = authManager;
        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {

            _logger.LogInformation($"Registration Attemp for {userDto.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userr = _mapper.Map<ApiUser>(userDto);
                userr.UserName = userDto.Email;
                var reuslt = await _userManager.CreateAsync(userr, userDto.ConfrimPassword);
                if (!reuslt.Succeeded)
                {
                    foreach (var item in reuslt.Errors)
                    {
                        ModelState.AddModelError(item.Code, item.Description);
                        
                    }
                    return BadRequest(ModelState);
                }
                    var resultRole = await _userManager.AddToRolesAsync(userr, userDto.Roles);
                    if (!resultRole.Succeeded)
                    {
                        foreach (var item in resultRole.Errors)
                        {
                            ModelState.AddModelError(item.Code, item.Description);

                        }
                        return BadRequest(ModelState);
                    }
  
                return Accepted();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Somting Wn wrong in the {nameof(Register)} ");
                return Problem($"Somting went Worng in the {nameof(Register)}",statusCode:500);

                throw;
            }
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto userDto)
        {
            _logger.LogInformation($"Login Attemp for {userDto.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (!await _authManager.ValidateUser(userDto))
                {
                    return Unauthorized();
                }
                return Accepted( new { Token = await _authManager.CreateToken(userDto.Email)});

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Somting Wn wrong in the {nameof(Login)} ");
                return Problem($"Somting went Worng in the {nameof(Login)}", statusCode: 500);

                throw;
            }
        }
    }
}
