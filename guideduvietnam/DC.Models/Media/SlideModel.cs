using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Models.Media
{
    public class SlideModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Images { get; set; }

        public string Url { get; set; }

        public int OrderBy { get; set; }

        public string Description { get; set; }
    }
}
