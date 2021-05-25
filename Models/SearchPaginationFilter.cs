using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetAuthentication.Models
{
    public class SearchPaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string searchstring { get; set; }
        public string SortString { get; set; }
        public string SortOrder { get; set; }

        public SearchPaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
            this.searchstring = "";
            this.SortString = "FIRSTNAME";
            this.SortOrder = "ASC";
        }

        public SearchPaginationFilter(string searchstring, int pageNumber, int pageSize, string sortString, string sortOrder)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 30 ? 30 : pageSize;
            this.searchstring = searchstring;
            SortString = sortString;
            SortOrder = sortOrder;
        }
    }
}
