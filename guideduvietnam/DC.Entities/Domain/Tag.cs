namespace DC.Entities.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Tag")]
    public partial class Tag
    {
        public int Id { get; set; }

        [StringLength(512)]
        public string KeySlug { get; set; }

        [StringLength(512)]
        public string Name { get; set; }

        [StringLength(512)]
        public string Title { get; set; }

        [StringLength(512)]
        public string Url { get; set; }

        [StringLength(500)]
        public string MetaKeyword { get; set; }

        [StringLength(500)]
        public string MetaDescription { get; set; }

        [StringLength(50)]
        public string TagType { get; set; }
    }
}
