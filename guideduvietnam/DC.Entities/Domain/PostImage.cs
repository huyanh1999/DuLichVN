namespace DC.Entities.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PostImage")]
    public partial class PostImage
    {
        public int Id { get; set; }

        public int? PostId { get; set; }

        [StringLength(512)]
        public string Images { get; set; }
    }
}
