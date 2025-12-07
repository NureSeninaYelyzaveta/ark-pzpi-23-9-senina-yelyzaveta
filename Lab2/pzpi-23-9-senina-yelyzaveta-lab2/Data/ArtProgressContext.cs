using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApplicationArtProgress1.Models;

namespace WebApplicationArtProgress1.Data
{
    public class ArtProgressContext : DbContext
    {
        public ArtProgressContext(DbContextOptions<ArtProgressContext> options) : base(options) { }

        // Таблиці бази даних
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Notification> Notifications { get; set; }


        // Проміжні таблиці
        public DbSet<ParentStudent> ParentStudents { get; set; }
        public DbSet<TeacherDiscipline> TeacherDisciplines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TeacherDiscipline (M:N) 
            modelBuilder.Entity<TeacherDiscipline>()
                .HasKey(td => new { td.TeacherID, td.DisciplineID }); // складений ключ

            modelBuilder.Entity<TeacherDiscipline>()
                .HasOne(td => td.Teacher)
                .WithMany(t => t.TeacherDisciplines)
                .HasForeignKey(td => td.TeacherID);

            modelBuilder.Entity<TeacherDiscipline>()
                .HasOne(td => td.Discipline)
                .WithMany(d => d.TeacherDisciplines)
                .HasForeignKey(td => td.DisciplineID);

            // ParentStudent (M:N) 
            modelBuilder.Entity<ParentStudent>()
                .HasKey(ps => new { ps.ParentID, ps.StudentID });   // складений ключ

            modelBuilder.Entity<ParentStudent>()
                .HasOne(ps => ps.Parent)
                .WithMany(p => p.ParentStudents)
                .HasForeignKey(ps => ps.ParentID);

            modelBuilder.Entity<ParentStudent>()
                .HasOne(ps => ps.Student)
                .WithMany(s => s.ParentStudents)
                .HasForeignKey(ps => ps.StudentID);

            // Grade (1:N) 
            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.StudentID);

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Teacher)
                .WithMany(t => t.Grades)
                .HasForeignKey(g => g.TeacherID);

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Discipline)
                .WithMany(d => d.Grades)
                .HasForeignKey(g => g.DisciplineID);

            // -Comment (1:N) 
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Student)
                .WithMany(s => s.Comments)
                .HasForeignKey(c => c.StudentID);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Teacher)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TeacherID);

            // Portfolio (1:1)
            modelBuilder.Entity<Portfolio>()
                .HasOne(p => p.Student)
                .WithOne(s => s.Portfolio)
                .HasForeignKey<Portfolio>(p => p.StudentID);

            // Notification (1:N) 
            modelBuilder.Entity<Notification>()
            .HasOne(n => n.Student)
            .WithMany(s => s.Notifications)
            .HasForeignKey(n => n.StudentID)
            .OnDelete(DeleteBehavior.Cascade); // видалити при видаленні студента


            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Parent)
                .WithMany(p => p.Notifications)
                .HasForeignKey(n => n.ParentID)
                .OnDelete(DeleteBehavior.Restrict);  // батька не видаляємо разом зі сповіщенням

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Teacher)
                .WithMany(t => t.Notifications)
                .HasForeignKey(n => n.TeacherID)
                .OnDelete(DeleteBehavior.Cascade); // видалити при видаленні вчителя

        }
    }
} 