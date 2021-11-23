using HotelListing.Data;
using HotelListing.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelListing.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataBaseContext _context;
        private readonly DbSet<T> _db;
        public GenericRepository(DataBaseContext context)
        {
            _context = context;
            _db = context.Set<T>();
        }

        public async Task Delete(int Id)
        {
            var enitity = await _db.FindAsync(Id);
            _db.Remove(enitity);

        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _db.RemoveRange(entities);
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, List<string> inculdees = null)
        {
            IQueryable<T> qoury = _db;//Dbset<T> yani tamame qouty yek jadvalio miarye to qoury
            if (inculdees is not null)
            {
                foreach (var IncludeProperty in inculdees)
                {
                    qoury = qoury.Include(IncludeProperty);
                }
            }
            return await qoury.AsNoTracking().FirstOrDefaultAsync(expression);  //toye update db.attach zadim pas mitonim asnotrak konim
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null, List<string> includes = null)
        {
            IQueryable<T> qoury = _db;
            if (expression!=null)
            {
                qoury = qoury.Where(expression);
            }
            if (includes is not null)
            {
                foreach (var IncludeProperty in includes)
                {
                    qoury = qoury.Include(IncludeProperty);
                }
            }
            if (OrderBy!=null)
            {
                qoury = OrderBy(qoury);

            }
            return await qoury.AsNoTracking().ToListAsync();
        }

        public async Task Insert(T entity)

        {
            await _db.AddAsync(entity);
        }

        public async Task InsertRange(IEnumerable<T> entites)
        {
            await _db.AddRangeAsync(entites);
        }

        public void Update(T entities)
        {
            _db.Attach(entities);
            _context.Entry(entities).State = EntityState.Modified;

        }
    }
}
