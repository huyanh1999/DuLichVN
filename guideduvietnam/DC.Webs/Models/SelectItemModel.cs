using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DC.Webs.Models
{
    public class SelectItemModel
    {
        public SelectItemModel(string text, string value, bool selected)
        {
            this.Text = text;
            this.Value = value;
            this.Selected = selected;
        }

        public SelectItemModel(string text, string value)
        {
            this.Text = text;
            this.Value = value;
        }

        public SelectItemModel()
        {
        }


        public string Value { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; }
        public bool Blocked { get; set; }
    }
}