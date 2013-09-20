namespace Sinister.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CoreCustomerMigration1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Identifies",
                c => new
                    {
                        Gid = c.Guid(nullable: false),
                        Series = c.String(maxLength: 50),
                        Number = c.String(maxLength: 50),
                        IssueDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        Issuer = c.String(maxLength: 500),
                        IssuerCode = c.String(maxLength: 50),
                        IsValid = c.Boolean(nullable: false),
                        IsMain = c.Boolean(nullable: false),
                        Customer_Gid = c.Guid(),
                        IdentifyType_Gid = c.Guid(),
                    })
                .PrimaryKey(t => t.Gid)
                .ForeignKey("dbo.Customers", t => t.Customer_Gid)
                .ForeignKey("dbo.DictionaryRecords", t => t.IdentifyType_Gid)
                .Index(t => t.Customer_Gid)
                .Index(t => t.IdentifyType_Gid);
            
            AddColumn("dbo.Customers", "IsFullCustomer", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropIndex("dbo.Identifies", new[] { "IdentifyType_Gid" });
            DropIndex("dbo.Identifies", new[] { "Customer_Gid" });
            DropForeignKey("dbo.Identifies", "IdentifyType_Gid", "dbo.DictionaryRecords");
            DropForeignKey("dbo.Identifies", "Customer_Gid", "dbo.Customers");
            DropColumn("dbo.Customers", "IsFullCustomer");
            DropTable("dbo.Identifies");
        }
    }
}
