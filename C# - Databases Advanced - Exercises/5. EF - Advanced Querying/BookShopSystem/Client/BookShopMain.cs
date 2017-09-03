namespace BookShopSystem.Client
{
    using Data;
    using EntityFramework.Extensions;
    using Models;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;

    internal class BookShopMain
    {
        private static void Main(string[] args)
        {
            BookShopContext context = new BookShopContext();

            // If you want to reset the sample data after running the Update or Delete tasks - Task 14 and Task 15 - you need to drop the database,
            // uncomment the 'MigrateDatabaseToLatestVersion' command in the Seed method and start the project again with the following command:
            //context.Database.Initialize(true);

            // Task 1
            //BooksTitlesByAgeRestriction(context);

            // Task 2
            //GolderBooks(context);

            // Task 3
            //BooksByPrice(context);

            // Task 4
            //NotReleasedBooks(context);

            // Task 5
            //BookTitlesByCategory(context);

            // Task 6
            //BooksReleasedBeforeDate(context);

            // Task 7
            //AuthorsSearch(context);

            // Task 8
            //BooksSearch(context);

            // Task 9
            //BookTitlesSearch(context);

            // Task 10
            //CountBooks(context);

            // Task 11
            //TotalBookCopies(context);

            // Task 12
            //FindProfit(context);

            // Task 13
            //MostRecentBook(context);

            // Task 14
            //IncreaseBookCopies(context);

            // Task 15
            //RemoveBooks(context);

            // Task 16
            //StoredProcedure(context);
        }

        private static void BooksTitlesByAgeRestriction(BookShopContext context)
        {
            string input = Console.ReadLine();
            switch (input.ToLower())
            {
                case "minor":
                    var minorBooks = context.Books
                        .Where(b => b.AgeRestriction == AgeRestriction.Minor)
                        .Select(b => b.Title)
                        .ToList();

                    foreach (var book in minorBooks)
                    {
                        Console.WriteLine(book);
                    }
                    break;

                case "teen":
                    var teenBooks = context.Books
                        .Where(b => b.AgeRestriction == AgeRestriction.Teen)
                        .Select(b => b.Title)
                        .ToList();

                    foreach (var book in teenBooks)
                    {
                        Console.WriteLine(book);
                    }
                    break;

                case "adult":
                    var adultBooks = context.Books
                        .Where(b => b.AgeRestriction == AgeRestriction.Adult)
                        .Select(b => b.Title)
                        .ToList();

                    foreach (var book in adultBooks)
                    {
                        Console.WriteLine(book);
                    }
                    break;

                default:
                    Console.WriteLine("Invalid input!");
                    break;
            }
        }

        private static void GolderBooks(BookShopContext context)
        {
            var goldenBooks = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .Select(b => new { b.Id, b.Title })
                .OrderBy(b => b.Id)
                .ToList();

            foreach (var book in goldenBooks)
            {
                Console.WriteLine(book.Title);
            }
        }

        private static void BooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Price < 5 || b.Price > 40)
                .Select(b => new { b.Id, b.Title, b.Price })
                .OrderBy(b => b.Id)
                .ToList();

            foreach (var book in books)
            {
                Console.WriteLine($"{book.Title} - ${book.Price}");
            }
        }

        private static void NotReleasedBooks(BookShopContext context)
        {
            int inputYear = int.Parse(Console.ReadLine());

            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != inputYear)
                .Select(b => new { b.Id, b.Title })
                .OrderBy(b => b.Id)
                .ToList();

            foreach (var book in books)
            {
                Console.WriteLine(book.Title);
            }
        }

        private static void BookTitlesByCategory(BookShopContext context)
        {
            string[] categories = Console.ReadLine().ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var books = context.Books
                .Where(b => b.Categories.Count(c => categories.Contains(c.Name)) != 0)
                .Select(b => new { b.Id, b.Title })
                .OrderBy(b => b.Id)
                .ToList();

            foreach (var book in books)
            {
                Console.WriteLine(book.Title);
            }
        }

        private static void BooksReleasedBeforeDate(BookShopContext context)
        {
            string inputDate = Console.ReadLine();

            DateTime givenDate = DateTime.ParseExact(inputDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(b => b.ReleaseDate < givenDate)
                .Select(b => new { b.Title, b.EditionType, b.Price })
                .ToList();

            foreach (var book in books)
            {
                Console.WriteLine($"{book.Title} - {book.EditionType} - {book.Price}");
            }
        }

        private static void AuthorsSearch(BookShopContext context)
        {
            string input = Console.ReadLine();

            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new { a.FirstName, a.LastName })
                .ToList();

            foreach (var author in authors)
            {
                Console.WriteLine($"{author.FirstName} {author.LastName}");
            }
        }

        private static void BooksSearch(BookShopContext context)
        {
            string input = Console.ReadLine().ToLower();

            var books = context.Books
                .Where(b => b.Title.Contains(input))
                .Select(b => b.Title)
                .ToList();

            foreach (var book in books)
            {
                Console.WriteLine(book);
            }
        }

        private static void BookTitlesSearch(BookShopContext context)
        {
            string input = Console.ReadLine().ToLower();

            var books = context.Books
                .Where(b => b.Author.LastName.StartsWith(input))
                .Select(b => new { b.Id, b.Title, b.Author })
                .OrderBy(b => b.Id)
                .ToList();

            foreach (var book in books)
            {
                Console.WriteLine($"{book.Title} ({book.Author.FirstName} {book.Author.LastName})");
            }
        }

        private static void CountBooks(BookShopContext context)
        {
            int inputSymbolsCount = int.Parse(Console.ReadLine());

            var books = context.Books
                .Where(b => b.Title.Length > inputSymbolsCount)
                .ToList();

            Console.WriteLine(books.Count);

            // There are {books.Count} books with longer title than {inputSymbolsCount} symbols;
        }

        private static void TotalBookCopies(BookShopContext context)
        {
            //Solution I

            var authors = context.Authors
                 .GroupBy(a => new
                 {
                     FullName = a.FirstName + " " + a.LastName,
                     TotalCopies = a.Books.Sum(b => b.Copies)
                 })
                 .OrderByDescending(a => a.Key.TotalCopies)
                 .ToList();

            foreach (var author in authors)
            {
                Console.WriteLine($"{author.Key.FullName} - {author.Key.TotalCopies}");
            }

            //Solution II

            //var books = context.Books
            //     .GroupBy(b => b.Author)
            //     .Select(b => new
            //     {
            //         Author = b.Key,
            //         Copies = b.Sum(c => c.Copies)
            //     })
            //     .OrderByDescending(c => c.Copies)
            //     .ToList();

            //foreach (var book in books)
            //{
            //    Console.WriteLine($"{book.Author.FirstName} {book.Author.LastName} - {book.Copies}");
            //}
        }

        private static void FindProfit(BookShopContext context)
        {
            var categories = context.Categories
                 .GroupBy(c => new
                 {
                     CategoryName = c.Name,
                     TotalProfit = c.Books.Sum(b => b.Copies * b.Price)
                 })
                 .OrderByDescending(c => c.Key.TotalProfit)
                 .ThenBy(c => c.Key.CategoryName)
                 .ToList();

            foreach (var category in categories)
            {
                Console.WriteLine($"{category.Key.CategoryName} - ${category.Key.TotalProfit}");
            }
        }

        private static void MostRecentBook(BookShopContext context)
        {
            var categories = context.Categories
                .GroupBy(c => new
                {
                    CategoryName = c.Name,
                    TotalBookCount = c.Books.Count
                })
                .Where(c => c.Key.TotalBookCount > 35)
                .OrderByDescending(c => c.Key.TotalBookCount)
                .ToList();

            foreach (var category in categories)
            {
                Console.WriteLine($"--{category.Key.CategoryName}: {category.Key.TotalBookCount} books");

                var books = context.Books
                    .Where(b => b.Categories.Any(c => c.Name == category.Key.CategoryName))
                    .Select(b => new { b.Title, b.ReleaseDate })
                    .OrderByDescending(b => b.ReleaseDate)
                    .ToList();

                int counter = 1;

                foreach (var book in books)
                {
                    Console.WriteLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                    counter++;
                    if (counter > 3)
                    {
                        break;
                    }
                }
            }
        }

        private static void IncreaseBookCopies(BookShopContext context)
        {
            DateTime date = new DateTime(2013, 06, 13);
            int newCopies = 44;

            int bookCopiesToUpdate = context.Books.Count(b => b.ReleaseDate > date) * newCopies;

            //Solution I

            context.Books
                .Where(b => b.ReleaseDate > date)
                .Update(b => new Book() { Copies = b.Copies + newCopies });

            //Solution II

            //var books = context.Books.Where(b => b.ReleaseDate > date);
            //context.Books.Update(books, b => new Book() { Copies = b.Copies + newCopies });

            //Solution III

            //var books = context.Books.Where(b => b.ReleaseDate > date);
            //foreach (var book in books)
            //{
            //    book.Copies += newCopies;
            //}

            int allCopiesAfterUpdate = context.Books.Where(b => b.ReleaseDate > date).Sum(b => b.Copies);

            Console.WriteLine($"Number of copies to update - {bookCopiesToUpdate} \n");
            Console.WriteLine($"All copies after the update - {allCopiesAfterUpdate} \n");

            // With EF Extended Update there is no need of context.SaveChanges();
        }

        private static void RemoveBooks(BookShopContext context)
        {
            int input = 4200;

            var booksForDelete = context.Books.Where(b => b.Copies < input);

            Console.WriteLine($"Number of books before delete - {context.Books.Count()} \n");
            Console.WriteLine($"Number of books to delete - {booksForDelete.Count()} \n");

            //Solution I - with EF Extended Delete there is no need of context.SaveChanges();

            context.Books.Delete(booksForDelete);

            //Solution II

            //context.Books.RemoveRange(booksForDelete);
            //context.SaveChanges();

            Console.WriteLine($"Number of books after delete - {context.Books.Count()} \n");
        }

        private static void StoredProcedure(BookShopContext context)
        {
            string[] authorNames = Console.ReadLine().Split(' ');

            SqlParameter firstName = new SqlParameter("@FirstName", SqlDbType.VarChar);
            SqlParameter lastName = new SqlParameter("@LastName", SqlDbType.VarChar);

            firstName.Value = authorNames[0];
            lastName.Value = authorNames[1];

            int writtenBooksNumber = context.Database.SqlQuery<int>("udp_TotalBooksByAuthor @FirstName, @LastName", firstName, lastName).Single();

            Console.WriteLine($"{firstName.Value} {lastName.Value} has written {writtenBooksNumber} books");

            //    Procedure written in MSSQL Server:
            //    CREATE PROCEDURE udp_TotalBooksByAuthor(@FirstName VARCHAR(max), @LastName VARCHAR(max))
            //    AS
            //    BEGIN
            //     SELECT COUNT(b.Id)
            //       FROM Books AS b
            //       JOIN Authors AS a ON b.AuthorId = a.Id
            //      WHERE a.FirstName = @FirstName
            //        AND a.LastName = @LastName
            //    END
        }
    }
}