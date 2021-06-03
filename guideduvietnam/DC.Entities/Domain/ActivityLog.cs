namespace DC.Entities.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ActivityLog")]
    public partial class ActivityLog
    {
        public int Id { get; set; }

        public int ActivityLogTypeId { get; set; }

        [Required]
        [StringLength(76)]
        public string SessionId { get; set; }

        [StringLength(4000)]
        public string Comment { get; set; }

        [StringLength(50)]
        public string ResourceType { get; set; }

        public int? UserId { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
