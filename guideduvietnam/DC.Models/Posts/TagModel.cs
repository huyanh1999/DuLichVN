using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Models.Posts
{
    public class TagModel
    {
        public int Id { get; set; }
        
        public string KeySlug { get; set; }
        
        public string Name { get; set; }
        
        public string Title { get; set; }
        
        public string Url { get; set; }
        
        public string MetaKeyword { get; set; }
        
        public string MetaDescription { get; set; }
        
        public string TagType { get; set; }
    }
}
