namespace TrabalhoDM106.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TrabalhoDM106.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TrabalhoDM106.Models.TrabalhoDM106Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TrabalhoDM106.Models.TrabalhoDM106Context context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.Products.AddOrUpdate(
                p => p.Id,
                new Product
                {
                    Id = 1,
                    name = "ASUS HG51",
                    desc = "Placa Mae",
                    color = "black",
                    model = "USB 3.0",
                    cod = "02324",
                    price = 230.70,
                    weight = 30,
                    height = 30,
                    width = 30,
                    length = 30,
                    diameter = 30,
                    url = "www.esjdev.com"
                },
                new Product
                {
                    Id = 2,
                    name = "MacBook Pro",
                    desc = "Notebook Apple",
                    color = "gray",
                    model = "256GB",
                    cod = "06624",
                    price = 4500.70,
                    weight = 30,
                    height = 30,
                    width = 30,
                    length = 30,
                    diameter = 30,
                    url = "www.esjdev.com"
                },
                new Product
                {
                    Id = 3,
                    name = "Lenovo Ideia 14",
                    desc = "Notebook Lenovo",
                    color = "red",
                    model = "YogaPad 14",
                    cod = "09924",
                    price = 1500.70,
                    weight = 30,
                    height = 30,
                    width = 30,
                    length = 30,
                    diameter = 30,
                    url = "www.esjdev.com"
                }
                );
        }
    }
}
