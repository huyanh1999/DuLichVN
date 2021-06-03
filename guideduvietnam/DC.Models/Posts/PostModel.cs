using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Models.Posts
{
    public class PostModel
    {
        public int Id { get; set; }
        
        public string KeySlug { get; set; }
        
        public string Name { get; set; }

        public int CateId { get; set; }
        
        public string CateName { get; set; }
        
        public string Title { get; set; }
        
        public string Url { get; set; }
        
        public string Picture { get; set; }

        public string Description { get; set; }
        
        public string MetaKeyword { get; set; }
        
        public string MetaDescription { get; set; }

        public string OverView { get; set; }

        public string Content { get; set; }
        
        public string Status { get; set; }

        public int ViewCount { get; set; }

        public string TagsId { get; set; }
        
        public string PostType { get; set; }
        
        public string CreateByUser { get; set; }

        public string CreateDate { get; set; }

        public string ModifiedDate { get; set; }
        public bool IsComment { get; set; }
        public string Target { get; set; }
    }
}
