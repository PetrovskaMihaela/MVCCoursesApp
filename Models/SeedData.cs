using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCoursesApp.Areas.Identity.Data;

namespace UniCoursesApp.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<UniCoursesAppUser>>();
            IdentityResult roleResult;

            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }

            UniCoursesAppUser user = await UserManager.FindByEmailAsync("admin@coursesapp.com");
            if (user == null)
            {
                var User = new UniCoursesAppUser();
                User.Email = "admin@coursesapp.com";
                User.UserName = "admin@coursesapp.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }

            roleCheck = await RoleManager.RoleExistsAsync("Teacher");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Teacher"));
            }

            user = await UserManager.FindByEmailAsync("danield@coursesapp.com");
            if (user == null)
            {
                var User = new UniCoursesAppUser();
                User.Email = "danield@coursesapp.com";
                User.UserName = "danield@coursesapp.com";
                User.TeacherId = 1;
                string userPWD = "Daniel123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Teacher"); }
            }

            roleCheck = await RoleManager.RoleExistsAsync("Student");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Student"));
            }
           
        }
        public static void Initialize(IServiceProvider serviceProvider)
        {

            using (var context = new UniCoursesAppContext(
                serviceProvider.GetRequiredService<
                DbContextOptions<UniCoursesAppContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();

                if (context.Teacher.Any() || context.Course.Any() || context.Student.Any())
                {
                    return; // DB has been seeded
                }
                context.Teacher.AddRange(
                    new Teacher { /*Id = 1,*/ FirstName = "Даниел", LastName = "Денковски", Degree = "Доктор на науки", AcademicRank = "Доцент", HireDate = DateTime.Parse("2000-3-6"), OfficeNumber = "121" },
                    new Teacher { /*Id = 2,*/ FirstName = "Перо", LastName = "Латкоски", Degree = "Доктор на науки", AcademicRank = "Редовен професор", HireDate = DateTime.Parse("2000-3-6"), OfficeNumber = "ТК" },
                    new Teacher { /*Id = 3,*/ FirstName = "Владимир", LastName = "Атанасовски", Degree = "Доктор на науки", AcademicRank = "Редовен професор", HireDate = DateTime.Parse("2000-3-6"), OfficeNumber = "Деканат" }
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
