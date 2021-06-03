using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC.Common;
using DC.Entities.Base;
using DC.Entities.Domain;
using DC.Common.Utility;

namespace DC.Services.Posts
{
    public class PostService : GenericRepository<Post>, IPostService
    {       
        
        public void Insert(Post item,ref int postId)
        {
            if (item == null)
                throw new ArgumentNullException("Post Table");
            item.CreateDate = DateTime.Now;
            context.Posts.Add(item);
            context.SaveChanges();
            postId = item.Id;
        }
        public void Update(Post item)
        {
            if (item == null)
                throw new ArgumentNullException("Post Table");
            item.ModifiedDate = DateTime.Now;
            context.SaveChanges();

        }

        public void Delete(int id)
        {
            var post = context.Posts.Find(id);
            var postImgs = context.PostImages.Where(m => m.PostId == id);
            if(postImgs.Any())
                context.PostImages.RemoveRange(postImgs);
            context.Posts.Remove(post);
            context.SaveChanges();
        }
        public void Refresh(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("Post Table");
            var post = context.Posts.FirstOrDefault(m => m.Id == id);
            if (post != null)
            {
                post.CreateDate = DateTime.Now;
                context.SaveChanges();
            }
        }
       
        public Post GetByKeySlug(string slugurl)
        {
            return context.Posts.FirstOrDefault(m => m.KeySlug == slugurl);
        }
       
        public IList<Post> GetAll(string postType)
        {
            var query = context.Posts.OrderBy(m => m.Name).ToList();
            if (!String.IsNullOrEmpty(postType))
                query = query.Where(m => m.PostType == postType).OrderByDescending(m => m.CreateDate).ToList();
            return query.ToList();
        }


        public IList<Post> GetAll(string postType, string status)
        {
            var query = context.Posts.AsQueryable();
            if (!string.IsNullOrEmpty(status))
                query = query.Where(m => m.Status == status);
            if (!string.IsNullOrEmpty(postType))
                query = query.Where(m => m.PostType == postType).OrderByDescending(m => m.CreateDate);
            return query.ToList();
        }

        public IList<Post> GetAll(string postType, string status, int take)
        {
            var query = context.Posts.AsQueryable();
            if (!string.IsNullOrEmpty(status))
                query = query.Where(m => m.Status == status);
            if (!string.IsNullOrEmpty(postType))
                query = query.Where(m => m.PostType == postType).OrderByDescending(m => m.CreateDate);
            return query.Take(take).ToList();
        }

        /// <summary>
        /// Danh sách bài viết - admin quan trị
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cateId"></param>
        /// <param name="status"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="postType"></param>
        /// <param name="sortBy"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<Post> GetFilterAdmin(string name, int cateId, string status, DateTime? fromDate, DateTime? toDate, string postType, string sortBy,
            string orderBy, int pageIndex, int pageSize)
        {            
            var query = context.Posts.AsQueryable();

            if (fromDate.HasValue)
            {
                var fDate = DateTime.Parse(fromDate.ToString()).StartOfDay();
                query = query.Where(c => c.CreateDate >= fDate);
            }
            if (toDate.HasValue)
            {
                var tDate = DateTime.Parse(toDate.ToString()).EndOfDay();
                query = query.Where(c => c.CreateDate <= tDate);
            }
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.ToLower().Contains(name.ToLower()) ||c.Title.ToLower().Contains(name.ToLower()));
            if (cateId>0)
                query = query.Where(m => m.CateId == cateId);
            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(c => c.Status== status);
            if (!string.IsNullOrEmpty(postType))
                query = query.Where(m => m.PostType == postType);
                       
