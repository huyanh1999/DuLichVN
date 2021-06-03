using DC.Common;
using DC.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC.Entities.Base;

namespace DC.Services.Posts
{
    public partial interface ITagsService : IGenericRepository<Tag>
    {
        void Insert(Tag item);

        void Insert(Tag item,ref int tagId);

        void Delete(int id);
        
        Tag GetByKeySlug(string slugurl);
        
        IList<Tag> GetAll(string tagType);

        IPagedList<Tag> GetAlls(string name, string tagType, string sortBy, string orderBy, int pageIndex, int pageSize);
        
    }
}
