namespace JobPosting.DAL.JBMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate5 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Position", "PositionDayofWork");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Position", "PositionDayofWork", c => c.String(nullable: false));
        }
    }
}
