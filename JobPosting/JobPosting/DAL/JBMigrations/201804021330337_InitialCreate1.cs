namespace JobPosting.DAL.JBMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Application", "Available", c => c.Boolean(nullable: false));
            AddColumn("dbo.Posting", "pstJobStartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Posting", "pstJobEndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PostingTemplate", "pstJobStartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PostingTemplate", "pstJobEndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PostingTemplate", "pstJobEndDate");
            DropColumn("dbo.PostingTemplate", "pstJobStartDate");
            DropColumn("dbo.Posting", "pstJobEndDate");
            DropColumn("dbo.Posting", "pstJobStartDate");
            DropColumn("dbo.Application", "Available");
        }
    }
}
