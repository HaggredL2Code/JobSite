namespace JobPosting.DAL.JBMigrations
{
    using JobPosting.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;

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
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            var unions = new List<Union>
                 {
                     new Union { UnionName = "Opseu 270" },
                     new Union { UnionName = "Opseu 250" },
                     new Union { UnionName = "Opseu 260" },
                     new Union { UnionName = "Opseu 290" },
                     new Union { UnionName = "Opseu 271" },
                     new Union { UnionName = "Opseu 124" },
                     new Union { UnionName = "Opseu 120" },
                     new Union { UnionName = "Opseu 104" }
                 };
            unions.ForEach(a => context.Unions.AddOrUpdate(n => n.UnionName, a));
            SaveChanges(context);


            var jobGroups = new List<JobGroup>
                 {
                        new JobGroup {  GroupTitle = "Maintenance" },
                        new JobGroup {  GroupTitle = "Technical Support" },
                        new JobGroup {  GroupTitle = "Teacher" },
                        new JobGroup {  GroupTitle = "Security" },
                        new JobGroup {  GroupTitle = "Administration" }

                 };
            jobGroups.ForEach(a => context.JobGroups.AddOrUpdate(n => n.GroupTitle, a));
            SaveChanges(context);

            var userRoles = new List<UserRole>
            {
                new UserRole { RoleTitle = "Hiring Team" },
                new UserRole { RoleTitle = "Admin" },
                new UserRole { RoleTitle = "Manager" },
                new UserRole { RoleTitle = "User" }
            };
            userRoles.ForEach(a => context.UserRoles.AddOrUpdate(n => n.RoleTitle, a));
            SaveChanges(context);

            var qualifications = new List<Qualification>
                        {
                            new Qualification { QlfDescription = "WHIMIS teaching Certificate"},
                            new Qualification { QlfDescription = "English Language 2017 Certificate"},
                            new Qualification { QlfDescription = "French Language 2018 Certificate"},
                            new Qualification { QlfDescription = "Welding teaching Certificate"},
                            new Qualification { QlfDescription = "Fist Aid Certificate"},
                            new Qualification { QlfDescription = "Teaching Certification 2304"}
                        };
            qualifications.ForEach(a => context.Qualification.AddOrUpdate(n => n.QlfDescription, a));
            SaveChanges(context);



            /*
                        var applicants = new List<Applicant>
                        {
                            new Applicant { apFirstName = "Kevin", apMiddleName = "m", apLastName = "Marty", apPhone = 9056677777, apPostalCode = "L2S5G6", apEMail = "testuser@hotmail.com", apAddress = "13 fake lane" , apSubscripted = false, UserRoleID = (context.UserRoles.Where(p=>p.RoleTitle == "User").SingleOrDefault().ID) },
                            new Applicant { apFirstName = "Bob", apMiddleName = "m", apLastName = "Doom", apPhone = 9056675555, apPostalCode = "L2S5G7", apEMail = "testuser1@hotmail.com", apAddress = "13 fake lane" , apSubscripted = false, UserRoleID = (context.UserRoles.Where(p=>p.RoleTitle == "User").SingleOrDefault().ID) },
                            new Applicant { apFirstName = "Robert", apMiddleName = "D", apLastName = "McKnight", apPhone = 9056676666, apPostalCode = "L2S5G8", apEMail = "testuser2@hotmail.com", apAddress = "13 placeholder street" , apSubscripted = false, UserRoleID = (context.UserRoles.Where(p=>p.RoleTitle == "User").SingleOrDefault().ID) },
                            new Applicant { apFirstName = "Joe", apMiddleName = "D", apLastName = "Master", apPhone = 9056676556, apPostalCode = "L2S5G2", apEMail = "testuser3@hotmail.com", apAddress = "13 placeholder lane" , apSubscripted = false, UserRoleID = (context.UserRoles.Where(p=>p.RoleTitle == "User").SingleOrDefault().ID) },
                            new Applicant { apFirstName = "Monkey", apMiddleName = "D", apLastName = "Luffy", apPhone = 9056676555, apPostalCode = "L2S5G3", apEMail = "testuser4@hotmail.com", apAddress = "1000 sunny lane" , apSubscripted = false, UserRoleID = (context.UserRoles.Where(p=>p.RoleTitle == "User").SingleOrDefault().ID) },
                            new Applicant { apFirstName = "Traflagar", apMiddleName = "D", apLastName = "Law", apPhone = 9056676555, apPostalCode = "L2S5G3", apEMail = "testuser4@hotmail.com", apAddress = "1000 sunny lane" , apSubscripted = false, UserRoleID = (context.UserRoles.Where(p=>p.RoleTitle == "User").SingleOrDefault().ID) }


                        };
                        applicants.ForEach(a => context.Applicants.AddOrUpdate(n => n.apEMail, a));
                        SaveChanges(context);

                */
            var days = new HashSet<DayOfWeek>
            {
                DayOfWeek.Monday,
                DayOfWeek.Friday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday
            };

            var positions = new List<Position>
                        {
                            new Position { UnionID = (context.Unions.Where(p=>p.UnionName == "Opseu 250").SingleOrDefault().ID),  JobGroupID=(context.JobGroups.Where(p=>p.GroupTitle == "Teacher").SingleOrDefault().ID),
                            PositionDescription = "English Teacher", PositionCode = "10330",
                        },

                            new Position { UnionID = (context.Unions.Where(p=>p.UnionName == "Opseu 270").SingleOrDefault().ID),  JobGroupID=(context.JobGroups.Where(p=>p.GroupTitle == "Technical Support").SingleOrDefault().ID),
                            PositionDescription = "IT Admin", PositionCode = "10331"
                        },

                            new Position { UnionID = (context.Unions.Where(p=>p.UnionName == "Opseu 104").SingleOrDefault().ID),  JobGroupID=(context.JobGroups.Where(p=>p.GroupTitle == "Technical Support").SingleOrDefault().ID),
                            PositionDescription = "IT Junior", PositionCode = "10336"
                        },
                            new Position { UnionID = (context.Unions.Where(p=>p.UnionName == "Opseu 124").SingleOrDefault().ID),  JobGroupID=(context.JobGroups.Where(p=>p.GroupTitle == "Teacher").SingleOrDefault().ID),
                            PositionDescription = "French Teacher", PositionCode = "10335"
                        },
                            new Position { UnionID = (context.Unions.Where(p=>p.UnionName == "Opseu 271").SingleOrDefault().ID),  JobGroupID=(context.JobGroups.Where(p=>p.GroupTitle == "Teacher").SingleOrDefault().ID),
                            PositionDescription = "Gym Teacher", PositionCode = "10339"
                        },
                              new Position { UnionID = (context.Unions.Where(p=>p.UnionName == "Opseu 271").SingleOrDefault().ID),  JobGroupID=(context.JobGroups.Where(p=>p.GroupTitle == "Administration").SingleOrDefault().ID),
                            PositionDescription = "Vice Principle", PositionCode = "10340"
                        },
                               new Position { UnionID = (context.Unions.Where(p=>p.UnionName == "Opseu 271").SingleOrDefault().ID),  JobGroupID=(context.JobGroups.Where(p=>p.GroupTitle == "Administration").SingleOrDefault().ID),
                            PositionDescription = "Secretary", PositionCode = "10401"
                        }



                        };
            positions.ForEach(a => context.Positions.AddOrUpdate(n => n.PositionCode, a));
            SaveChanges(context);

            var postings = new List<Posting>
                        {
                             new Posting { pstNumPosition = 1, pstEndDate = DateTime.Parse("2019-11-15"), pstJobStartDate = DateTime.Parse("2019-11-15"), pstJobEndDate = DateTime.Parse("2021-11-1")
                           , pstJobDescription = "This is a job description for a Vice Principle", PositionID=6, pstFTE = 1.0m, pstSalary = 20.00m, pstCompensationType = 1,
                             Days = new List<Day> {
                               new Day {
                                    dayName = "Monday",
                                    dayOrder = 1
                               },
                               new Day {
                                    dayName = "Tuesday",
                                    dayOrder = 2
                               },
                               new Day {
                                    dayName = "Wednesday",
                                    dayOrder = 3
                               },
                               new Day {
                                    dayName = "Thursday",
                                    dayOrder = 4
                               },
                               new Day {
                                    dayName = "Friday",
                                    dayOrder = 5
                               },
                               new Day {
                                    dayName = "Saturday",
                                    dayOrder = 6
                               },
                               new Day {
                                    dayName = "Sunday",
                                    dayOrder = 7
                               },
                           } },
                              new Posting { pstNumPosition = 2, pstEndDate = DateTime.Parse("2019-11-16"), pstJobStartDate = DateTime.Parse("2019-11-16"), pstJobEndDate = DateTime.Parse("2021-11-1")
                           , pstJobDescription = "This is a job description for a french teacher job", PositionID=4, pstFTE = 0.8m, pstSalary = 18.00m, pstCompensationType = 1,},
                             new Posting { pstNumPosition = 4, pstEndDate = DateTime.Parse("2019-11-17"), pstJobStartDate = DateTime.Parse("2019-11-17"), pstJobEndDate = DateTime.Parse("2021-11-1")
                           , pstJobDescription = "This is a job description for a Secretary", PositionID=7, pstFTE = 0.6m, pstSalary = 14.00m, pstCompensationType = 1},
                              new Posting { pstNumPosition = 1, pstEndDate = DateTime.Parse("2019-11-17"), pstJobStartDate = DateTime.Parse("2019-11-18"), pstJobEndDate = DateTime.Parse("2021-11-1")
                           , pstJobDescription = "This is a job description for a Vice Principle", PositionID=6, pstFTE = 0.6m, pstSalary = 25.00m, pstCompensationType = 1 }


                        };
            postings.ForEach(a => context.Postings.AddOrUpdate(n => n.ID, a));
            SaveChanges(context);

            /*
            var applications = new List<Application>
                                    {

                                       new Application { PostingID = 1,  ApplicantID=1, Priority = 2},
                                       new Application { PostingID = 1,  ApplicantID=2, Priority = 1},
                                       new Application { PostingID = 1,  ApplicantID=3, Priority = 3}

                                    };
            applications.ForEach(a => context.Applications.AddOrUpdate(n => new { n.ApplicantID, n.PostingID }, a));
            SaveChanges(context);
    */
            var Locations = new List<Location>
                        {
                            new Location { Address = "Alexander Kuska, Welland"},
                            new Location { Address = "Assumption, St. Catharines"},
                            new Location { Address = "Canadian Martyrs, St. Catharines"},
                            new Location { Address = "Holy Name, Welland"},
                            new Location { Address = "Loretto Catholic, Niagara Falls"},
                            new Location { Address = "St. Augustine, Welland"},
                            new Location { Address = "St. Christopher, St. Catharines"},
                            new Location { Address = "St. Denis, St. Catharines"},
                            new Location { Address = "St. Andrew, Welland"},
                            new Location { Address = "St. Gabriel Lalemant, Niagara Falls"},
                            new Location { Address = "St. Mary, Welland"},
                            new Location { Address = "St. Peter, St. Catharines"},
                            new Location { Address = "St. Theresa, St. Catharines"},
                            new Location { Address = "St. Therese, Port Colborne"},
                            new Location { Address = "St. Vincent de Paul, Niagara Falls"}
                        };
            Locations.ForEach(a => context.Locations.AddOrUpdate(n => n.ID, a));
            SaveChanges(context);

            var Skills = new List<Skill>
            {
                new Skill { SkillDescription = "C#"},
                new Skill { SkillDescription = "JQuery"},
                new Skill { SkillDescription = "C++"},
                new Skill { SkillDescription = "Hard Working"},
                new Skill { SkillDescription = "Punctual"},
                new Skill { SkillDescription = "Detail Oriented"},
                new Skill { SkillDescription = "Welding"},
                new Skill { SkillDescription = "Carpentry"}
            };
            Skills.ForEach(a => context.Skills.AddOrUpdate(n => n.ID, a));
            SaveChanges(context);


            //var jobRequirements = new List<JobRequirement>
            //            {
            //                new JobRequirement { PostingID = 1, QualificationID = (context.Qualification.Where(p=>p.QlfDescription == "Hard Working").SingleOrDefault().ID)  },
            //                new JobRequirement { PostingID = 1, QualificationID = (context.Qualification.Where(p=>p.QlfDescription == "Punctual").SingleOrDefault().ID)  },
            //                new JobRequirement { PostingID = 1, QualificationID = (context.Qualification.Where(p=>p.QlfDescription == "French Language").SingleOrDefault().ID)  },
            //                new JobRequirement { PostingID = 1, QualificationID = (context.Qualification.Where(p=>p.QlfDescription == "Hard Working").SingleOrDefault().ID)  }

            //            };
            //jobRequirements.ForEach(a => context.JobRequirements.AddOrUpdate(n => new { n.PostingID, n.QualificationID }, a));
            //SaveChanges(context);

            var jobRequirements = new List<JobRequirement>
            {
                new JobRequirement { PostingID = 1, QualificationID = 1},
                new JobRequirement { PostingID = 1, QualificationID = 2},
                new JobRequirement { PostingID = 1, QualificationID = 3},
                new JobRequirement { PostingID = 2, QualificationID = 6},
                new JobRequirement { PostingID = 2, QualificationID = 5},
                new JobRequirement { PostingID = 3, QualificationID = 1},
                new JobRequirement { PostingID = 3, QualificationID = 3},
                new JobRequirement { PostingID = 4, QualificationID = 4},
                new JobRequirement { PostingID = 4, QualificationID = 5}
            };
            jobRequirements.ForEach(a => context.JobRequirements.AddOrUpdate(n => new { n.PostingID, n.QualificationID }, a));
            SaveChanges(context);

            var jobLocations = new List<JobLocation>
                        {
                            new JobLocation { PostingID = 1, LocationID = 2},
                            new JobLocation { PostingID = 1, LocationID = 5},
                            new JobLocation { PostingID = 2, LocationID = 2},
                            new JobLocation { PostingID = 3, LocationID = 4},
                            new JobLocation { PostingID = 4, LocationID = 3}
                        };
            jobLocations.ForEach(a => context.JobLocations.AddOrUpdate(n => new { n.PostingID, n.LocationID }, a));
            SaveChanges(context);


            /*
            var applicationQualifications = new List<ApplicationQualification>
                   {
                       new ApplicationQualification { ApplicationID = (context.Applications.Where(p=>p.ID == 1).SingleOrDefault().ID), QualificationID = (context.Qualification.Where(p=>p.QlfDescription == "Hard Working").SingleOrDefault().ID)}
                   };
            applicationQualifications.ForEach(a => context.ApplicationQualification.AddOrUpdate(n => new { n.ApplicationID, n.QualificationID }, a));
            SaveChanges(context);
            */
            var postingSkills = new List<PostingSkill>
            {
                new PostingSkill { PostingID = 1, SkillID = 1},
                new PostingSkill { PostingID = 1, SkillID = 2},
                new PostingSkill { PostingID = 1, SkillID = 3},
                new PostingSkill { PostingID = 2, SkillID = 2},
                new PostingSkill { PostingID = 2, SkillID = 3},
                new PostingSkill { PostingID = 2, SkillID = 4},
                new PostingSkill { PostingID = 3, SkillID = 2},
                new PostingSkill { PostingID = 4, SkillID = 3},

            };
            postingSkills.ForEach(a => context.PostingSkills.AddOrUpdate(n => new { n.PostingID, n.SkillID }, a));
            SaveChanges(context);
            /*
                        var applicationSkills = new List<ApplicationSkill>
                        {
                            new ApplicationSkill { ApplicationId = 1, skillID = 2}
                        };
                        applicationSkills.ForEach(a => context.ApplicationSkills.AddOrUpdate(n => new { n.ApplicationId, n.skillID }, a));
                        SaveChanges(context);
                        */

            //var postingTemplate = new List<PostingTemplate>
            //{

            //    new PostingTemplate { RequirementIDs = "1,2,3"  }
            //};

            //var applicationsCart = new List<ApplicationCart>
            //            {
            //                new ApplicationCart { PostingID = 1,  ApplicantID=(context.Applicants.Where(p=>p.apEMail == "testuser@hotmail.com").SingleOrDefault().ID), Priority = 2},

            //            };
            //applicationsCart.ForEach(a => context.ApplicationCarts.AddOrUpdate(n => n.ID, a));
            //SaveChanges(context);
        }
    }
}
