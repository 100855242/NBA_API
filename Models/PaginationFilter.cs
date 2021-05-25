using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetAuthentication.Models
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortString { get; set; }
        public string SortOrder { get; set; }

        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
            this.SortString = "FIRSTNAME";
            this.SortOrder = "ASC";
        }

        public PaginationFilter(int pageNumber, int pageSize, string sortString, string sortOrder)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber; ;
            PageSize = pageSize > 30 ? 30 : pageSize; ;
            SortString = sortString;
            SortOrder = sortOrder;
        }


    }
}
