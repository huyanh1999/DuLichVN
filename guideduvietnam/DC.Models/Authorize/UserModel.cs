using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Models.Authorize
{
    public class UserModel
    {
        public int Id { get; set; }

        public string Guid { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string RoleName { get; set; }

        public string Phone { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public string Picture { get; set; }

        public bool? ReceiveEmailNotification { get; set; }

        public bool IsLockedOut { get; set; }

        public string CreateDate { get; set; }

        public int CreateByUserId { get; set; }

    }
}
