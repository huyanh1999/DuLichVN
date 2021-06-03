namespace DC.Entities.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Parameter")]
    public partial class Parameter
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        [StringLength(50)]
        public string Value { get; set; }
    }
}
