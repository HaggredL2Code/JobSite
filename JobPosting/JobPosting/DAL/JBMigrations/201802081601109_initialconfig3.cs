namespace JobPosting.DAL.JBMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialconfig3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Application", "interviewID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Application", "interviewID", c => c.Int(nullable: false));
        }
    }
}
