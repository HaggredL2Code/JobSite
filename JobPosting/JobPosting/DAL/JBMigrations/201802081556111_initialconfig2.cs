namespace JobPosting.DAL.JBMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialconfig2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Interview", "applicationID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Interview", "applicationID", c => c.Int(nullable: false));
        }
    }
}
