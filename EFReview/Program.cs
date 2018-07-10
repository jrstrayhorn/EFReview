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
        }
    }
}
