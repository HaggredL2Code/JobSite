using JobPosting.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace JobPosting.DAL
{
    public class JBEntities : DbContext
    {
        public JBEntities() : base("name = JBEntities")
        {
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationCart> ApplicationCarts { get; set; }
        public DbSet<ApplicationQualification> ApplicationQualification { get; set; }
        public DbSet<Archive> Archives { get; set; }      
       public DbSet<Position> Positions { get; set; }
        public DbSet<JobGroup> JobGroups { get; set; }
        public DbSet<JobLocation> JobLocations { get; set; }
        public DbSet<JobRequirement> JobRequirements { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Posting> Postings { get; set; }
        public DbSet<Qualification> Qualification { get; set; }
        public DbSet<Union> Unions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Day> Days { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<JobGroup>()
                .HasMany(j => j.Positions)
                .WithRequired(p => p.JobGroup)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.JobLocations)
                .WithRequired(p => p.Location)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Position>()
                .HasMany(po => po.Postings)
                .WithRequired(p => p.Position)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Position>()
                .HasMany(po => po.JobRequirements)
                .WithRequired(p => p.Position)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Position>()
                .HasMany(po => po.JobLocations)
                .WithRequired(p => p.Position)
                .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Posting>()
            //    .HasMany(pos => pos.Applications)
            //    .WithRequired(p => p.Posting)
            //    .WillCascadeOnDelete(true);

            modelBuilder.Entity<Applicant>()
                .HasMany(a => a.Applications)
                .WithRequired(p => p.Applicant)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Application>()
                .HasMany(a => a.aFiles)
                .WithRequired(p => p.Application)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Application>()
                .HasMany(a => a.ApplicationsQualifications)
                .WithRequired(p => p.Application)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<aFile>()
                .HasOptional(w => w.FileContent)
                .WithRequired(p => p.aFile)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ApplicationCart>()
                .HasMany(a => a.aFileTemps)
                .WithRequired(p => p.ApplicationCart)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<aFileTemp>()
                .HasOptional(w => w.FileContentTemp)
                .WithRequired(p => p.aFileTemp)
                .WillCascadeOnDelete(true);
        }

        public override int SaveChanges()
        {
            //Get Audit Values if not supplied
            string auditUser = "Anonymous";
            try //Need to try becuase HttpContext might not exist
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                    auditUser = HttpContext.Current.User.Identity.Name;
            }
            catch (Exception)
            { }

            DateTime auditDate = DateTime.UtcNow;
            foreach (DbEntityEntry<IAuditable> entry in ChangeTracker.Entries<IAuditable>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = auditDate;
                    entry.Entity.CreatedBy = auditUser;
                    entry.Entity.UpdatedOn = auditDate;
                    entry.Entity.UpdatedBy = auditUser;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedOn = auditDate;
                    entry.Entity.UpdatedBy = auditUser;
                }
            }
            return base.SaveChanges();
        }
    }
}