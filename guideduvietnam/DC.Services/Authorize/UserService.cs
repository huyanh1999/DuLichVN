using DC.Entities.Base;
using DC.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Services.Authorize
{
    public partial class UserService : GenericRepository<User>, IUserService
    {
        public UserService() : base() { }
        
        public User GetByUserName(string userName)
        {
            return context.Users.FirstOrDefault(m => m.UserName == userName);
        }
        public bool GetUserLogin(string userName, string password)
        {
            var user =
                context.Users.FirstOrDefault(
                    m => m.UserName == userName && m.Password == password && m.IsLockedOut == false);
            if (user != null)
                return true;
            else return false;
        }
    }
}
