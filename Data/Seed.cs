using ThisLinq.Models;

namespace ThisLinq.Data
{
    public class Seed
    {
        public static async void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<Linq22DbContext>();

                context.Database.EnsureCreated();

                if (!context.Classes.Any())
                {
                    context.Classes.AddRange(new List<Class>()
                    {
                        new Class()
                        {
                            ClassName = "A01"
                        },
                        new Class()
                        {
                            ClassName = "A02"
                        },
                        new Class()
                        {
                            ClassName = "A03"
                        },
                    });

                    context.SaveChanges();
                }

                if (!context.Courses.Any())
                {
                    context.Courses.AddRange(new List<Course>()
                    {
                        new Course()
                        {
                            CourseName = "Programmering 1"
                        },
                        new Course()
                        {
                            CourseName = "Webbutveckling 1"
                        },
                        new Course()
                        {
                            CourseName = "Företagsekonomi"
                        },
                        new Course()
                        {
                            CourseName = "Programmering 2"
                        },
                        new Course()
                        {
                            CourseName = "Webbutveckling 2"
                        },
                    });

                    context.SaveChanges();
                }

                if (!context.Students.Any())
                {
                    context.Students.AddRange(new List<Student>()
                    {
                        new Student()
                        {
                            FirstName = "Isabella",
                            LastName = "Andersson"
                        },
                        new Student()
                        {
                            FirstName = "Alexander",
                            LastName = "Johansson"
                        },
                        new Student()
                        {
                            FirstName = "Sofia",
                            LastName = "Karlsson"
                        },
                        new Student()
                        {
                            FirstName = "Oskar",
                            LastName = "Eriksson"
                        },
                        new Student()
                        {
                            FirstName = "Lucas",
                            LastName = "Nilsson"
                        },
                        new Student()
                        {
                            FirstName = "Madde",
                            LastName = "Stålhand"
                        }
                    });
                    context.SaveChanges();
                }

                if (!context.Teachers.Any())
                {
                    context.Teachers.AddRange(new List<Teacher>()
                    {
                        new Teacher()
                        {
                            FirstName = "Anders",
                            LastName = "Anderson"
                        },
                        new Teacher()
                        {
                            FirstName = "Tobias",
                            LastName = "Landén"
                        },
                        new Teacher()
                        {
                            FirstName = "Reidar",
                            LastName = "Nilsen"
                        }
                    });
                    context.SaveChanges();
                }

                if (!context.Connections.Any())
                {
                    context.Connections.AddRange(new List<ConnectionLink>()
                    {
                        new ConnectionLink()
                        {
                            FK_StudentId = 1,
                            FK_CourseId = 1,
                            FK_TeacherId = 1,
                            FK_ClassId = 1
                        },
                        new ConnectionLink()
                        {
                            FK_StudentId = 1,
                            FK_CourseId = 2,
                            FK_TeacherId = 2,
                            FK_ClassId = 1
                        },
                        new ConnectionLink()
                        {
                            FK_StudentId = 2,
                            FK_CourseId = 3,
                            FK_TeacherId = 3,
                            FK_ClassId = 3
                        },
                        new ConnectionLink()
                        {
                            FK_StudentId = 3,
                            FK_CourseId = 4,
                            FK_TeacherId = 1,
                            FK_ClassId = 2
                        },
                        new ConnectionLink()
                        {
                            FK_StudentId = 3,
                            FK_CourseId = 5,
                            FK_TeacherId = 2,
                            FK_ClassId = 2
                        },
                        new ConnectionLink()
                        {
                            FK_StudentId = 2,
                            FK_CourseId = 1,
                            FK_TeacherId = 1,
                            FK_ClassId = 1
                        },
                        new ConnectionLink()
                        {
                            FK_StudentId = 4,
                            FK_CourseId = 4,
                            FK_TeacherId = 1,
                            FK_ClassId = 2
                        },
                        new ConnectionLink()
                        {
                            FK_StudentId = 4,
                            FK_CourseId = 5,
                            FK_TeacherId = 2,
                            FK_ClassId = 2
                        },
                        new ConnectionLink()
                        {
                            FK_StudentId = 5,
                            FK_CourseId = 3,
                            FK_TeacherId = 3,
                            FK_ClassId = 3
                        },
                        new ConnectionLink()
                        {
                            FK_StudentId = 6,
                            FK_CourseId = 3,
                            FK_TeacherId = 3,
                            FK_ClassId = 3
                        },
                        new ConnectionLink()
                        {
                            FK_StudentId = 5,
                            FK_CourseId = 4,
                            FK_TeacherId = 1,
                            FK_ClassId = 2
                        },
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}

