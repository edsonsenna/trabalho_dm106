namespace TrabalhoDM106.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        desc = c.String(),
                        color = c.String(),
                        model = c.String(nullable: false),
                        cod = c.String(nullable: false),
                        price = c.Double(nullable: false),
                        weight = c.Double(nullable: false),
                        height = c.Double(nullable: false),
                        width = c.Double(nullable: false),
                        length = c.Double(nullable: false),
                        diameter = c.Double(nullable: false),
                        url = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Products");
        }
    }
}
