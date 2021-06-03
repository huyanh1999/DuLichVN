namespace DC.Entities.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Menu")]
    public partial class Menu
    {
        public int Id { get; set; }

        public int? ItemId { get; set; }

        public int ParentId { get; set; }

        [StringLength(512)]
        public string Name { get; set; }

        [StringLength(512)]
        public string Alias { get; set; }

        [StringLength(512)]
        public string Url { get; set; }

        [StringLength(50)]
        public string Target { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(50)]
        public string MenuType { get; set; }

        [StringLength(50)]
        public string MenuIcon { get; set; }

        public int OrderBy { get; set; }
    }
}
