using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Models.Cms
{
    public class CategoryModel
    {
        public int Id { get; set; }

        public string KeySlug { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string CateType { get; set; }

        public string Description { get; set; }

        public string MetaKeyword { get; set; }

        public string MetaDescription { get; set; }

        public int ParentId { get; set; }

        public int OrderBy { get; set; }


        //Extend
        public string ParentName { get; set; }
        public List<CategoryChildModel> ChildItems { get; set; }
        
    }


    public class CategoryChildModel
    {
        public int Id { get; set; }

        public string KeySlug { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string CateType { get; set; }

        public string Description { get; set; }

        public string MetaKeyword { get; set; }

        public string MetaDescription { get; set; }

        public int ParentId { get; set; }

        public int OrderBy { get; set; }
    }
}
