using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC.Common;
using DC.Common.Utility;
using DC.Entities.Base;
using DC.Entities.Domain;
using DC.Services.Cms;

namespace DC.Services.Cms
{
    public partial class CategoryService : GenericRepository<Category>, ICategoryService
    {                
        

        public void Update(Category item)
        {
            if (item == null)
                throw new ArgumentNullException("User Table");
            //update lại tên trong bang post, product
            var post = context.Posts.Where(m => m.CateId == item.Id).ToList();
            foreach (var postitem in post)
            {
                postitem.CateName = item.Name;
            }
           
            context.SaveChanges();

        }

        public void Delete(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("User Table");
            var cate = context.Categories.FirstOrDefault(m => m.Id == id);
            if (cate != null)
            {
                var parent = from a in context.Categories.Where(m => m.ParentId == id) select a;
                foreach (var item in parent)
                {
                    item.ParentId = 0;
                }
                //update lại bảng post có CateId= id
                var postItems = from a in context.Posts.Where(m => m.CateId == id) select a;
                foreach (var item in postItems)
                {
                    item.CateId = 0;
                    item.CateName = string.Empty;
                }
                
                context.Categories.Remove(cate);
                context.SaveChanges();
            }                
        }
                
        public Category GetByKeySlug(string slugurl)
        {
            return context.Categories.FirstOrDefault(m=>m.KeySlug==slugurl);
        }
      
        public IList<Category> GetAll(string cateType)
        {
            var query = context.Categories.OrderBy(m => m.OrderBy).ToList();
            if (!String.IsNullOrEmpty(cateType))
                query = query.Where(m => m.CateType == cateType).OrderBy(m => m.OrderBy).ToList();
            return query.ToList();
        }

        public IList<Category> GetParent(int cateId, string cateType)
        {
            var query = context.Categories.Where(m=>m.Id==cateId || m.ParentId==cateId).OrderBy(m => m.OrderBy).ToList();
            if (!String.IsNullOrEmpty(cateType))
                query = query.Where(m => m.CateType == cateType).OrderBy(m => m.OrderBy).ToList();
            return query.ToList();
        }

        public List<int> ListCateIds(int parentId)
        {
            var query = context.Categories.Where(m => m.Id == parentId || m.ParentId == parentId).Select(m => m.Id).ToList();
            return query;
        }

        public IPagedList<Category> GetAll(string name,string type, string sortBy,
            string orderBy, int pageIndex, int pageSize)
        {
            var query = context.Categories.AsQueryable();
            if (!string.IsNullOrWhiteSpace(type))
                query = query.Where(c => c.CateType == type);
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.ToLower().Contains(name.ToLower()) || c.Title.ToLower().Contains(name.ToLower()));
            
            // Sorting
            switch (sortBy.ToUpper())
            {
                case "NAME":
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.Name) : query = query.OrderByDescending(c => c.Name);
                        break;
                    }
                case "ORDERBY":
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.OrderBy) : query = query.OrderByDescending(c => c.OrderBy);
                        break;
                    }
                default:
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.Id) : query = query.OrderByDescending(c => c.Id);
                        break;
                    }
            }

            return new PagedList<Category>(query, pageIndex, pageSize);
        }
    }
}
