namespace TrabalhoDM106.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderAndOrderItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        customerEmail = c.String(nullable: false),
                        orderDate = c.DateTime(nullable: false),
                        deliverDate = c.DateTime(nullable: false),
                        status = c.String(),
                        totalPrice = c.Double(nullable: false),
                        totalWeight = c.Double(nullable: false),
                        deliverPrice = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        qtd = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItems", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderItems", "ProductId", "dbo.Products");
            DropIndex("dbo.OrderItems", new[] { "OrderId" });
            DropIndex("dbo.OrderItems", new[] { "ProductId" });
            DropTable("dbo.OrderItems");
            DropTable("dbo.Orders");
        }
    }
}
