namespace EFReview
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PlutoCleanContext : DbContext
    {
        public PlutoCleanContext()
            : base("name=PlutoCleanContext")
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseSection> CourseSections { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<tblUser> tblUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.LevelString)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.Courses)
                .Map(m => m.ToTable("CourseTags").MapLeftKey("CourseID").MapRightKey("TagID"));

            modelBuilder.Entity<CourseSection>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.Body)
                .IsUnicode(false);

            modelBuilder.Entity<Tag>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<tblUser>()
                .Property(e => e.Username)
                .IsUnicode(false);
        }
    }
}
