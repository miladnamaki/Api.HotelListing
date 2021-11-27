using AspNetCoreRateLimit;
using HotelListing.Data;
using HotelListing.Model;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HotelListing
{
    public static class ServicesExtantion
    {
        public static void ConfigureIdentity(this IServiceCollection services)
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

        public static void ConfigureExeptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextfeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextfeature!=null)
                    {
                        Log.Error($"SomeThing Went Wrong in the {contextfeature.Error}");
                        await context.Response.WriteAsync(new Error()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message="Internal Server Error . Please Try Again Later .",

                        }.ToString());
                    }
                });
            });
        }
        public static void ConfiugureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt=>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
        }
        public static void CounfigureHttpCacheHeaders(this IServiceCollection services)
        {
            services.AddResponseCaching();
            services.AddHttpCacheHeaders(
                (expiretonopt) =>
                {
                    expiretonopt.MaxAge = 65;
                    expiretonopt.CacheLocation = CacheLocation.Private;
                },
                (validationopt) =>
                {
                    validationopt.MustRevalidate = true;
                }
               );
             
        }
        public static void CounfigureRateLimiting(this IServiceCollection services)
        {
            var ratelimitrules = new List<RateLimitRule>()
            {
                new RateLimitRule()
                {
                    Endpoint= "*", //baryae hameye endpoint hast 
                    Limit=1, //baraye yekbar call kardan 
                    Period="10s",//1 sanie zamn bayad sabr kone ..
                                 //ke khili kame 10s khube ,10m ham mitonim bezarim 
              
                }
            };
            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = ratelimitrules;
            });
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton < IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

        }
    }
}
