using System;
using System.Collections;
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
            var context = new VidzyContext();

            //var allVideos = context.Videos.ToList();

            //foreach (var v in allVideos)
            //{
            //    // this causes a null reference exception
            //    // why? Because you didn't eager load the videos
            //    // with their genres and lazy loading is not on currently
            //    Console.WriteLine($"Name: {v.Name}, Genre: {v.Genres.Name}");
            //}

            //// Setting Genre to virtual gets this to work now
            //// but we've introduced the N + 1 issue
            //// where EF is making N + 1 calls to database
            //// 1 to get all videos then 1 call per video being iterated
            //// to get genre
            //var allVideos = context.Videos.ToList();

            //foreach (var v in allVideos)
            //{
            //    Console.WriteLine($"Name: {v.Name}, Genre: {v.Genres.Name}");
            //}

            // turned off lazy loading via constructor of dbContext
            // so above will fail with null exception
            // using eager loading now to solve N + 1 issue
            //var allVideos = context.Videos.Include(v => v.Genres).ToList();

            //// with eager loading enabled this will now work
            //foreach (var v in allVideos)
            //{
            //    Console.WriteLine($"Name: {v.Name}, Genre: {v.Genres.Name}");
            //}

            // using explicit loading to solve N+1 issue
            var allVideos = context.Videos.ToList();
            var genreIds = allVideos.Select(v => v.GenreId);

            context.Genres.Where(g => genreIds.Contains(g.Id)).Load();

            // with explicit loading enabled this will now work
            foreach (var v in allVideos)
            {
                Console.WriteLine($"Name: {v.Name}, Genre: {v.Genres.Name}");
            }
        }
    }
}
