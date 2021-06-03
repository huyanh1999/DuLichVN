using DC.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC.Entities.Base;

namespace DC.Services.Cms
{
    public partial interface IMenuService : IGenericRepository<Menu>
    {      
        void Delete(int id);        

        IList<Menu> GetAll(string menuType);

        IList<Menu> GetAll(int itemId, string menuType);
    }
}
