namespace JobPosting.DAL.JBMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;
    using JobPosting.Models;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<JobPosting.DAL.JBEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DAL\JBMigrations";
        }

        private void SaveChanges(DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
            catch (Exception e)
            {
                throw new Exception(
                     "Seed Failed - errors follow:\n" +
                     e.InnerException.InnerException.Message.ToString(), e
                 ); // Add the original exception as the innerException
            }
        }

        protected override void Seed(JobPosting.DAL.JBEntities context)
        {

            var applicants = new List<Applicant>
            {
                new Applicant { apFirstName = "kevin", apMiddleName = "m", apLastName = "Marty", apPhone = 905-667-7777, apPostalCode = "L2S5G6", apEMail = "testuser@hotmail.com", apAddress = "13 placeholder lane" , apSubscripted = false, cityID = (context.Cities.Where(p=>p.city == "St.Catharines").SingleOrDefault().ID), UserRoleID = 1 }
              
            };
            applicants.ForEach(a => context.Applicants.AddOrUpdate(n => n.apEMail, a));
            SaveChanges(context);

            var cities = new List<City>
            {
                new City { city = "St.Catharines", provinceID=(context.Provinces.Where(p=>p.province == "Ontario").SingleOrDefault().ID) },
                new City { city = "Wellend", provinceID=(context.Provinces.Where(p=>p.province == "Ontario").SingleOrDefault().ID)},
                new City { city = "Fort Erie", provinceID=(context.Provinces.Where(p=>p.province == "Ontario").SingleOrDefault().ID)},
                new City { city = "Niagara Falls", provinceID=(context.Provinces.Where(p=>p.province == "Ontario").SingleOrDefault().ID)},

            };
            cities.ForEach(a => context.Cities.AddOrUpdate(n => n.city, a));
            SaveChanges(context);

            var provinces = new List<Province>
            {
                new Province { province = "Ontario"},
                new Province { province = "Qubec"}
            };
            provinces.ForEach(a => context.Provinces.AddOrUpdate(n => n.province, a));
            SaveChanges(context);

            
            var applications = new List<Application>
            {
               new Application {  PostingID=(context.Postings.Where(p=>p.pstOpenDate == DateTime.Parse("2017-11-15")).SingleOrDefault().ID),  ApplicantID=(context.Applicants.Where(p=>p.apFirstName == "kevin").SingleOrDefault().ID)
            }
            };
            applications.ForEach(a => context.Applications.AddOrUpdate(n => n.Applicant, a));
            SaveChanges(context);
            
            var applicationsCart = new List<ApplicationCart>
            {
                new ApplicationCart {  PostingID=(context.Postings.Where(p=>p.pstOpenDate == DateTime.Parse("2017-11-15") ).SingleOrDefault().ID),  ApplicantID=(context.Applicants.Where(p=>p.apFirstName == "kevin").SingleOrDefault().ID), Priority = 1},
             
            };
            applicationsCart.ForEach(a => context.ApplicationCarts.AddOrUpdate(n => n.ID, a));
            SaveChanges(context);

            var interviewCart = new List<InterviewCart>
            {
                   new InterviewCart {  ApplicationID =(context.Applications.Where(p=>p.ID == 1).SingleOrDefault().ID), InterviewDate = DateTime.Parse("2018-11-15")}
            };
            interviewCart.ForEach(a => context.InterviewCarts.AddOrUpdate(n => n.ApplicationID, a));
            SaveChanges(context);


            var jobGroups = new List<JobGroup>
            {
                   new JobGroup {  GroupTitle = "Maintenance" },
                   new JobGroup {  GroupTitle = "Technical Support" },
                   new JobGroup {  GroupTitle = "Teacher" }
            };
            jobGroups.ForEach(a => context.JobGroup.AddOrUpdate(n => n.GroupTitle, a));
            SaveChanges(context);

            var Locations = new List<Location>
            {
                new Location { Province = "Ontario", Street = "13 fake lane",  CityID = (context.Cities.Where(p=>p.city == "St.Catharines").SingleOrDefault().ID) }
            };
            Locations.ForEach(a => context.Locations.AddOrUpdate(n => n.Province, a));
            SaveChanges(context);

            var positions = new List<Position>
            {
                new Position { UnionID = (context.Unions.Where(p=>p.UnionName == "Opseu 250").SingleOrDefault().ID),  JobGroupID=(context.JobGroup.Where(p=>p.GroupTitle == "Teacher").SingleOrDefault().ID),
                 PositionSalary = 10000, PositionDescription = "basically this is a english teaching job", PositionFTE = 1, PositionCode = "10330",
                 PositionCompensationType = 1, PositionDayofWork = "Monday-Friday"  
                }
            };
            positions.ForEach(a => context.Positions.AddOrUpdate(n => n.PositionCode, a));
            SaveChanges(context);

            var postings = new List<Posting>
            {
                new Posting { pstNumPosition = 4, pstOpenDate = DateTime.Parse("2017-11-15"), pstEndDate=DateTime.Parse("2019-11-15")
                , pstJobDescription = "this job will take all the skills of teaching and more as the Vice Principle you are required to" +
                "look into many different fields of the school...", PositionID=(context.Positions.Where(p=>p.PositionCode == "110089").SingleOrDefault().ID)}
            };
            postings.ForEach(a => context.Postings.AddOrUpdate(n => n.pstOpenDate, a));
            SaveChanges(context);

            var qualifications = new List<Qualification>
            {
                new Qualification { QlfDescription = "Hard Working"},
                new Qualification { QlfDescription = "Punctual"},
                new Qualification { QlfDescription = "French Language"},
                new Qualification { QlfDescription = "Self Mostivated"},
                new Qualification { QlfDescription = "High Initiative"},
                new Qualification { QlfDescription = "positive Attitude"}
            };
            qualifications.ForEach(a => context.Qualification.AddOrUpdate(n => n.QlfDescription, a));
            SaveChanges(context);

            var unions = new List<Union>
            {
                new Union { UnionName = "Opseu 270" },
                new Union { UnionName = "Opseu 250" },
                new Union { UnionName = "Opseu 260" },
                new Union { UnionName = "Opseu 290" },
                new Union { UnionName = "Opseu 271" }
            };
            unions.ForEach(a => context.Unions.AddOrUpdate(n => n.UnionName, a));
            SaveChanges(context);

        }
    }
}
