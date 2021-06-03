using DC.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC.Entities.Base;

namespace DC.Services.Logging
{
    public partial interface IActivityLogTypeService : IGenericRepository<ActivityLogType>
    {
        ActivityLogType GetSingle(int itemId);

        ActivityLogType GetSingle(string name);
        
    }
}
