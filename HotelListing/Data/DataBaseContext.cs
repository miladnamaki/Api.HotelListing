using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Data
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions options) : base(options)
        {}
        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            modelBuilder.Entity<Country>(option =>
            {
                option.HasData(
                    new Country {Id=1, Name="Jamaica" ,ShortName="JM"},
                    new Country { Id = 2,Name ="Iran" ,ShortName="IR"},
                    new Country { Id = 3,Name ="Cayman Island" ,ShortName="CI"});

            });
            modelBuilder.Entity<Hotel>(option =>
            {
                option.HasData(

                    new Hotel { Id=1, Name = "Sanals REsort and spa", Address = "Negril", CountryId = 1, Rating = 4.5 },
                    new Hotel { Id = 2, Name = "Pars", Address = "Kianpars", CountryId = 3, Rating = 4.5 },
                    new Hotel { Id = 3,Name = "Grand Palladium", Address = "Nassua", CountryId = 2, Rating = 4.5 });

                
            });

        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set;}

    }
}
