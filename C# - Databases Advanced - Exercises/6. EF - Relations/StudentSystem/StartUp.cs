namespace StudentSystem
{
    using System;
    using System.Data.Entity.SqlServer;
    using System.Linq;
    
    class StartUp
    {
        static void Main(string[] args)
        {
            // Task 1 - The database and the models are created;
            // Task 2 - The inserted sample data is in the Seed method in Migrations.Configuration;
            // Task 4 - Added migration 'AddedLicenseModel' for the creation of the License model and foreign key with the Resource model;

            StudentsContext context = new StudentsContext();

            //Task 3.1:
            //ListAllStudentsAndHomeworkSubmission(context);

            //Task 3.2:
            //ListAllCoursesWithTheirResources(context);

            //Task 3.3:
            //AllCoursesWithMoreThanFiveResources(context);

            //Task 3.4:
            //AllCoursesActiveOnGivenDate(context);

            //Task 3.5:
            //AllCoursesOfAllStudent(context);
        }

        private static void ListAllStudentsAndHomeworkSubmission(StudentsContext context)
        {
            var students = context.Students.Select(student => new { student.Name, student.Homeworks });

            foreach (var student in students)
            {
                Console.WriteLine($"Student: {student.Name}");
                Console.WriteLine("Student homeworks: ");
                foreach (var homework in student.Homeworks)
                {
                    Console.WriteLine($"--{homework.Content} of Type: {homework.ContentType}");
                }

                Console.WriteLine();
            }
        }

        private static void ListAllCoursesWithTheirResources(StudentsContext context)
        {
            var courses = context.Courses
                                 .OrderBy(course => course.StartDate)
                                 .ThenByDescending(course => course.EndDate)
                                 .Select(course => new { course.Name, course.Description, course.Resources });

            foreach (var course in courses)
            {
                Console.WriteLine($"Course: {course.Name}");
                Console.WriteLine($"Description: {course.Description}");
                foreach (var resource in course.Resources)
                {
                    Console.WriteLine($"--Resource Name: {resource.Name}");
                    Console.WriteLine($"--Resource Type: {resource.Type}");
                    Console.WriteLine($"--Resource URL: {resource.URL}");
                }

                Console.WriteLine();
            }
        }

        private static void AllCoursesWithMoreThanFiveResources(StudentsContext context)
        {
            var courses = context.Courses
                                 .Where(course => course.Resources.Count > 5)
                                 .OrderByDescending(course => course.Resources.Count)
                                 .ThenByDescending(course => course.StartDate);

            foreach (var course in courses)
            {
                Console.WriteLine($"Course: {course.Name}");
                Console.WriteLine($"Number of Resources: {course.Resources.Count}");

                Console.WriteLine();
            }

            if (!courses.Any())
            {
                Console.WriteLine("There is no course with more than five resources!");
            }
        }

        private static void AllCoursesActiveOnGivenDate(StudentsContext context)
        {
            DateTime date = new DateTime(2017, 02, 16);

            var courses = context.Courses
                                 .Where(course => course.StartDate <= date && course.EndDate >= date)
                                 .OrderBy(course => course.Students.Count)
                                 .Select(course => new
                                 {
                                     course.Name,
                                     course.StartDate,
                                     course.EndDate,
                                     Duration = SqlFunctions.DateDiff("day", course.StartDate, course.EndDate),
                                     course.Students
                                 });

            foreach (var course in courses.OrderByDescending(course => course.Duration))
            {
                Console.WriteLine($"Course: {course.Name}");
                Console.WriteLine($"Starting on: {course.StartDate.ToShortDateString()}");
                Console.WriteLine($"Ending on: {course.EndDate.ToShortDateString()}");
                Console.WriteLine($"Duration: {course.Duration} days");
                Console.WriteLine($"Students in Course: {course.Students.Count}");

                Console.WriteLine();
            }
        }

        private static void AllCoursesOfAllStudent(StudentsContext context)
        {
            var students = context.Students
                                  .OrderByDescending(student => student.Courses.Sum(c => c.Price))
                                  .ThenByDescending(student => student.Courses.Count)
                                  .ThenBy(student => student.Name)
                                  .Select(student => new { student.Name, student.Courses });

            foreach (var student in students)
            {
                Console.WriteLine($"Student: {student.Name}");
                Console.WriteLine($"Number of Courses: {student.Courses.Count}");
                Console.WriteLine($"Total price of Courses: {student.Courses.Sum(course => course.Price)}");
                Console.WriteLine($"Average price of Courses: {student.Courses.Average(course => course.Price)}");

                Console.WriteLine();
            }
        }
    }
}
