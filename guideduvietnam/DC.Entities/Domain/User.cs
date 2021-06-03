namespace DC.Entities.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public int Id { get; set; }

        [StringLength(150)]
        public string Guid { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(150)]
        public string FullName { get; set; }

        [StringLength(150)]
        public string Email { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(250)]
        public string Password { get; set; }

        [StringLength(50)]
        public string RoleName { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(500)]
        public string Url { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(500)]
        public string Picture { get; set; }

        public bool? ReceiveEmailNotification { get; set; }

        public bool IsLockedOut { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateByUserId { get; set; }
    }
}
