using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DC.Models.Posts;

namespace DC.Webs.Models
{
    public class PostViewModel: BaseViewModel
    {
        public PostModel PostInfo { get; set; }
        public List<PostModel> PostItems { get; set; }
        public List<SelectItemModel> CategoryOptions { get; set; }
        public List<SelectItemModel> TagsOptions { get; set; }
        public List<SelectItemModel> StatusOptions { get; set; }
        public List<SelectItemModel> PostTypeOptions { get; set; }
        public List<JsonItem> PostImageItems { get; set; }

        public List<TagModel> TagItems { get; set; } 
    }
}