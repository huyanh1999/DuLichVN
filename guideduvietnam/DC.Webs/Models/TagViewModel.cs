using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DC.Models.Posts;

namespace DC.Webs.Models
{
    public class TagViewModel: BaseViewModel
    {
        public TagModel TagInfo { get; set; }
        public List<TagModel> TagItems { get; set; }
        public string TagType { get; set; }
        public List<SelectItemModel> TagOptions { get; set; } 
    }
}