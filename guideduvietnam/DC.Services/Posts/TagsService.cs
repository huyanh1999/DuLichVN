using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC.Common;
using DC.Entities.Base;
using DC.Entities.Domain;

namespace DC.Services.Posts
{
    public partial class TagsService : GenericRepository<Tag> , ITagsService
    {        
        public void Insert(Tag item)
        {
            if (item == null)
                throw new ArgumentNullException("Tag Table");
            context.Tags.Add(item);
            context.SaveChanges();
        }


        public void Insert(Tag item, ref int tagId)
        {
            if (item == null)
                throw new ArgumentNullException("Tag Table");
            context.Tags.Add(item);
            context.SaveChanges();
            tagId = item.Id;
        }
        
        public void Delete(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("Tag Table");
            var tags = context.Tags.FirstOrDefault(m => m.Id == id);
            if (tags != null)
            {                         
                context.Tags.Remove(tags);
                context.SaveChanges();
            }
        }
        
        public Tag GetByKeySlug(string slugurl)
        {
            return context.Tags.FirstOrDefault(m => m.KeySlug == slugurl);
        }
        
        public IList<Tag> GetAll(string tagType)
        {
            var query = context.Tags.OrderBy(m => m.Name).ToList();
            if (!String.IsNullOrEmpty(tagType))
                query = query.Where(m => m.TagType == tagType).OrderBy(m => m.Name).ToList();
            return query.ToList();
        }
        public IPagedList<Tag> GetAlls(string name,string tagType, string sortBy, string orderBy, int pageIndex, int pageSize)
        {
            var query = context.Tags.AsQueryable();
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.ToLower().Contains(name.ToLower()));
            if (!string.IsNullOrEmpty(tagType))
                query = query.Where(m => m.TagType == tagType).OrderBy(m => m.Name);

            // Sorting
            switch (sortBy.ToUpper())
            {
                case "NAME":
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.Name) : query = query.OrderByDescending(c => c.Name);
                        break;
                    }                
                default:
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.Id) : query = query.OrderByDescending(c => c.Id);
                        break;
                    }
            }

            return new PagedList<Tag>(query, pageIndex, pageSize);
        }
        
    }
}
