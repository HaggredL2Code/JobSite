namespace JobPosting.DAL.JBMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applicant",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        apFirstName = c.String(nullable: false, maxLength: 50),
                        apMiddleName = c.String(maxLength: 50),
                        apLastName = c.String(nullable: false, maxLength: 50),
                        apPhone = c.Long(nullable: false),
                        apSubscripted = c.Boolean(nullable: false),
                        apEMail = c.String(maxLength: 255),
                        apAddress = c.String(nullable: false),
                        apPostalCode = c.String(nullable: false),
                        UserRoleID = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserRole", t => t.UserRoleID)
                .Index(t => t.apEMail, unique: true, name: "IX_Unique_Email")
                .Index(t => t.UserRoleID);
            
            CreateTable(
                "dbo.Application",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Priority = c.Int(nullable: false),
                        PostingID = c.Int(nullable: false),
                        ApplicantID = c.Int(nullable: false),
                        Comment = c.String(maxLength: 2000),
                        Available = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Posting", t => t.PostingID, cascadeDelete: true)
                .ForeignKey("dbo.Applicant", t => t.ApplicantID, cascadeDelete: true)
                .Index(t => t.PostingID)
                .Index(t => t.ApplicantID);
            
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
                "dbo.Posting",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        pstNumPosition = c.Int(nullable: false),
                        pstFTE = c.Decimal(nullable: false, precision: 18, scale: 2),
                        pstSalary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        pstCompensationType = c.Short(nullable: false),
                        pstJobDescription = c.String(nullable: false),
                        pstOpenDate = c.DateTime(nullable: false),
                        pstEndDate = c.DateTime(nullable: false),
                        pstJobStartDate = c.DateTime(nullable: false),
                        pstJobEndDate = c.DateTime(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                        PositionID = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Position", t => t.PositionID, cascadeDelete: true)
                .Index(t => t.PositionID);
            
            CreateTable(
                "dbo.Day",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        dayName = c.String(nullable: false, maxLength: 50),
                        dayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.dayName, unique: true, name: "IX_Unique_Day");
            
            CreateTable(
                "dbo.JobLocation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PostingID = c.Int(nullable: false),
                        LocationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Location", t => t.LocationID, cascadeDelete: true)
                .ForeignKey("dbo.Posting", t => t.PostingID, cascadeDelete: true)
                .Index(t => t.PostingID)
                .Index(t => t.LocationID);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.JobRequirement",
                c => new
                    {
                        PostingID = c.Int(nullable: false),
                        QualificationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PostingID, t.QualificationID })
                .ForeignKey("dbo.Qualification", t => t.QualificationID)
                .ForeignKey("dbo.Posting", t => t.PostingID, cascadeDelete: true)
                .Index(t => t.PostingID)
                .Index(t => t.QualificationID);
            
            CreateTable(
                "dbo.Qualification",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        QlfDescription = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.QlfDescription, unique: true, name: "IX_Unique_QlfDesc");
            
            CreateTable(
                "dbo.ApplicationQualification",
                c => new
                    {
                        ApplicationID = c.Int(nullable: false),
                        QualificationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationID, t.QualificationID })
                .ForeignKey("dbo.Qualification", t => t.QualificationID)
                .ForeignKey("dbo.Application", t => t.ApplicationID, cascadeDelete: true)
                .Index(t => t.ApplicationID)
                .Index(t => t.QualificationID);
            
            CreateTable(
                "dbo.Position",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PositionCode = c.String(nullable: false, maxLength: 5),
                        PositionDescription = c.String(nullable: false, maxLength: 50),
                        JobGroupID = c.Int(nullable: false),
                        UnionID = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.JobGroup", t => t.JobGroupID, cascadeDelete: true)
                .ForeignKey("dbo.Union", t => t.UnionID)
                .Index(t => t.PositionCode, unique: true, name: "IX_Unique_Code")
                .Index(t => t.PositionDescription, name: "IX_Unique_Desc")
                .Index(t => t.JobGroupID)
                .Index(t => t.UnionID);
            
            CreateTable(
                "dbo.JobGroup",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GroupTitle = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.GroupTitle, unique: true, name: "IX_Unique_Title");
            
            CreateTable(
                "dbo.Union",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UnionName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.UnionName, unique: true, name: "IX_Unique_Name");
            
            CreateTable(
                "dbo.BinaryFile",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 256),
                        Description = c.String(maxLength: 256),
                        ApplicationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Application", t => t.ApplicationID, cascadeDelete: true)
                .Index(t => t.ApplicationID);
            
            CreateTable(
                "dbo.FileContent",
                c => new
                    {
                        FileContentID = c.Int(nullable: false),
                        Content = c.Binary(),
                        MimeType = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.FileContentID)
                .ForeignKey("dbo.BinaryFile", t => t.FileContentID, cascadeDelete: true)
                .Index(t => t.FileContentID);
            
            CreateTable(
                "dbo.Picked",
                c => new
                    {
                        PickedID = c.Int(nullable: false),
                        jobTypePrevPicked1 = c.Int(nullable: false),
                        jobTypePrevPicked2 = c.Int(nullable: false),
                        jobTypeJustPicked = c.Int(nullable: false),
                        firstTimeAccess = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PickedID)
                .ForeignKey("dbo.Applicant", t => t.PickedID)
                .Index(t => t.PickedID);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RoleTitle = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.RoleTitle, unique: true, name: "IX_Unique_Role");
            
            CreateTable(
                "dbo.Archive",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EmployeeName = c.String(),
                        EmployeePhone = c.String(),
                        EmployeeAddress = c.String(),
                        EmployeePosition = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
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
                        pstJobStartDate = c.DateTime(nullable: false),
                        pstJobEndDate = c.DateTime(nullable: false),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Applicant", "UserRoleID", "dbo.UserRole");
            DropForeignKey("dbo.Picked", "PickedID", "dbo.Applicant");
            DropForeignKey("dbo.Application", "ApplicantID", "dbo.Applicant");
            DropForeignKey("dbo.BinaryFile", "ApplicationID", "dbo.Application");
            DropForeignKey("dbo.FileContent", "FileContentID", "dbo.BinaryFile");
            DropForeignKey("dbo.ApplicationQualification", "ApplicationID", "dbo.Application");
            DropForeignKey("dbo.ApplicationSkill", "ApplicationId", "dbo.Application");
            DropForeignKey("dbo.ApplicationSkill", "skillID", "dbo.Skill");
            DropForeignKey("dbo.PostingSkill", "SkillID", "dbo.Skill");
            DropForeignKey("dbo.PostingSkill", "PostingID", "dbo.Posting");
            DropForeignKey("dbo.Position", "UnionID", "dbo.Union");
            DropForeignKey("dbo.Posting", "PositionID", "dbo.Position");
            DropForeignKey("dbo.Position", "JobGroupID", "dbo.JobGroup");
            DropForeignKey("dbo.JobRequirement", "PostingID", "dbo.Posting");
            DropForeignKey("dbo.JobRequirement", "QualificationID", "dbo.Qualification");
            DropForeignKey("dbo.ApplicationQualification", "QualificationID", "dbo.Qualification");
            DropForeignKey("dbo.JobLocation", "PostingID", "dbo.Posting");
            DropForeignKey("dbo.JobLocation", "LocationID", "dbo.Location");
            DropForeignKey("dbo.DayPosting", "Posting_ID", "dbo.Posting");
            DropForeignKey("dbo.DayPosting", "Day_ID", "dbo.Day");
            DropForeignKey("dbo.Application", "PostingID", "dbo.Posting");
            DropIndex("dbo.DayPosting", new[] { "Posting_ID" });
            DropIndex("dbo.DayPosting", new[] { "Day_ID" });
            DropIndex("dbo.PostingTemplate", "IX_Unique_Name");
            DropIndex("dbo.UserRole", "IX_Unique_Role");
            DropIndex("dbo.Picked", new[] { "PickedID" });
            DropIndex("dbo.FileContent", new[] { "FileContentID" });
            DropIndex("dbo.BinaryFile", new[] { "ApplicationID" });
            DropIndex("dbo.Union", "IX_Unique_Name");
            DropIndex("dbo.JobGroup", "IX_Unique_Title");
            DropIndex("dbo.Position", new[] { "UnionID" });
            DropIndex("dbo.Position", new[] { "JobGroupID" });
            DropIndex("dbo.Position", "IX_Unique_Desc");
            DropIndex("dbo.Position", "IX_Unique_Code");
            DropIndex("dbo.ApplicationQualification", new[] { "QualificationID" });
            DropIndex("dbo.ApplicationQualification", new[] { "ApplicationID" });
            DropIndex("dbo.Qualification", "IX_Unique_QlfDesc");
            DropIndex("dbo.JobRequirement", new[] { "QualificationID" });
            DropIndex("dbo.JobRequirement", new[] { "PostingID" });
            DropIndex("dbo.JobLocation", new[] { "LocationID" });
            DropIndex("dbo.JobLocation", new[] { "PostingID" });
            DropIndex("dbo.Day", "IX_Unique_Day");
            DropIndex("dbo.Posting", new[] { "PositionID" });
            DropIndex("dbo.PostingSkill", new[] { "SkillID" });
            DropIndex("dbo.PostingSkill", new[] { "PostingID" });
            DropIndex("dbo.Skill", "IX_Unique_SkillDesc");
            DropIndex("dbo.ApplicationSkill", new[] { "skillID" });
            DropIndex("dbo.ApplicationSkill", new[] { "ApplicationId" });
            DropIndex("dbo.Application", new[] { "ApplicantID" });
            DropIndex("dbo.Application", new[] { "PostingID" });
            DropIndex("dbo.Applicant", new[] { "UserRoleID" });
            DropIndex("dbo.Applicant", "IX_Unique_Email");
            DropTable("dbo.DayPosting");
            DropTable("dbo.PostingTemplate");
            DropTable("dbo.Archive");
            DropTable("dbo.UserRole");
            DropTable("dbo.Picked");
            DropTable("dbo.FileContent");
            DropTable("dbo.BinaryFile");
            DropTable("dbo.Union");
            DropTable("dbo.JobGroup");
            DropTable("dbo.Position");
            DropTable("dbo.ApplicationQualification");
            DropTable("dbo.Qualification");
            DropTable("dbo.JobRequirement");
            DropTable("dbo.Location");
            DropTable("dbo.JobLocation");
            DropTable("dbo.Day");
            DropTable("dbo.Posting");
            DropTable("dbo.PostingSkill");
            DropTable("dbo.Skill");
            DropTable("dbo.ApplicationSkill");
            DropTable("dbo.Application");
            DropTable("dbo.Applicant");
        }
    }
}
