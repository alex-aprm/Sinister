namespace Sinister.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GidsMigration1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Identifies", "TypeGid", "dbo.DictionaryRecords");
            DropIndex("dbo.Identifies", new[] { "TypeGid" });
            AlterColumn("dbo.Identifies", "TypeGid", c => c.Guid());
            AddForeignKey("dbo.Identifies", "TypeGid", "dbo.DictionaryRecords", "Gid");
            CreateIndex("dbo.Identifies", "TypeGid");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Identifies", new[] { "TypeGid" });
            DropForeignKey("dbo.Identifies", "TypeGid", "dbo.DictionaryRecords");
            AlterColumn("dbo.Identifies", "TypeGid", c => c.Guid(nullable: false));
            CreateIndex("dbo.Identifies", "TypeGid");
            AddForeignKey("dbo.Identifies", "TypeGid", "dbo.DictionaryRecords", "Gid", cascadeDelete: true);
        }
    }
}
