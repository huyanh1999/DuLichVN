namespace DC.Entities.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Slide")]
    public partial class Slide
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Images { get; set; }

        [StringLength(500)]
        public string Url { get; set; }

        public int OrderBy { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
}
