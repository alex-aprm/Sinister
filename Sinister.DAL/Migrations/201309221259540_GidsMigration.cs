namespace Sinister.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GidsMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Identifies", "Customer_Gid", "dbo.Customers");
            DropForeignKey("dbo.Identifies", "Type_Gid", "dbo.DictionaryRecords");
            DropIndex("dbo.Identifies", new[] { "Customer_Gid" });
            DropIndex("dbo.Identifies", new[] { "Type_Gid" });
            RenameColumn(table: "dbo.DictionaryRecords", name: "Dictionary_Gid", newName: "DictionaryGid");
            RenameColumn(table: "dbo.Identifies", name: "Customer_Gid", newName: "CustomerGid");
            RenameColumn(table: "dbo.Identifies", name: "Type_Gid", newName: "TypeGid");
            AddForeignKey("dbo.Identifies", "CustomerGid", "dbo.Customers", "Gid", cascadeDelete: true);
            AddForeignKey("dbo.Identifies", "TypeGid", "dbo.DictionaryRecords", "Gid", cascadeDelete: true);
            CreateIndex("dbo.Identifies", "CustomerGid");
            CreateIndex("dbo.Identifies", "TypeGid");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Identifies", new[] { "TypeGid" });
            DropIndex("dbo.Identifies", new[] { "CustomerGid" });
            DropForeignKey("dbo.Identifies", "TypeGid", "dbo.DictionaryRecords");
            DropForeignKey("dbo.Identifies", "CustomerGid", "dbo.Customers");
            RenameColumn(table: "dbo.Identifies", name: "TypeGid", newName: "Type_Gid");
            RenameColumn(table: "dbo.Identifies", name: "CustomerGid", newName: "Customer_Gid");
            RenameColumn(table: "dbo.DictionaryRecords", name: "DictionaryGid", newName: "Dictionary_Gid");
            CreateIndex("dbo.Identifies", "Type_Gid");
            CreateIndex("dbo.Identifies", "Customer_Gid");
            AddForeignKey("dbo.Identifies", "Type_Gid", "dbo.DictionaryRecords", "Gid");
            AddForeignKey("dbo.Identifies", "Customer_Gid", "dbo.Customers", "Gid");
        }
    }
}
