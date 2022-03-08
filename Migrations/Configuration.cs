namespace MiddleTestAuthetication.Migrations
{
    using MiddleTestAuthetication.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MiddleTestAuthetication.Data.MyIdentityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MiddleTestAuthetication.Data.MyIdentityDbContext context)
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
            var roles = new List<Role>
            {
                new Role {Name = "ADMIN"},
                new Role {Name = "USER"},
                new Role {Name = "EMPLOYEE"},
            };
            roles.ForEach(e => context.Roles.Add(e));
            context.SaveChanges();
        }
    }
}