            // Sorting
            switch (sortBy.ToUpper())
            {
                case "NAME":
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.Name) : query = query.OrderByDescending(c => c.Name);
                        break;
                    }
                case "CREATEDATE":
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.CreateDate) : query = query.OrderByDescending(c => c.CreateDate);
                        break;
                    }
                default:
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.Id) : query = query.OrderByDescending(c => c.Id);
                        break;
                    }
            }

            return new PagedList<Post>(query, pageIndex, pageSize);
        }

        public IPagedList<Post> GetFilterAdmin(string name, int cateId, string status, DateTime? fromDate,
            DateTime? toDate, List<string> postType, string sortBy,
            string orderBy, int pageIndex, int pageSize)
        {
            var query = context.Posts.AsQueryable();

            if (fromDate.HasValue)
            {
                var fDate = DateTime.Parse(fromDate.ToString()).StartOfDay();
                query = query.Where(c => c.CreateDate >= fDate);
            }
            if (toDate.HasValue)
            {
                var tDate = DateTime.Parse(toDate.ToString()).EndOfDay();
                query = query.Where(c => c.CreateDate <= tDate);
            }
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.ToLower().Contains(name.ToLower()) || c.Title.ToLower().Contains(name.ToLower()));
            if (cateId > 0)
                query = query.Where(m => m.CateId == cateId);
            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(c => c.Status == status);
            if (postType.Count > 0)
                query = query.Where(m => postType.Contains(m.PostType));

            // Sorting
            switch (sortBy.ToUpper())
            {
                case "NAME":
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.Name) : query = query.OrderByDescending(c => c.Name);
                        break;
                    }
                case "CREATEDATE":
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.CreateDate) : query = query.OrderByDescending(c => c.CreateDate);
                        break;
                    }
                default:
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.Id) : query = query.OrderByDescending(c => c.Id);
                        break;
                    }
            }

            return new PagedList<Post>(query, pageIndex, pageSize);
        }

        public IPagedList<Post> GetFilterAdmin(string username, string name, int cateId, string status,
            DateTime? fromDate, DateTime? toDate, string postType, string sortBy,
            string orderBy, int pageIndex, int pageSize)
        {
            var query = context.Posts.AsQueryable();

            if (fromDate.HasValue)
            {
                var fDate = DateTime.Parse(fromDate.ToString()).StartOfDay();
                query = query.Where(c => c.CreateDate >= fDate);
            }
            if (toDate.HasValue)
            {
                var tDate = DateTime.Parse(toDate.ToString()).EndOfDay();
                query = query.Where(c => c.CreateDate <= tDate);
            }
            if(!string.IsNullOrEmpty(username))
                query = query.Where(m => m.CreateByUser == username);
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.ToLower().Contains(name.ToLower()) || c.Title.ToLower().Contains(name.ToLower()));
            if (cateId > 0)
                query = query.Where(m => m.CateId == cateId);
            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(c => c.Status == status);
            if (!string.IsNullOrEmpty(postType))
                query = query.Where(m => m.PostType == postType);

            // Sorting
            switch (sortBy.ToUpper())
            {
                case "NAME":
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.Name) : query = query.OrderByDescending(c => c.Name);
                        break;
                    }
                case "CREATEDATE":
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.CreateDate) : query = query.OrderByDescending(c => c.CreateDate);
                        break;
                    }
                default:
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.Id) : query = query.OrderByDescending(c => c.Id);
                        break;
                    }
            }

            return new PagedList<Post>(query, pageIndex, pageSize);
        }


        public IPagedList<Post> GetAll(string name, List<int> cateIds, string status, DateTime? fromDate,
            DateTime? toDate, string postType, string sortBy,
            string orderBy, int pageIndex, int pageSize)
        {
            var query = context.Posts.AsQueryable();

            if (fromDate.HasValue)
            {
                var fDate = DateTime.Parse(fromDate.ToString()).StartOfDay();
                query = query.Where(c => c.CreateDate >= fDate);
            }
            if (toDate.HasValue)
            {
                var tDate = DateTime.Parse(toDate.ToString()).EndOfDay();
                query = query.Where(c => c.CreateDate <= tDate);
            }
            
            if (cateIds.Count > 0)
                query = query.Where(m => cateIds.Contains(m.CateId));

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.ToLower().Contains(name.ToLower()) || c.Title.ToLower().Contains(name.ToLower()));

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(c => c.Status == status);

            if (!string.IsNullOrEmpty(postType))
                query = query.Where(m => m.PostType == postType);

            // Sorting
            switch (sortBy.ToUpper())
            {
                case "NAME":
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.Name) : query = query.OrderByDescending(c => c.Name);
                        break;
                    }
                case "CREATEDATE":
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.CreateDate) : query = query.OrderByDescending(c => c.CreateDate);
                        break;
                    }
                default:
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.Id) : query = query.OrderByDescending(c => c.Id);
                        break;
                    }
            }

            return new PagedList<Post>(query, pageIndex, pageSize);
        }

        public IPagedList<Post> GetAll(string name, List<int> cateIds, string status, DateTime? fromDate,
            DateTime? toDate, List<string> postTypes, string sortBy,
            string orderBy, int pageIndex, int pageSize)
        {
            var query = context.Posts.AsQueryable();

            if (fromDate.HasValue)
            {
                var fDate = DateTime.Parse(fromDate.ToString()).StartOfDay();
                query = query.Where(c => c.CreateDate >= fDate);
            }
            if (toDate.HasValue)
            {
                var tDate = DateTime.Parse(toDate.ToString()).EndOfDay();
                query = query.Where(c => c.CreateDate <= tDate);
            }

            if (cateIds.Count > 0)
                query = query.Where(m => cateIds.Contains(m.CateId));

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.ToLower().Contains(name.ToLower()) || c.Title.ToLower().Contains(name.ToLower()));

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(c => c.Status == status);

            if (postTypes.Count>0)
                query = query.Where(m => postTypes.Contains(m.PostType));

            // Sorting
            switch (sortBy.ToUpper())
            {
                case "NAME":
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.Name) : query = query.OrderByDescending(c => c.Name);
                        break;
                    }
                case "CREATEDATE":
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.CreateDate) : query = query.OrderByDescending(c => c.CreateDate);
                        break;
                    }
                default:
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.Id) : query = query.OrderByDescending(c => c.Id);
                        break;
                    }
            }

            return new PagedList<Post>(query, pageIndex, pageSize);
        }

        public IPagedList<Post> GetAllByTag(string name, string tagName, string status, DateTime? fromDate,
            DateTime? toDate, List<string> postType, string sortBy,
            string orderBy, int pageIndex, int pageSize)
        {
            var query = context.Posts.AsQueryable();

            if (fromDate.HasValue)
            {
                var fDate = DateTime.Parse(fromDate.ToString()).StartOfDay();
                query = query.Where(c => c.CreateDate >= fDate);
            }
            if (toDate.HasValue)
            {
                var tDate = DateTime.Parse(toDate.ToString()).EndOfDay();
                query = query.Where(c => c.CreateDate <= tDate);
            }

            if (!string.IsNullOrEmpty(tagName))
                query = query.Where(m => m.TagsId.Contains(tagName));

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.ToLower().Contains(name.ToLower()) || c.Title.ToLower().Contains(name.ToLower()));

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(c => c.Status == status);

            if (postType.Count > 0)
                query = query.Where(m => postType.Contains(m.PostType));

            // Sorting
            switch (sortBy.ToUpper())
            {
                case "NAME":
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.Name) : query = query.OrderByDescending(c => c.Name);
                        break;
                    }
                case "CREATEDATE":
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.CreateDate) : query = query.OrderByDescending(c => c.CreateDate);
                        break;
                    }
                default:
                    {
                        query = orderBy.Equals("asc", StringComparison.OrdinalIgnoreCase) ? query = query.OrderBy(c => c.Id) : query = query.OrderByDescending(c => c.Id);
                        break;
                    }
            }

            return new PagedList<Post>(query, pageIndex, pageSize);
        }
        public List<Post> GetPostRelated(int postId, string status,string type, int take)
        {
            return context.Posts.Where(m => m.Status == status && m.Id != postId && (m.PostType == type|| type=="")).OrderBy(r => Guid.NewGuid()).Take(take).ToList();
        }

        public List<Post> GetPostRelated(int postId,int cateId, string status, List<string> type, int take)
        {
            var query = context.Posts.AsQueryable();
            if (postId > 0)
                query = query.Where(m => m.Id!=postId);
            if (cateId > 0)
                query = query.Where(m => m.CateId != cateId);
            if (!string.IsNullOrEmpty(status))
                query = query.Where(m => m.Status == status);
            if (type.Count > 0)
                query = query.Where(m => type.Contains(m.PostType));
            return query.OrderBy(r => Guid.NewGuid()).Take(take).ToList();
        }
    }
}
