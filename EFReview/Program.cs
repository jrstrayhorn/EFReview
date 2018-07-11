using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFReview
{
    class Program
    {
        static void Main(string[] args)
        {
            //var context = new PlutoContext();

            //// LINQ syntax
            //// good for those comfortable with sql
            //var query =
            //    from c in context.Courses
            //    where c.Name.Contains("c#")
            //    orderby c.Name
            //    select c;

            //foreach (var course in query)
            //{
            //    Console.WriteLine(course.Name);
            //}

            //// Extension methods
            //// Good with lambda, delegates, etc
            //// this is more powerful, so perfer this way
            //var courses = context.Courses
            //    .Where(c => c.Name.Contains("c#"))
            //    .OrderBy(c => c.Name);

            //foreach (var course in courses)
            //{
            //    Console.WriteLine(course.Name);
            //}

            var context = new PlutoContext();

            // Filtering
            var query =
                from c in context.Courses
                where c.Level == 1 && c.Author.Id == 1
                select c;

            // Sorting
            var sortingQuery =
                from c in context.Courses
                where c.Author.Id == 1
                orderby c.Level descending, c.Name
                select c;

            // Projection
            var projectionQuery =
                from c in context.Courses
                where c.Author.Id == 1
                orderby c.Level descending, c.Name
                select new { c.Name, Author = c.Author.Name };

            // Grouping
            // this will give us a list of groups
            var groupingQuery =
                from c in context.Courses
                group c by c.Level
                into g
                select g;

            foreach (var group in groupingQuery)
            {
                Console.WriteLine(group.Key);

                foreach (var course in group)
                {
                    Console.WriteLine($"\t{course.Name}");
                }
            }

            // aggregate with group
            // group is an IEnumerable of course
            foreach (var group in groupingQuery)
            {
                Console.WriteLine($"Level: {group.Key} ({group.Count()})");
                
            }

            // Joining
            // in LINQ, 3 types of joins
            // Inner Join - similar to SQL
            // Group Join
            // Cross Join - similar to SQL
            var innerJoiningQuery =
                from c in context.Courses
                select new { CourseName = c.Name, AuthorName = c.Author.Name };

            // use inner join that do not have navigation property
            // complex model, lots of relationships
            // may only have Author Id instead of nav property
            var innerJoinQuery =
                from c in context.Courses
                join a in context.Authors on c.AuthorId equals a.Id
                select new { CourseName = c.Name, AuthorName = a.Name };

            // group join for any left join like from sql
            // a (Authors) is the left side
            // g (group of matching courses) is the right side
            var groupJoinQuery =
                from a in context.Authors
                join c in context.Courses on a.Id equals c.AuthorId into g
                select new { AuthorName = a.Name, CourseCount = g.Count() };

            foreach (var x in groupJoinQuery)
            {
                Console.WriteLine($"{x.AuthorName} Count of Courses: {x.CourseCount}");
            }

            // cross join
            // all authors and all courses
            var crossJoinQuery =
                from a in context.Authors
                from c in context.Courses
                select new { AuthorName = a.Name, CourseName = c.Name };

            foreach (var x in crossJoinQuery)
            {
                Console.WriteLine($"{x.AuthorName} - {x.CourseName}");
            }


            // LINQ Extension Methods

            // all courses level 1
            // Restriction, Ordering, Projection
            var courses = context.Courses
                .Where(c => c.Level == 1)
                .OrderByDescending(c => c.Name)
                .ThenBy(c => c.Level)
                .Select(c => new { CourseName = c.Name, AuthorName = c.Author.Name });

            // Projection - flatten hierarchial data
            var tags = context.Courses
                .Where(c => c.Level == 1)
                .OrderByDescending(c => c.Name)
                .ThenByDescending(c => c.Level)
                .SelectMany(c => c.Tags)
                .Distinct();

            foreach (var t in tags)
            {
                Console.WriteLine(t.Name);
            }

            // set operation  - distinct

            // Grouping
            var groups = context.Courses
                .GroupBy(c => c.Level);

            foreach (var group in groups)
            {
                Console.WriteLine($"Key: {group.Key}");

                // Order each grouping of items by name
                foreach (var course in group.OrderBy(c => c.Name))
                {
                    Console.WriteLine($"\t{course.Name}");
                }
            }

            // Joining

            // Inner Join when no relationship between entities
            // if relationship use navigation property
            var innerJoinCourseAuthor = context.Courses.Join(context.Authors, 
                c => c.AuthorId, 
                a => a.Id, 
                (course, author) => new
                    {
                        CourseName = course.Name,
                        AuthorName = author.Name
                    });

            // Group Join
            var groupJoinAuthorCourses = context.Authors.GroupJoin(context.Courses, 
                a => a.Id, 
                c => c.AuthorId,
                (author, authorCourses) => new
                {
                    AuthorName = author.Name,
                    CourseCount = authorCourses.Count()
                });

            // Cross Join
            // this is select many
            // author courses combined
            context.Authors.SelectMany(a => context.Courses, (author, course) => new
            {
                AuthorName = author.Name,
                CourseName = course.Name
            });

            // LINQ Extension Methods - Additonal Methods not support in Query syntax
            // Partitioning
            // for a page of records, size of 10
            var courseSecondPage = context.Courses.Skip(10).Take(10);

            // Element Operators
            // what if you want a single object instead of a list
            context.Courses.OrderBy(c => c.Level).First(c => c.FullPrice > 100);

            // first will give exception if null
            // firstOrDefault will return null if no records
            // this has overloads so you can use with or without lambda expression

            // can use this with SQL - context.Courses.Last
            // sort descending then get first

            // if expression returns more than 1 you'll get an exception below
            context.Courses.SingleOrDefault(c => c.Id == 1);

            // Quantifying - all return boolean
            var allAbove10Dollars = context.Courses.All(c => c.FullPrice > 10);

            var anyLevel1Courses = context.Courses.Any(c => c.Level == 1);

            // Aggregating
            var count = context.Courses.Where(c => c.Level == 1).Count();

            // also have avg, sum, max and min
            context.Courses.Max(c => c.FullPrice);
            context.Courses.Min(c => c.FullPrice);
            context.Courses.Average(c => c.FullPrice);

            // DEFERRED EXECUTION
            // query is executed and brought into memory
            // this impacts performance though....
            // because you bring in all records into memory
            // instead of just the records that you need
            var allCourses = context.Courses.ToList();

            var beginnerCourses = allCourses.Where(c => c.IsBeginnerCourse);
            //var filtered = allCourses.Where(c => c.Level == 1);
            //var sorted = filtered.OrderBy(c => c.Name);

            foreach (var c in allCourses)
            {
                Console.WriteLine(c.Name);
            }

            // Queries are not execxuted at the time you create them

            // Query 
        
        }
    }
}
