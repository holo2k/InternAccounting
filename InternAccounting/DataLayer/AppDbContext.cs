using InternAccounting.BusinessLayer.DataTransferObjects.Intern;
using InternAccounting.DataLayer;
using InternAccounting.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InternAccounting.DAL
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public DbSet<InternEntity> Interns { get; private set; }
        public DbSet<DirectionEntity> Directions { get; private set; }
        public DbSet<ProjectEntity> Projects { get; private set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InternEntity>()
                .Property(e => e.Sex)
                .HasConversion<string>()
                .HasColumnType("varchar(10)");

            var sexConverter = new ValueConverter<Gender, string>(
                s => s.ToString(),
                s => (Gender)Enum.Parse(typeof(Gender),s));

            var emailConverter = new ValueConverter<Email, string>(
                email => email.ToString(), 
                value => new Email(value));

            modelBuilder.Entity<InternEntity>()
                .Property(u => u.Email)
                .HasConversion(emailConverter)
                .HasColumnType("TEXT");

            var phoneConverter = new ValueConverter<PhoneNumber, string>(
                phone => phone.ToString(),   
                value => new PhoneNumber(value));

            modelBuilder.Entity<InternEntity>()
                .Property(u => u.PhoneNumber)
                .HasConversion(phoneConverter)
                .HasColumnType("TEXT");

            modelBuilder.Entity<InternEntity>()
                .HasOne(i => i.Direction)
                .WithMany(d => d.Interns)
                .HasForeignKey(i => i.DirectionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InternEntity>()
                .HasOne(i => i.Project)
                .WithMany(p => p.Interns)
                .HasForeignKey(i => i.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
