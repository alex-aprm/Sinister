namespace Sinister.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomersMigration3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Identifies", "Customer_Gid", "dbo.Customers");
            DropIndex("dbo.Identifies", new[] { "Customer_Gid" });
            AlterColumn("dbo.Identifies", "Customer_Gid", c => c.Guid(nullable: false));
            AddForeignKey("dbo.Identifies", "Customer_Gid", "dbo.Customers", "Gid", cascadeDelete: true);
            CreateIndex("dbo.Identifies", "Customer_Gid");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Identifies", new[] { "Customer_Gid" });
            DropForeignKey("dbo.Identifies", "Customer_Gid", "dbo.Customers");
            AlterColumn("dbo.Identifies", "Customer_Gid", c => c.Guid());
            CreateIndex("dbo.Identifies", "Customer_Gid");
            AddForeignKey("dbo.Identifies", "Customer_Gid", "dbo.Customers", "Gid");
        }
    }
}
