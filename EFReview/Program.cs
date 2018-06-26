using System;

namespace EFReview
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new VidzyDbContext();

            //dbContext.AddVideo("Black Panther", new DateTime(2018, 2, 16), "Action");
            //dbContext.AddVideo("Wedding Crashers", new DateTime(2000, 3, 5), "Comedy");

            dbContext.AddVideo("Iron Man", new DateTime(2008, 4, 1), "Action");
            dbContext.AddVideo("Thor Ragnarok", new DateTime(2017, 11, 10), "Comedy");
        }
    }
}
