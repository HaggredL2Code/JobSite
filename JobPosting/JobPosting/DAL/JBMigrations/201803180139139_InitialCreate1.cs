namespace JobPosting.DAL.JBMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DayPosition", "Day_ID", "dbo.Day");
            DropForeignKey("dbo.DayPosition", "Position_ID", "dbo.Position");
            DropForeignKey("dbo.JobLocation", "PositionID", "dbo.Position");
            DropForeignKey("dbo.JobRequirement", "PositionID", "dbo.Position");
            DropIndex("dbo.JobRequirement", new[] { "PositionID" });
            DropIndex("dbo.JobLocation", new[] { "PositionID" });
            DropIndex("dbo.DayPosition", new[] { "Day_ID" });
            DropIndex("dbo.DayPosition", new[] { "Position_ID" });
            DropPrimaryKey("dbo.JobRequirement");
            CreateTable(
                "dbo.ApplicationSkill",
                c => new
                    {
                        ApplicationId = c.Int(nullable: false),
                        skillID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationId, t.skillID })
                .ForeignKey("dbo.Skill", t => t.skillID)
                .ForeignKey("dbo.Application", t => t.ApplicationId, cascadeDelete: true)
                .Index(t => t.ApplicationId)
                .Index(t => t.skillID);
            
            CreateTable(
                "dbo.Skill",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SkillDescription = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.SkillDescription, unique: true, name: "IX_Unique_SkillDesc");
            
            CreateTable(
                "dbo.PostingSkill",
                c => new
                    {
                        PostingID = c.Int(nullable: false),
                        SkillID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PostingID, t.SkillID })
                .ForeignKey("dbo.Posting", t => t.PostingID, cascadeDelete: true)
                .ForeignKey("dbo.Skill", t => t.SkillID)
                .Index(t => t.PostingID)
                .Index(t => t.SkillID);
            
            CreateTable(
                "dbo.PostingTemplate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        templateName = c.String(maxLength: 255),
                        pstNumPosition = c.Int(nullable: false),
                        pstFTE = c.Decimal(nullable: false, precision: 18, scale: 2),
                        pstSalary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        pstCompensationType = c.Short(nullable: false),
                        pstJobDescription = c.String(),
                        pstOpenDate = c.DateTime(nullable: false),
                        pstEndDate = c.DateTime(nullable: false),
                        PositionID = c.Int(nullable: false),
                        RequirementIDs = c.String(),
                        SkillIDs = c.String(),
                        LocationIDs = c.String(),
                        dayIDs = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.templateName, unique: true, name: "IX_Unique_Name");
            
            CreateTable(
                "dbo.DayPosting",
                c => new
                    {
                        Day_ID = c.Int(nullable: false),
                        Posting_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Day_ID, t.Posting_ID })
                .ForeignKey("dbo.Day", t => t.Day_ID, cascadeDelete: true)
                .ForeignKey("dbo.Posting", t => t.Posting_ID, cascadeDelete: true)
                .Index(t => t.Day_ID)
                .Index(t => t.Posting_ID);
            
            AddColumn("dbo.JobRequirement", "PostingID", c => c.Int(nullable: false));
            AddColumn("dbo.JobLocation", "PostingID", c => c.Int(nullable: false));
            AddColumn("dbo.Posting", "pstFTE", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Posting", "pstSalary", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Posting", "pstCompensationType", c => c.Short(nullable: false));
            AddColumn("dbo.Posting", "Enabled", c => c.Boolean(nullable: false));
            AddPrimaryKey("dbo.JobRequirement", new[] { "PostingID", "QualificationID" });
            CreateIndex("dbo.JobLocation", "PostingID");
            CreateIndex("dbo.JobRequirement", "PostingID");
            AddForeignKey("dbo.JobLocation", "PostingID", "dbo.Posting", "ID", cascadeDelete: true);
            AddForeignKey("dbo.JobRequirement", "PostingID", "dbo.Posting", "ID", cascadeDelete: true);
            DropColumn("dbo.JobRequirement", "PositionID");
            DropColumn("dbo.Position", "PositionFTE");
            DropColumn("dbo.Position", "PositionSalary");
            DropColumn("dbo.Position", "PositionCompensationType");
            DropColumn("dbo.JobLocation", "PositionID");
            DropTable("dbo.DayPosition");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DayPosition",
                c => new
                    {
                        Day_ID = c.Int(nullable: false),
                        Position_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Day_ID, t.Position_ID });
            
            AddColumn("dbo.JobLocation", "PositionID", c => c.Int(nullable: false));
            AddColumn("dbo.Position", "PositionCompensationType", c => c.Short(nullable: false));
            AddColumn("dbo.Position", "PositionSalary", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Position", "PositionFTE", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.JobRequirement", "PositionID", c => c.Int(nullable: false));
            DropForeignKey("dbo.ApplicationSkill", "ApplicationId", "dbo.Application");
            DropForeignKey("dbo.ApplicationSkill", "skillID", "dbo.Skill");
            DropForeignKey("dbo.PostingSkill", "SkillID", "dbo.Skill");
            DropForeignKey("dbo.PostingSkill", "PostingID", "dbo.Posting");
            DropForeignKey("dbo.JobRequirement", "PostingID", "dbo.Posting");
            DropForeignKey("dbo.JobLocation", "PostingID", "dbo.Posting");
            DropForeignKey("dbo.DayPosting", "Posting_ID", "dbo.Posting");
            DropForeignKey("dbo.DayPosting", "Day_ID", "dbo.Day");
            DropIndex("dbo.DayPosting", new[] { "Posting_ID" });
            DropIndex("dbo.DayPosting", new[] { "Day_ID" });
            DropIndex("dbo.PostingTemplate", "IX_Unique_Name");
            DropIndex("dbo.JobRequirement", new[] { "PostingID" });
            DropIndex("dbo.JobLocation", new[] { "PostingID" });
            DropIndex("dbo.PostingSkill", new[] { "SkillID" });
            DropIndex("dbo.PostingSkill", new[] { "PostingID" });
            DropIndex("dbo.Skill", "IX_Unique_SkillDesc");
            DropIndex("dbo.ApplicationSkill", new[] { "skillID" });
            DropIndex("dbo.ApplicationSkill", new[] { "ApplicationId" });
            DropPrimaryKey("dbo.JobRequirement");
            DropColumn("dbo.Posting", "Enabled");
            DropColumn("dbo.Posting", "pstCompensationType");
            DropColumn("dbo.Posting", "pstSalary");
            DropColumn("dbo.Posting", "pstFTE");
            DropColumn("dbo.JobLocation", "PostingID");
            DropColumn("dbo.JobRequirement", "PostingID");
            DropTable("dbo.DayPosting");
            DropTable("dbo.PostingTemplate");
            DropTable("dbo.PostingSkill");
            DropTable("dbo.Skill");
            DropTable("dbo.ApplicationSkill");
            AddPrimaryKey("dbo.JobRequirement", new[] { "PositionID", "QualificationID" });
            CreateIndex("dbo.DayPosition", "Position_ID");
            CreateIndex("dbo.DayPosition", "Day_ID");
            CreateIndex("dbo.JobLocation", "PositionID");
            CreateIndex("dbo.JobRequirement", "PositionID");
            AddForeignKey("dbo.JobRequirement", "PositionID", "dbo.Position", "ID", cascadeDelete: true);
            AddForeignKey("dbo.JobLocation", "PositionID", "dbo.Position", "ID", cascadeDelete: true);
            AddForeignKey("dbo.DayPosition", "Position_ID", "dbo.Position", "ID", cascadeDelete: true);
            AddForeignKey("dbo.DayPosition", "Day_ID", "dbo.Day", "ID", cascadeDelete: true);
        }
    }
}
