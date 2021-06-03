using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC.Common;
using DC.Common.Utility;
using DC.Entities.Base;
using DC.Entities.Domain;

namespace DC.Services.Logging
{
    
    public partial class ActivityLogService : GenericRepository<ActivityLog>, IActivityLogService
    {        
        #region #Methods
        
        public IPagedList<ActivityLog> GetAll(string name,
            int activityLogTypeId,
            DateTime? fromDate,
            DateTime? toDate,
            string sortBy, 
            string orderBy, 
            int pageIndex, 
            int pageSize)
        {
            var query = context.ActivityLogs.AsQueryable();
            if (activityLogTypeId > 0)
                query = query.Where(c => c.ActivityLogTypeId == activityLogTypeId);

            
            if (fromDate.HasValue)
            {
                DateTime startDate = DateTimeTools.StartOfDay(fromDate.Value);
                query = query.Where(c => c.CreateDate >= startDate);
            }
            if (toDate.HasValue)
            {
                DateTime endDate = DateTimeTools.EndOfDay(toDate.Value);
                query = query.Where(c => c.CreateDate <= endDate);
            }

            // Filter here
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Comment.ToLower().Contains(name.ToLower()));

            // Sort
            query = query.OrderByDescending(c => c.CreateDate);

            // Return to list with paging
            return new PagedList<ActivityLog>(query, pageIndex, pageSize);
        }


        

        public void Insert(ActivityLog item)
        {
            if (item == null)
                throw new ArgumentNullException("ActivityLog Table");
            item.CreateDate = DateTime.Now;
            context.ActivityLogs.Add(item);
            context.SaveChanges();            
        }


        
        public void Delete(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("ActivityLog Table");
            var activityLog = context.ActivityLogs.FirstOrDefault(m => m.Id == id);
            if (activityLog != null)
            {

                context.ActivityLogs.Remove(activityLog);
                context.SaveChanges();
            }
        }

        public void Delete(IList<ActivityLog> items)
        {
            if (items == null)
                throw new ArgumentNullException("ActivityLog Table");

            context.ActivityLogs.RemoveRange(items);
        }
        #endregion
    }
}
