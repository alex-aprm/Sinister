namespace Sinister.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CoreCustomerMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dictionaries",
                c => new
                    {
                        Gid = c.Guid(nullable: false),
                        Name = c.String(maxLength: 200),
                        SID = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Gid);
            
            CreateTable(
                "dbo.DictionaryRecords",
                c => new
                    {
                        Gid = c.Guid(nullable: false),
                        OrderNumber = c.Int(nullable: false),
                        Name = c.String(maxLength: 200),
                        SID = c.String(maxLength: 200),
                        Dictionary_Gid = c.Guid(),
                    })
                .PrimaryKey(t => t.Gid)
                .ForeignKey("dbo.Dictionaries", t => t.Dictionary_Gid)
                .Index(t => t.Dictionary_Gid);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Gid = c.Guid(nullable: false),
                        Code = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 500),
                        FirstName = c.String(maxLength: 500),
                        MiddleName = c.String(maxLength: 500),
                        INN = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Gid);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.DictionaryRecords", new[] { "Dictionary_Gid" });
            DropForeignKey("dbo.DictionaryRecords", "Dictionary_Gid", "dbo.Dictionaries");
            DropTable("dbo.Customers");
            DropTable("dbo.DictionaryRecords");
            DropTable("dbo.Dictionaries");
        }
    }
}
