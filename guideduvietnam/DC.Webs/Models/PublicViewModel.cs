using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;
using DC.Models.Authorize;
using DC.Models.Cms;
using DC.Models.Posts;
using DC.Models.Media;

namespace DC.Webs.Models
{
    public class PublicViewModel:BaseViewModel
    {
        public List<PostModel> PostItems { get; set; } 
        
        public List<ParameterModel> ParameterItems { get; set; }

        //public ParametersModel ParametersInfo { get; set; }
        
        public List<MenuModel> MenuItems { get; set; }

        public List<CategoryModel> CategoryItems { get; set; }

        public List<SlideModel> SlideItems { get; set; }
        public List<TagModel> TagItems { get; set; }      
        
        
    }
}