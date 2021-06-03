using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DC.Webs.Models
{
    public class UserLoggedIn
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string LastName { get; set; }
        public string PackageName { get; set; }
    }
}