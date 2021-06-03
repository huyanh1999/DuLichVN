using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Models.Logging
{
    public class ActivityLogModel
    {
        public int Id { get; set; }

        public int ActivityLogTypeId { get; set; }
        
        public string SessionId { get; set; }
        
        public string Comment { get; set; }
        
        public string ResourceType { get; set; }

        public int? UserId { get; set; }

        public string CreateDate { get; set; }

        //Extend

        //extend
        public string ActivityLogTypeName { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}
