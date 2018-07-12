using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace EFReview
{
    class Program
    {
        static void Main(string[] args)
        {
            // 3rd way - some how we have a Author object that is not in
            // DBContext
            //var author = new Author() { Id = 1, Name = "Mosh Hamedani" };

            var context = new PlutoContext();
            // 3rd way - bring into DBContext
            // context.Authors.Attach(author);

            //// Author objects already in DB Context from previous action
            //var authors = context.Authors.ToList();

            //var author = context.Authors.Single(a => a.Id == 1);

            // DONT need the above lines of code if using foreign key approach

            // Add a new object
            var course = new Course
            {
                Name = "New Course",
                Description = "New Description",
                FullPrice = 19.95f,
                Level = 1,
                // there is an issue with below code
                // because we instantiate a new Author here
                // EF will create a new record in the Author table
                // instead of using the existing Author with Id of 1 in DB
                //Author = new Author { Id = 1, Name = "Mosh Hamedani" }

                // resovle by #1 - bringing Author into DBContext 
                // or #2 - use AuthorId foreign key
                // #1 works better in wpf application, #2 work better in Web applications


                // wpf DBContext is long lived, could already have author based on previous actions
                // just get from context
                // Author = author
                // if author object is not in context, then EF will run query to get
                // author object and put into memory

                // foreign key approach
                //AuthorId = 1
                // in web app, DbContext is short lived, create context and dispose

                // a third way, not common, don't use this
                // Author = author // use the newly attached author object
            };

            context.Courses.Add(course);

            var courseToEdit = context.Courses.Find(4); // same as Single(c => c.Id == 4)
                // can also pass and find by composite keys like .Find(4, 1);
            courseToEdit.Name = "New Name";
            courseToEdit.AuthorId = 2;

            // you have to load object 1st, before you can change them
            // because ChangeTracker needs the object to update state

            var courseToRemove = context.Courses.Find(6);
            context.Courses.Remove(courseToRemove);

            //var authorToRemove = context.Authors.Find(2);
            // if you want to remove author you have to bring in Courses as well.
            var authorToRemove = context.Authors
                .Include(a => a.Courses)
                .Single(a => a.Id == 2);

            context.Courses.RemoveRange(authorToRemove.Courses);
            context.Authors.Remove(authorToRemove); 
            // this will cause an exception bc of foreign key constraint
            // there are existing courses with that author id
            // sql server won't allow to delete in this state

            context.SaveChanges();


            // Working with Change Tracker
            // Add an object
            context.Authors.Add(new Author { Name = "New Author" });

            // Update an object
            var author = context.Authors.Find(3);
            author.Name = "Updated";

            // Remove an object
            var another = context.Authors.Find(4);
            context.Authors.Remove(another);

            var entries = context.ChangeTracker.Entries();  // can also use .Entries<Author>() to be author entries

            foreach (var entry in entries)
            {
                entry.Reload();
                Console.WriteLine(entry.State);
            }
        
        }
    }
}
