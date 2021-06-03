using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Models.Posts
{
    public class PostImageModel
    {
        public int Id { get; set; }

        public int PostId { get; set; }
        
        public string Images { get; set; }
    }
}
