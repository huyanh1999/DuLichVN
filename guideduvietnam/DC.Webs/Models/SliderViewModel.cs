using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DC.Models.Media;

namespace DC.Webs.Models
{
    public class SliderViewModel : BaseViewModel
    {
        public SlideModel SliderInfo { get; set; }
        public List<SlideModel> SliderItems { get; set; }
    }
}