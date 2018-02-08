namespace JobPosting.DAL.JBMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialconfig4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Location", "Province");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Location", "Province", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
