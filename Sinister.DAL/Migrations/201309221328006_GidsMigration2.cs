namespace Sinister.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GidsMigration2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Identifies", "CustomerGid", "dbo.Customers");
            DropIndex("dbo.Identifies", new[] { "CustomerGid" });
            RenameColumn(table: "dbo.Identifies", name: "CustomerGid", newName: "Customer_Gid");
            AddForeignKey("dbo.Identifies", "Customer_Gid", "dbo.Customers", "Gid");
            CreateIndex("dbo.Identifies", "Customer_Gid");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Identifies", new[] { "Customer_Gid" });
            DropForeignKey("dbo.Identifies", "Customer_Gid", "dbo.Customers");
            RenameColumn(table: "dbo.Identifies", name: "Customer_Gid", newName: "CustomerGid");
            CreateIndex("dbo.Identifies", "CustomerGid");
            AddForeignKey("dbo.Identifies", "CustomerGid", "dbo.Customers", "Gid", cascadeDelete: true);
        }
    }
}
