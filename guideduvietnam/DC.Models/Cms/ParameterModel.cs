using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Models.Cms
{
    public class ParametersModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public string Value { get; set; }

        //Extend
        public string MetaTitle { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string GooglePlus { get; set; }
        public string Youtube { get; set; }
        public string BusinessLicense { get; set; }
        public string Gmap { get; set; }
        public string About { get; set; }
        public string Instagram { get; set; }
        public string Footer { get; set; }
        public string CustomizeTour { get; set; }
        public string FrmContact { get; set; }
        public string AboutHome { get; set; }
        public string ImgMap { get; set; }
    }
}
