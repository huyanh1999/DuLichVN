using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DC.Models.Cms;

namespace DC.Webs.Models
{
    public class HotelViewModel:BaseViewModel
    {
        public List<HotelModel> HotelItems { get; set; } 
        public HotelModel HotelInfo { get; set; }
    }
}