using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Models.Cms
{
    public class MenuModel
    {
        public int Id { get; set; }

        public int? ItemId { get; set; }

        public int ParentId { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public string Url { get; set; }

        public string Target { get; set; }

        public string Type { get; set; }

        public string MenuType { get; set; }

        public string MenuIcon { get; set; }

        public int OrderBy { get; set; }

        public List<ItemModel> ChildItems { get; set; }
    }

    public class ItemModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int OrderBy { get; set; }
        public List<SubItemModel> ChildItems { get; set; }
    }

    public class SubItemModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int OrderBy { get; set; }
    }
}
