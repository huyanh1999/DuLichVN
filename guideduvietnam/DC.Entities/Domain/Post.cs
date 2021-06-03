namespace DC.Entities.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Post")]
    public partial class Post
    {
        public int Id { get; set; }

        [Required]
        [StringLength(512)]
        public string KeySlug { get; set; }

        [Required]
        [StringLength(512)]
        public string Name { get; set; }

        public int CateId { get; set; }

        [StringLength(512)]
        public string CateName { get; set; }

        [StringLength(512)]
        public string Title { get; set; }

        [StringLength(512)]
        public string Url { get; set; }

        [StringLength(500)]
        public string Picture { get; set; }

        public string Description { get; set; }

        [StringLength(500)]
        public string MetaKeyword { get; set; }

        [StringLength(500)]
        public string MetaDescription { get; set; }

        public string OverView { get; set; }

        public string Content { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public int ViewCount { get; set; }

        public string TagsId { get; set; }

        [StringLength(50)]
        public string PostType { get; set; }

        [StringLength(250)]
        public string CreateByUser { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public bool? IsComment { get; set; }
        [StringLength(50)]
        public string Target { get; set; }
    }
}
