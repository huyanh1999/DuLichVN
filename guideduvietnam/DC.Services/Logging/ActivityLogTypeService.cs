using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC.Entities.Base;
using DC.Entities.Domain;

namespace DC.Services.Logging
{
    
    public class ActivityLogTypeService:GenericRepository<ActivityLogType>, IActivityLogTypeService
    {
        
        #region #Methods
        public ActivityLogType GetSingle(int itemId)
        {
            if (itemId == 0)
                return null;

            return context.ActivityLogTypes.FirstOrDefault(m => m.Id == itemId);
        }


        public ActivityLogType GetSingle(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return context.ActivityLogTypes.FirstOrDefault(m => m.Name == name);
        }        
        #endregion
    }
}