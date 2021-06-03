using DC.Models.Cms;
using DC.Models.Media;
using DC.Models.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DC.Webs.Models
{
    public class HomeViewModel : BaseViewModel
    {
        public List<SlideModel> SlideItems { get; set; }        

        public List<PostModel> PostItems { get; set; }

        public List<PostImageModel> PostImageItems { get; set; }

        public CategoryModel CategoryInfo { get; set; }        

        public List<SelectItemModel> OrderByOptions { get; set; }        

        public PostModel PostInfo { get; set; }

        public List<CartModel> CartItems { get; set; }        

        public List<TagModel> TagItems { get; set; }

        public TagModel TagInfo { get; set; }

        public List<HotelModel> HotelItems { get; set; }

        public List<SelectItemModel> PaymentOptions { get; set; }

        public List<SelectItemModel> DeliveryOptions { get; set; }

        public List<SelectItemModel> FunctionByOptions { get; set; }

        public CustomizeTourModel CustomizeTourModel { get; set; }

        public List<MenuModel> MenuItems { get; set; }
    }
}