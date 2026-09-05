using Microsoft.EntityFrameworkCore;
using Backend.Entities;

namespace Backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {}
    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Opening> Openings { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<OtpVerification> OtpVerifications { get; set; }
    public DbSet<PlacementSettings> PlacementSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.RollNo)
            .IsUnique();

        modelBuilder.Entity<Company>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder.Entity<Application>()
            .HasIndex(a => new { a.StudentId, a.OpeningId })
            .IsUnique();

        modelBuilder.Entity<OtpVerification>()
            .HasIndex(o => o.Email)
            .IsUnique();
        
        modelBuilder.Entity<Student>()
            .HasIndex(s => s.PhoneNumber)
            .IsUnique()
            .HasFilter("[PhoneNumber] IS NOT NULL");
    }
}