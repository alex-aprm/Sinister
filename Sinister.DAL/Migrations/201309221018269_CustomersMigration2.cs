namespace Sinister.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomersMigration2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Identifies", "IdentifyType_Gid", "dbo.DictionaryRecords");
            DropIndex("dbo.Identifies", new[] { "IdentifyType_Gid" });
            AddColumn("dbo.Identifies", "Type_Gid", c => c.Guid());
            AddForeignKey("dbo.Identifies", "Type_Gid", "dbo.DictionaryRecords", "Gid");
            CreateIndex("dbo.Identifies", "Type_Gid");
            DropColumn("dbo.Identifies", "IdentifyType_Gid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Identifies", "IdentifyType_Gid", c => c.Guid());
            DropIndex("dbo.Identifies", new[] { "Type_Gid" });
            DropForeignKey("dbo.Identifies", "Type_Gid", "dbo.DictionaryRecords");
            DropColumn("dbo.Identifies", "Type_Gid");
            CreateIndex("dbo.Identifies", "IdentifyType_Gid");
            AddForeignKey("dbo.Identifies", "IdentifyType_Gid", "dbo.DictionaryRecords", "Gid");
        }
    }
}
