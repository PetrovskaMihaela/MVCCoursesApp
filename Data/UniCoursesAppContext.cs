using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniCoursesApp.Models;

namespace UniCoursesApp.Models
{
    public class UniCoursesAppContext : DbContext
    {
        public UniCoursesAppContext (DbContextOptions<UniCoursesAppContext> options)
            : base(options)
        {
        }

        public DbSet<UniCoursesApp.Models.Student> Student { get; set; }

        public DbSet<UniCoursesApp.Models.Teacher> Teacher { get; set; }

        public DbSet<UniCoursesApp.Models.Course> Course { get; set; }

        public DbSet<UniCoursesApp.Models.Enrollment> Enrollment { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Enrollment>()
                .HasOne<Course>(p => p.Course)
                .WithMany(p => p.Students)
                .HasForeignKey(p => p.CourseId);

            builder.Entity<Enrollment>()
                .HasOne<Student>(p => p.Student)
                .WithMany(p => p.Courses)
                .HasForeignKey(p => p.StudentId);

            builder.Entity<Course>()
                .HasOne(p => p.FirstTeacher)
                .WithMany(p => p.Courses)
                .HasForeignKey(p => p.FirstTeacherId);

            builder.Entity<Course>()
                .HasOne(p => p.SecondTeacher)
                .WithMany(p => p.CoursesSecond)
                .HasForeignKey(p => p.SecondTeacherId);

        }
    }
}
