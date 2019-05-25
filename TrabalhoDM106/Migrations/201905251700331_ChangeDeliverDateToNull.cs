namespace TrabalhoDM106.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDeliverDateToNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "deliverDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "deliverDate", c => c.DateTime(nullable: false));
        }
    }
}
