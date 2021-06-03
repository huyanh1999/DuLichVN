namespace DC.Entities.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Hotel")]
    public partial class Hotel
    {
        public int Id { get; set; }

        [StringLength(512)]
        public string Name { get; set; }

        public int? OrderBy { get; set; }
    }
}
