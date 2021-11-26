using HotelListing.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HotelListing
{
    public static class ServicesExtantion
    {
        public static void COnfigureIdentity(this IServiceCollection services)
        {

            var builder = services.AddIdentity<ApiUser,IdentityRole>(q =>
            {
                q.User.RequireUniqueEmail = true;

            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<DataBaseContext>().AddDefaultTokenProviders();

        }
        public static void ConfigureJWT(this IServiceCollection services , IConfiguration Configuration )
        {
            var jwtSettings = Configuration.GetSection("Jwt");
            var key = Environment.GetEnvironmentVariable("KEY");
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(o=> 
            {
                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                    IssuerSigningKey= new  SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                   
                    
                };
            });

        }
    }
}
