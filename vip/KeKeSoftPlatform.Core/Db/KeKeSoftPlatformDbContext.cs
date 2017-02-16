using KeKeSoftPlatform.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeKeSoftPlatform.Core
{
    public class KeKeSoftPlatformDbContext : DbContext
    {
        static KeKeSoftPlatformDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<KeKeSoftPlatformDbContext, KeKeSoftPlatform.Core.Migrations.Configuration>());
        }
        public KeKeSoftPlatformDbContext()
            : base("KeKeSoftPlatformDb")
        {

        }
        public DbSet<T_Admin> Admin { get; set; }
        public DbSet<T_Role> Role { get; set; }
        public DbSet<T_Student> Student { get; set; }
        public DbSet<T_Class> Class { get; set; }
        public DbSet<T_PCC> PCC { get; set; }
        public DbSet<T_File> File { get; set; }
        public DbSet<T_Image> Image { get; set; }
        public DbSet<T_News> News { get; set; }
        public DbSet<T_StudentRoleLink> StudentRoleLink { get; set; }
        public DbSet<T_Test> Test { get; set; }
        public DbSet<T_Book> Book { get; set; }
        public DbSet<T_Author> Author { get; set; }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var newException = new FormattedDbEntityValidationException(ex);
                throw newException;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<T_Author>()
                        .HasMany(m => m.Book)
                        .WithRequired(m => m.Author)
                        .HasForeignKey(m => m.AuthorId);

            modelBuilder.Entity<T_Class>()
                        .HasMany(m => m.Student)
                        .WithRequired(m => m.Class)
                        .HasForeignKey(m => m.ClassId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<T_Student>()
                        .HasMany(m => m.StudentRoleLink)
                        .WithRequired(m => m.Student)
                        .HasForeignKey(m => m.StudentId);

            modelBuilder.Entity<T_Role>()
                        .HasMany(m => m.StudentRoleLink)
                        .WithRequired(m => m.Role)
                        .HasForeignKey(m => m.RoleId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
