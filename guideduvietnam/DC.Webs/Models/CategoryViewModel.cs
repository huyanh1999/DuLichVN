using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DC.Models.Cms;

namespace DC.Webs.Models
{
    public class CategoryViewModel : BaseViewModel
    {
        public CategoryModel CategoryInfo { get; set; }
        public List<CategoryModel> CategoryItems { get; set; }
        public List<SelectItemModel> ParentItems { get; set; }
        public List<SelectItemModel> TypeOptions { get; set; }

    }
}