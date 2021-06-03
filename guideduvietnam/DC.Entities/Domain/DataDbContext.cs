namespace DC.Entities.Domain
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataDbContext : DbContext
    {
        public DataDbContext()
            : base("name=DataDbContext")
        {
        }

        public virtual DbSet<ActivityLog> ActivityLogs { get; set; }
        public virtual DbSet<ActivityLogType> ActivityLogTypes { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Parameter> Parameters { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostImage> PostImages { get; set; }
        public virtual DbSet<Slide> Slides { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);
        }
    }
}
