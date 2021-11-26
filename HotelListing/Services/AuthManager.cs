using HotelListing.Data;
using HotelListing.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace HotelListing.Services
{
    public class AuthManager : IAuthManager
    {
        private UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private ApiUser _user;

        public AuthManager(UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> CreateToken(string username)
        {
            
            var singcrentials = GetSignInCredential();
            var Claims = await GetClaims(username);
            var TokenOption = GenreateTokenOption(singcrentials, Claims);
            return new JwtSecurityTokenHandler().WriteToken(TokenOption);
        }

        private JwtSecurityToken GenreateTokenOption(SigningCredentials getSignInCredential, List<Claim> claimss)
        {
            var jwtsetting = _configuration.GetSection("JWT");
            var tokens = new JwtSecurityToken(
                issuer: jwtsetting.GetSection("Issuer").Value,
                claims: claimss,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtsetting.GetSection("LifeTime").Value)),
                signingCredentials: getSignInCredential
                );
            return tokens;
        }
        private async Task<List<Claim>> GetClaims( string username)
        {
            var _user = await _userManager.FindByNameAsync(username);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,_user.UserName),
            };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));


            }
            return claims;
        }

        private SigningCredentials GetSignInCredential()
        {
            var key = Environment.GetEnvironmentVariable("KEY");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            return new Microsoft.IdentityModel.Tokens.SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        public async Task<bool> ValidateUser(LoginDto userDto)
        {
            var _user = await _userManager.FindByNameAsync(userDto.Email);
            return (_user != null && await _userManager.CheckPasswordAsync(_user, userDto.Password));

                 
        }
    }
}
