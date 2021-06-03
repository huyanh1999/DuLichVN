using DC.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC.Entities.Base;

namespace DC.Services.Authorize
{
    public partial interface IUserService : IGenericRepository<User>
    {        
        User GetByUserName(string userName);

        bool GetUserLogin(string userName, string password);
        
    }
}
