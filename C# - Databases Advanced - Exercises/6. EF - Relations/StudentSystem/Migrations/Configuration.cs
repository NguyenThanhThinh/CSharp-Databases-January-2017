namespace StudentSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<StudentsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "StudentSystem.StudentsContext";
        }

        protected override void Seed(StudentsContext context)
        {

            Student student = new Student()
            {
                Name = "Dragan",
                RegistrationDate = new DateTime(2017, 03, 07),
                Birthday = new DateTime(1992, 01, 08),
                PhoneNumber = "0888 123 456"
            };

            Student student2 = new Student()
            {
                Name = "Stamen",
                RegistrationDate = new DateTime(2017, 03, 07),
                Birthday = new DateTime(1992, 01, 08),
                PhoneNumber = "0888 456 789"
            };

            Course course = new Course()
            {
                Name = "C#",
                Description = "C# stuff",
                StartDate = new DateTime(2017, 01, 16),
                EndDate = new DateTime(2017, 03, 16),
                Price = 200m
            };

            Course course2 = new Course()
            {
                Name = "Java",
                Description = "Java stuff",
                StartDate = new DateTime(2017, 01, 16),
                EndDate = new DateTime(2017, 03, 16),
                Price = 200m
            };
           
            Resource resource = new Resource()
            {
                Name = "C# Book",
                Type = TypeOfResource.Document,
                URL = "www.CSharp.com"
            };

            Resource resource2 = new Resource()
            {
                Name = "Java Book",
                Type = TypeOfResource.Document,
                URL = "www.Java.com"
            };
            
            Homework homework = new Homework()
            {
                Content = "C# homework",
                ContentType = ContentType.Zip,
                SubmissionDate = new DateTime(2017, 02, 01)
            };

            Homework homework2 = new Homework()
            {
                Content = "Java homework",
                ContentType = ContentType.Zip,
                SubmissionDate = new DateTime(2017, 02, 01)
            };

            
            student.Homeworks.Add(homework);
            student2.Homeworks.Add(homework2);

            student.Courses.Add(course);
            student2.Courses.Add(course2);

            course.Homeworks.Add(homework);
            course2.Homeworks.Add(homework2);

            course.Resources.Add(resource);
            course2.Resources.Add(resource2);

            context.Students.AddOrUpdate(s => s.Name, student, student2);
            context.Courses.AddOrUpdate(c => c.Name, course, course2);
            context.Resources.AddOrUpdate(r => r.Name, resource, resource2);
            context.Homeworks.AddOrUpdate(h => h.Content, homework, homework2);
        }
    }
}
