using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DC.Models.Cms;

namespace DC.Webs.Models
{
    public class MenuViewModel : BaseViewModel
    {
        public MenuModel MenuInfo { get; set; }
        public List<MenuModel> MenuItems { get; set; }

        public List<SelectItemModel> MenuTypeOptions { get; set; }
        public List<SelectItemModel> MenuSourceOptions { get; set; }

        public List<SelectItemModel> ParentOptions { get; set; }

        public List<SelectItemModel> MenuContentOptions { get; set; }
    }
}