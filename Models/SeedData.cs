using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniCoursesApp.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {

            using (var context = new UniCoursesAppContext(
                serviceProvider.GetRequiredService<
                DbContextOptions<UniCoursesAppContext>>()))
            {

                if (context.Teacher.Any() || context.Course.Any() || context.Student.Any())
                {
                    return; // DB has been seeded
                }
                context.Teacher.AddRange(
                    new Teacher { /*Id = 1,*/ FirstName = "Перо", LastName = "Латкоски", Degree = "Доктор на науки", AcademicRank = "Редовен професор", HireDate = DateTime.Parse("2000-3-6"), OfficeNumber = "ТК" },
                    new Teacher { /*Id = 2,*/ FirstName = "Даниел", LastName = "Денковски", Degree = "Доктор на науки", AcademicRank = "Доцент", HireDate = DateTime.Parse("2000-3-6"), OfficeNumber = "121" },
                    new Teacher { /*Id = 2,*/ FirstName = "Владимир", LastName = "Атанасовски", Degree = "Доктор на науки", AcademicRank = "Редовен професор", HireDate = DateTime.Parse("2000-3-6"), OfficeNumber = "Деканат" }
                );
                context.SaveChanges();

                context.Course.AddRange(
                    new Course { Title = "Развој на серверски ВЕБ апликации", Credits = 6, EducationLevel = "Додипломски", Programme = "КТИ", Semester = 6, FirstTeacherId = context.Teacher.Single(t => t.FirstName == "Даниел" && t.LastName == "Денковски").Id, SecondTeacherId = context.Teacher.Single(t => t.FirstName == "Перо" && t.LastName == "Латкоски").Id },
                    new Course { Title = "Мобилни сервиси со АП", Credits = 6, EducationLevel = "Додипломски", Programme = "ТКИИ", Semester = 6, FirstTeacherId = context.Teacher.Single(t => t.FirstName == "Даниел" && t.LastName == "Денковски").Id, SecondTeacherId = context.Teacher.Single(t => t.FirstName == "Перо" && t.LastName == "Латкоски").Id },
                    new Course { Title = "Андроид Програмирање", Credits = 6, EducationLevel = "Додипломски", Programme = "ТКИИ", Semester = 5, FirstTeacherId = context.Teacher.Single(t => t.FirstName == "Даниел" && t.LastName == "Денковски").Id, SecondTeacherId = context.Teacher.Single(t => t.FirstName == "Владимир" && t.LastName == "Атанасовски").Id }
                    );
                context.SaveChanges();

                context.Student.AddRange(
                    new Student { FirstName = "Михаела", LastName = "Петровска", AcquiredCredits = 150, CurrentSemester = 6, EducationLevel = "додипломски", StudentId = "1/2018", EnrollmentDate = DateTime.Parse("2000-3-6") },
                    new Student { FirstName = "Мартина", LastName = "Јовановиќ", AcquiredCredits = 150, CurrentSemester = 6, EducationLevel = "додипломски", StudentId = "2/2018", EnrollmentDate = DateTime.Parse("2000-3-6") },
                    new Student { FirstName = "Надежда", LastName = "Илиева", AcquiredCredits = 150, CurrentSemester = 6, EducationLevel = "додипломски", StudentId = "3/2018", EnrollmentDate = DateTime.Parse("2000-3-6") }

                    );
                context.SaveChanges();

                context.Enrollment.AddRange(
                    new Enrollment { StudentId = 1, CourseId = 2 },
                    new Enrollment { StudentId = 2, CourseId = 2 },
                    new Enrollment { StudentId = 3, CourseId = 1 },
                    new Enrollment { StudentId = 1, CourseId = 1 },
                    new Enrollment { StudentId = 3, CourseId = 3 },
                    new Enrollment { StudentId = 1, CourseId = 3 },
                    new Enrollment { StudentId = 2, CourseId = 3 }
                    );
                context.SaveChanges();

            }
        }
    }
}
