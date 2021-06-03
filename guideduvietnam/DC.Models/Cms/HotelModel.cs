using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Models.Cms
{
    public class HotelModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public int? OrderBy { get; set; }
    }
}
