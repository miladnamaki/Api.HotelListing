﻿using HotelListing.Data;
using HotelListing.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataBaseContext _context;
        private  IGenericRepository<Country> _countries;
        private IGenericRepository<Hotel> _hotels;
        public UnitOfWork(DataBaseContext context)
        {
            _context = context;
          
        }

        public IGenericRepository<Country> Countries =>
            _countries ??= new GenericRepository<Country>(_context);
        //agar  country khali nabud ye done genreic misaze

        public IGenericRepository<Hotel> Hotels => _hotels ??= new GenericRepository<Hotel>(_context);
                                                  //agar hotels khali nabud ye done genreic misaze
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
            //gc clas marbot be dispos va garbage collection mibashad
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
