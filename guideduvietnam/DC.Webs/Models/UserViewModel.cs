using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DC.Models.Authorize;

namespace DC.Webs.Models
{
    public class UserViewModel : BaseViewModel
    {
        public List<SelectItemModel> RoleOptions { get; set; }
        public List<SelectItemModel> UserOptions { get; set; }
        public List<SelectItemModel> StatusOptions { get; set; }
        public UserModel UserInfo { get; set; }
        public List<UserModel> UserItems { get; set; }
    }
}