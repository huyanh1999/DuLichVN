using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC.Entities.Base;
using DC.Entities.Domain;
using DC.Services.Cms;

namespace DC.Services.Cms
{
    public partial class MenuService : GenericRepository<Menu>, IMenuService
    {
        
        public void Delete(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("Menu Table");
            var menu = context.Menus.FirstOrDefault(m => m.Id == id);
            if (menu != null)
            {
                var parent = from a in context.Menus.Where(m => m.ParentId == id) select a;
                foreach (var item in parent)
                {
                    item.ParentId = 0;
                }
                context.Menus.Remove(menu);
                context.SaveChanges();
            }            
        }
                

        public IList<Menu> GetAll(string menuType)
        {
            return context.Menus.Where(m => m.MenuType.ToUpper() == menuType.ToUpper()).ToList();
        }

        public IList<Menu> GetAll(int itemId, string type)
        {
            return context.Menus.Where(m => m.Type.ToUpper() == type.ToUpper() && m.ItemId==itemId).ToList();
        }
    }
}
