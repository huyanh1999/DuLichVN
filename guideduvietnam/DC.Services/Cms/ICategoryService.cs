using DC.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC.Common;
using DC.Entities.Base;

namespace DC.Services.Cms
{
    public partial interface ICategoryService : IGenericRepository<Category>
    {
        

        void Update(Category item);

        void Delete(int id);
        

        Category GetByKeySlug(string slugurl);

        

        IList<Category> GetAll(string cateType);

        /// <summary>
        /// Danh sách danh mục parent
        /// </summary>
        /// <param name="cateId"></param>
        /// <param name="cateType"></param>
        /// <returns></returns>
        IList<Category> GetParent(int cateId, string cateType);


        List<int> ListCateIds(int parentId);

        IPagedList<Category> GetAll(string name, string type, string sortBy,
            string orderBy, int pageIndex, int pageSize);

    }
}
