using DC.Common;
using DC.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC.Entities.Base;

namespace DC.Services.Logging
{
    public partial interface IActivityLogService : IGenericRepository<ActivityLog>
    {       

        IPagedList<ActivityLog> GetAll(string name,
            int activityLogTypeId,
            DateTime? fromDate,
            DateTime? toDate,
            string sortBy,
            string orderBy,
            int pageIndex,
            int pageSize);

        void Insert(ActivityLog item);
        

        void Delete(int id);

        void Delete(IList<ActivityLog> items);
    }
}
