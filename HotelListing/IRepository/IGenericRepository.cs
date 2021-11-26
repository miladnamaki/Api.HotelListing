using HotelListing.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace HotelListing.IRepository
{
    public interface IGenericRepository<T> where T :class
    {
        Task<IList<T>> GetAll(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null,
            List<string> includes = null);

        Task<IPagedList<T>> GetPageList(List<string> Includes = null,
            RequestParams requestParams = null);

        Task<T> Get(Expression<Func<T, bool>> expression,
            List<string> inculdees = null);
        Task Insert(T entity);
        Task InsertRange(IEnumerable<T> entites);
        Task Delete(int Id);
        void DeleteRange(IEnumerable<T> entities);

        void Update(T entities);


    }
}
