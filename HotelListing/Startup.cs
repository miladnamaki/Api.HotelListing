using AspNetCoreRateLimit;
using HotelListing.Counfiguration;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Repository;
using HotelListing.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddDbContext<DataBaseContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("sqlConnection"));
            });
            services.AddCors(o => 
            {
                o.AddPolicy("AddPolicy", builder =>
                 {
                     builder.AllowAnyOrigin().
                     AllowAnyMethod()
                     .AllowAnyHeader();
 
                 });
            });

            services.ConfiugureApiVersioning();

            services.CounfigureHttpCacheHeaders();

            services.AddMemoryCache();


            services.CounfigureRateLimiting();
            services.AddHttpContextAccessor();


            services.AddAuthentication();
            services.ConfigureIdentity();

            services.ConfigureJWT(Configuration);
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddAutoMapper(typeof(MapperInitilizer));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelListing", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
            services.AddControllers(conf=> 
            {
                conf.CacheProfiles.Add("120SecondsDuration", new CacheProfile()
                {
                    Duration = 120,
                    
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
          
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelListing v1");


                string swaggerjsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerjsonBasePath}/swagger/v1/swagger.json", "hotel Listing Api");
                ////yek url jadid dorost kardim ke betonim document ro bebinim 

            });
            app.UseHttpsRedirection();

            app.ConfigureExeptionHandler();

            app.UseCors("AddPolicy");

            app.UseResponseCaching();

            app.UseHttpCacheHeaders();

            app.UseIpRateLimiting();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
