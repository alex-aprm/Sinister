namespace Sinister.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CoreCustomerMigration2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DictionaryRecords", "Dictionary_Gid", "dbo.Dictionaries");
            DropIndex("dbo.DictionaryRecords", new[] { "Dictionary_Gid" });
            AlterColumn("dbo.DictionaryRecords", "Dictionary_Gid", c => c.Guid(nullable: false));
            AddForeignKey("dbo.DictionaryRecords", "Dictionary_Gid", "dbo.Dictionaries", "Gid", cascadeDelete: true);
            CreateIndex("dbo.DictionaryRecords", "Dictionary_Gid");
        }
        
        public override void Down()
        {
            DropIndex("dbo.DictionaryRecords", new[] { "Dictionary_Gid" });
            DropForeignKey("dbo.DictionaryRecords", "Dictionary_Gid", "dbo.Dictionaries");
            AlterColumn("dbo.DictionaryRecords", "Dictionary_Gid", c => c.Guid());
            CreateIndex("dbo.DictionaryRecords", "Dictionary_Gid");
            AddForeignKey("dbo.DictionaryRecords", "Dictionary_Gid", "dbo.Dictionaries", "Gid");
        }
    }
}
