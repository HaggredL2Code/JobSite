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
                        cityID = c.Int(nullable: false),
                        UserRoleID = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.City", t => t.cityID)
                .ForeignKey("dbo.UserRole", t => t.UserRoleID)
                .Index(t => t.apEMail, unique: true, name: "IX_Unique_Email")
                .Index(t => t.cityID)
                .Index(t => t.UserRoleID);
            
            CreateTable(
                "dbo.Application",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Priority = c.Int(nullable: false),
                        PostingID = c.Int(nullable: false),
                        ApplicantID = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Posting", t => t.PostingID)
                .ForeignKey("dbo.Applicant", t => t.ApplicantID, cascadeDelete: true)
                .Index(t => t.PostingID)
                .Index(t => t.ApplicantID);
            
            CreateTable(
                "dbo.BinaryFile",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        fileName = c.String(maxLength: 256),
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
                "dbo.Qualification",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        QlfDescription = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.QlfDescription, unique: true, name: "IX_Unique_QlfDesc");
            
            CreateTable(
                "dbo.JobRequirement",
                c => new
                    {
                        PositionID = c.Int(nullable: false),
                        QualificationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PositionID, t.QualificationID })
                .ForeignKey("dbo.Position", t => t.PositionID, cascadeDelete: true)
                .ForeignKey("dbo.Qualification", t => t.QualificationID)
                .Index(t => t.PositionID)
                .Index(t => t.QualificationID);
            
            CreateTable(
                "dbo.Position",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PositionCode = c.String(nullable: false, maxLength: 5),
                        PositionDescription = c.String(nullable: false, maxLength: 50),
                        PositionFTE = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PositionSalary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PositionCompensationType = c.Short(nullable: false),
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
                "dbo.JobGroup",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GroupTitle = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.GroupTitle, unique: true, name: "IX_Unique_Title");
            
            CreateTable(
                "dbo.JobLocation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PositionID = c.Int(nullable: false),
                        LocationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Location", t => t.LocationID, cascadeDelete: true)
                .ForeignKey("dbo.Position", t => t.PositionID, cascadeDelete: true)
                .Index(t => t.PositionID)
                .Index(t => t.LocationID);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Street = c.String(nullable: false, maxLength: 50),
                        CityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.City", t => t.CityID)
                .Index(t => t.CityID);
            
            CreateTable(
                "dbo.City",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        city = c.String(nullable: false, maxLength: 100),
                        provinceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Province", t => t.provinceID)
                .Index(t => t.city, unique: true, name: "IX_Unique_City")
                .Index(t => t.provinceID);
            
            CreateTable(
                "dbo.Province",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        province = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.province, unique: true, name: "IX_Unique_Province");
            
            CreateTable(
                "dbo.Posting",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        pstNumPosition = c.Int(nullable: false),
                        pstJobDescription = c.String(nullable: false),
                        pstOpenDate = c.DateTime(nullable: false),
                        pstEndDate = c.DateTime(nullable: false),
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
                "dbo.Union",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UnionName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.UnionName, unique: true, name: "IX_Unique_Name");
            
            CreateTable(
                "dbo.Interview",
                c => new
                    {
                        InterviewID = c.Int(nullable: false),
                        interviewDate = c.DateTime(nullable: false),
                        Accepted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.InterviewID)
                .ForeignKey("dbo.Application", t => t.InterviewID, cascadeDelete: true)
                .Index(t => t.InterviewID);
            
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
                "dbo.ApplicationCart",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Priority = c.Int(nullable: false),
                        ApplicantID = c.Int(nullable: false),
                        PostingID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.BinaryFileTemp",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        fileName = c.String(maxLength: 256),
                        Description = c.String(maxLength: 256),
                        ApplicationCartID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ApplicationCart", t => t.ApplicationCartID, cascadeDelete: true)
                .Index(t => t.ApplicationCartID);
            
            CreateTable(
                "dbo.FileContentTemp",
                c => new
                    {
                        FileContentTempID = c.Int(nullable: false),
                        Content = c.Binary(),
                        MimeType = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.FileContentTempID)
                .ForeignKey("dbo.BinaryFileTemp", t => t.FileContentTempID, cascadeDelete: true)
                .Index(t => t.FileContentTempID);
            
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
                "dbo.InterviewCart",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ApplicationID = c.Int(nullable: false),
                        InterviewDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.DayPosition",
                c => new
                    {
                        Day_ID = c.Int(nullable: false),
                        Position_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Day_ID, t.Position_ID })
                .ForeignKey("dbo.Day", t => t.Day_ID, cascadeDelete: true)
                .ForeignKey("dbo.Position", t => t.Position_ID, cascadeDelete: true)
                .Index(t => t.Day_ID)
                .Index(t => t.Position_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BinaryFileTemp", "ApplicationCartID", "dbo.ApplicationCart");
            DropForeignKey("dbo.FileContentTemp", "FileContentTempID", "dbo.BinaryFileTemp");
            DropForeignKey("dbo.Applicant", "UserRoleID", "dbo.UserRole");
            DropForeignKey("dbo.Application", "ApplicantID", "dbo.Applicant");
            DropForeignKey("dbo.Interview", "InterviewID", "dbo.Application");
            DropForeignKey("dbo.ApplicationQualification", "ApplicationID", "dbo.Application");
            DropForeignKey("dbo.ApplicationQualification", "QualificationID", "dbo.Qualification");
            DropForeignKey("dbo.JobRequirement", "QualificationID", "dbo.Qualification");
            DropForeignKey("dbo.Position", "UnionID", "dbo.Union");
            DropForeignKey("dbo.Posting", "PositionID", "dbo.Position");
            DropForeignKey("dbo.Application", "PostingID", "dbo.Posting");
            DropForeignKey("dbo.JobRequirement", "PositionID", "dbo.Position");
            DropForeignKey("dbo.JobLocation", "PositionID", "dbo.Position");
            DropForeignKey("dbo.JobLocation", "LocationID", "dbo.Location");
            DropForeignKey("dbo.City", "provinceID", "dbo.Province");
            DropForeignKey("dbo.Location", "CityID", "dbo.City");
            DropForeignKey("dbo.Applicant", "cityID", "dbo.City");
            DropForeignKey("dbo.Position", "JobGroupID", "dbo.JobGroup");
            DropForeignKey("dbo.DayPosition", "Position_ID", "dbo.Position");
            DropForeignKey("dbo.DayPosition", "Day_ID", "dbo.Day");
            DropForeignKey("dbo.BinaryFile", "ApplicationID", "dbo.Application");
            DropForeignKey("dbo.FileContent", "FileContentID", "dbo.BinaryFile");
            DropIndex("dbo.DayPosition", new[] { "Position_ID" });
            DropIndex("dbo.DayPosition", new[] { "Day_ID" });
            DropIndex("dbo.FileContentTemp", new[] { "FileContentTempID" });
            DropIndex("dbo.BinaryFileTemp", new[] { "ApplicationCartID" });
            DropIndex("dbo.UserRole", "IX_Unique_Role");
            DropIndex("dbo.Interview", new[] { "InterviewID" });
            DropIndex("dbo.Union", "IX_Unique_Name");
            DropIndex("dbo.Posting", new[] { "PositionID" });
            DropIndex("dbo.Province", "IX_Unique_Province");
            DropIndex("dbo.City", new[] { "provinceID" });
            DropIndex("dbo.City", "IX_Unique_City");
            DropIndex("dbo.Location", new[] { "CityID" });
            DropIndex("dbo.JobLocation", new[] { "LocationID" });
            DropIndex("dbo.JobLocation", new[] { "PositionID" });
            DropIndex("dbo.JobGroup", "IX_Unique_Title");
            DropIndex("dbo.Day", "IX_Unique_Day");
            DropIndex("dbo.Position", new[] { "UnionID" });
            DropIndex("dbo.Position", new[] { "JobGroupID" });
            DropIndex("dbo.Position", "IX_Unique_Desc");
            DropIndex("dbo.Position", "IX_Unique_Code");
            DropIndex("dbo.JobRequirement", new[] { "QualificationID" });
            DropIndex("dbo.JobRequirement", new[] { "PositionID" });
            DropIndex("dbo.Qualification", "IX_Unique_QlfDesc");
            DropIndex("dbo.ApplicationQualification", new[] { "QualificationID" });
            DropIndex("dbo.ApplicationQualification", new[] { "ApplicationID" });
            DropIndex("dbo.FileContent", new[] { "FileContentID" });
            DropIndex("dbo.BinaryFile", new[] { "ApplicationID" });
            DropIndex("dbo.Application", new[] { "ApplicantID" });
            DropIndex("dbo.Application", new[] { "PostingID" });
            DropIndex("dbo.Applicant", new[] { "UserRoleID" });
            DropIndex("dbo.Applicant", new[] { "cityID" });
            DropIndex("dbo.Applicant", "IX_Unique_Email");
            DropTable("dbo.DayPosition");
            DropTable("dbo.InterviewCart");
            DropTable("dbo.Archive");
            DropTable("dbo.FileContentTemp");
            DropTable("dbo.BinaryFileTemp");
            DropTable("dbo.ApplicationCart");
            DropTable("dbo.UserRole");
            DropTable("dbo.Interview");
            DropTable("dbo.Union");
            DropTable("dbo.Posting");
            DropTable("dbo.Province");
            DropTable("dbo.City");
            DropTable("dbo.Location");
            DropTable("dbo.JobLocation");
            DropTable("dbo.JobGroup");
            DropTable("dbo.Day");
            DropTable("dbo.Position");
            DropTable("dbo.JobRequirement");
            DropTable("dbo.Qualification");
            DropTable("dbo.ApplicationQualification");
            DropTable("dbo.FileContent");
            DropTable("dbo.BinaryFile");
            DropTable("dbo.Application");
            DropTable("dbo.Applicant");
        }
    }
}
