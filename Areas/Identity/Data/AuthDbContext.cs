using AuthSystem.Areas.Identity.Data;
using AuthSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Data
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        public DbSet<CourseGrade> CourseGrades { get; set; }
        public DbSet<Teacher> Teachers { get; set; } // Teacher sınıfını DbContext’e ekledik

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ApplicationUser için tablo ve sütun isimlerini tanımlıyoruz
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("AspNetUsers"); // Tablo adı
                entity.Property(e => e.Id).HasColumnName("Id"); // Sütun adı
                entity.Property(e => e.FirstName).HasColumnName("FirstName"); // Sütun adı
                entity.Property(e => e.LastName).HasColumnName("LastName"); // Sütun adı
                entity.Property(e => e.Role).HasColumnName("Role"); // Sütun adı
                // Diğer sütun tanımlarını ekleyebilirsiniz
            });

            // ApplicationUser için null kontrolleri
            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.FirstName)
                .IsRequired()
                .HasDefaultValue("");

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.LastName)
                .IsRequired()
                .HasDefaultValue("");

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.Role)
                .IsRequired()
                .HasDefaultValue("Student");

            // CourseGrade için null kontrolleri
            modelBuilder.Entity<CourseGrade>()
                .Property(g => g.Grade)
                .IsRequired();

            modelBuilder.Entity<CourseGrade>()
                .Property(g => g.CourseName)
                .IsRequired();

            modelBuilder.Entity<CourseGrade>()
                .Property(g => g.FirstName)
                .IsRequired();

            modelBuilder.Entity<CourseGrade>()
                .Property(g => g.TeachersName)
                .IsRequired();

            // CourseGrade ile User arasındaki ilişki
            modelBuilder.Entity<CourseGrade>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
