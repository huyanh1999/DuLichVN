using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DC.Models.Logging;

namespace DC.Webs.Models
{
    public class ActivityLogViewModel : BaseViewModel
    {
        public List<SelectItemModel> ActivityLogTypeOptions { get; set; }
        public ActivityLogModel ActivityLogInfo { get; set; }
        public List<ActivityLogModel> ActivityLogItems { get; set; }
    }
}