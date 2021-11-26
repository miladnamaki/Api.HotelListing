using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Model
{
    public class RequestParams
    {
        const int MaxPaginSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pagesize = 10;
        public int Pageisze
        {
            get
            {
                return _pagesize;
            }
            set
            {
                _pagesize = (value > MaxPaginSize) ? MaxPaginSize : value;
            }
        }
            
    }
}
