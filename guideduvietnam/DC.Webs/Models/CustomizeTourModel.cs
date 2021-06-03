using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DC.Webs.Models
{
    public class CustomizeTourModel
    {
        public string TourName { get; set; }
        public string Name { get; set; }
        public string YourName { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
        public string Telephone { get; set; }
        public string ArrivalDate { get; set; }
        public string DepatureDate { get; set; }
        public string PersonNumber { get; set; }
        public string HotelId { get; set; }
        public bool Lunch { get; set; }
        public bool Dinner { get; set; }
        public string Flight { get; set; }
        public string Message { get; set; }
    }
}