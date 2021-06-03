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
    public partial interface IPostService : IGenericRepository<Post>
    {
      
        void Insert(Post item, ref int postId);

        void Update(Post item);

        void Delete(int id);

        void Refresh(int id);        

        Post GetByKeySlug(string slugurl);        

        IList<Post> GetAll(string postType);

        IList<Post> GetAll(string postType, string status);

        IList<Post> GetAll(string postType, string status, int take);

        IPagedList<Post> GetFilterAdmin(string name, int cateId, string status, DateTime? fromDate, DateTime? toDate, string postType, string sortBy,
            string orderBy, int pageIndex, int pageSize);

        IPagedList<Post> GetFilterAdmin(string name, int cateId, string status, DateTime? fromDate, DateTime? toDate, List<string> postType, string sortBy,
            string orderBy, int pageIndex, int pageSize);


        IPagedList<Post> GetFilterAdmin(string username,string name, int cateId, string status, DateTime? fromDate, DateTime? toDate, string postType, string sortBy,
            string orderBy, int pageIndex, int pageSize);

        IPagedList<Post> GetAll(string name, List<int> cateId, string status, DateTime? fromDate, DateTime? toDate, string postType, string sortBy,
            string orderBy, int pageIndex, int pageSize);

        IPagedList<Post> GetAll(string name, List<int> cateId, string status, DateTime? fromDate, DateTime? toDate, List<string> postType, string sortBy,
            string orderBy, int pageIndex, int pageSize);

        IPagedList<Post> GetAllByTag(string name, string tagName, string status, DateTime? fromDate, DateTime? toDate, List<string> postType, string sortBy,
           string orderBy, int pageIndex, int pageSize);

        List<Post> GetPostRelated(int postId, string status, string type, int take);

        List<Post> GetPostRelated(int postId, int cateId, string status, List<string> type, int take);
    }
}
