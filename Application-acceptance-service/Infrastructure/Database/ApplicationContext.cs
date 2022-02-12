using Application_acceptance_service.Domain;
using Microsoft.EntityFrameworkCore;

namespace Application_acceptance_service.Infrastructure.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<RequestedCredit> RequestedCredits { get; set; }

        public ApplicationContext() : base()
        {
        }
        
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>()
                .HasOne(a => a.Applicant)
                .WithMany(a => a.Applications)
                .HasForeignKey(a => a.ApplicantId);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.RequestedCredit)
                .WithMany(a => a.Applications)
                .HasForeignKey(a => a.RequestedCreditId);
        }
    }
}