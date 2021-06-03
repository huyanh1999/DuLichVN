using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Models.Cms
{
    public class CountryModel
    {
        public int Id { get; set; }

        public string ShortCode { get; set; }

        public string LongCode { get; set; }

        public string Name { get; set; }

        public string EnglishName { get; set; }

        public string Description { get; set; }
    }
}
