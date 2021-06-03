using DC.Models.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DC.Webs.Models
{
    public class BaseViewModel
    {
        // Authentication
        public UserLoggedIn CurrentUser { get; set; }
        
        public int TotalNotify { get; set; }
        public string TabName { get; set; }
        public string ActionName { get; set; }

        public int TotalCart { get; set; }

        public decimal TotalPrice { get; set; }

        public ParametersModel ParameterInfo { get; set; }

        public string Language { get; set; }

        public string Type { get; set; }

        public LableModel LableInfo { get; set; }

        // Paging | Sorting
        public string QueryStrings { get; set; }
        public string SortBy { get; set; }
        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}