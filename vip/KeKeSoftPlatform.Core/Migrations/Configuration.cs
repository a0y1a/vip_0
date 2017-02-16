namespace KeKeSoftPlatform.Core.Migrations
{
    using KeKeSoftPlatform.Common;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<KeKeSoftPlatform.Core.KeKeSoftPlatformDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(KeKeSoftPlatform.Core.KeKeSoftPlatformDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            if (context.Admin.Any(m => m.Code == "admin") == false)
            {
                context.Admin.Add(new T_Admin { Code = "admin", CreateDate = DateTime.Now, Pwd = EncryptUtils.MD5Encrypt("admin") });
            }
        }
    }
}
