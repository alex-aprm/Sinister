namespace Sinister.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomersMigration1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Identifies", "IssueDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Identifies", "IssueDate", c => c.DateTime(nullable: false));
        }
    }
}
