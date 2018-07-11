using System;
using System.Collections;
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
            var context = new VidzyContext();

            Console.WriteLine("***Action movies sorted by name");

            var actionMovies = context.Videos
                .Where(v => v.Genres.Name == "Action")
                .OrderBy(v => v.Name);

            foreach (var v in actionMovies)
            {
                Console.WriteLine($"\t{v.Name}");
            }

            Console.WriteLine("***Gold drama movies sorted by release date (newest first)");

            var goldDramaMovies = context.Videos
                .Where(v => v.Genres.Name == "Drama" && v.Classification == Classification.Gold)
                .OrderByDescending(v => v.ReleaseDate);

            foreach (var v in goldDramaMovies)
            {
                Console.WriteLine($"\t{v.Name}");
            }

            Console.WriteLine("***All movies projected into an anonymous type with two properties: MovieName and Genre");

            var allMovieNameGenre = context.Videos
                .Select(v => new { MovieName = v.Name, Genre = v.Genres.Name });

            foreach (var item in allMovieNameGenre)
            {
                Console.WriteLine($"\t - Movie Name: {item.MovieName}, Genre: {item.Genre}");
            }

            Console.WriteLine("***All movies grouped by their classification");

            var allMoviesGroupedByClassification = context.Videos
                .GroupBy(v => v.Classification)
                .Select(g => new { Classification = g.Key.ToString(), Movies = (IEnumerable<Video>)g });

            foreach (var g in allMoviesGroupedByClassification)
            {
                Console.WriteLine($"Classification: {g.Classification}");

                foreach (var v in g.Movies.OrderBy(v => v.Name))
                {
                    Console.WriteLine($"\t{v.Name}");
                }
            }

            Console.WriteLine("***List of classifications sorted alphabetically and count of videos in them.");

            var classificationMovieCount = context.Videos
                .GroupBy(v => v.Classification.ToString())
                .OrderBy(g => g.Key);

            foreach (var g in classificationMovieCount)
            {
                Console.WriteLine($"\t{g.Key} ({g.Count()})");
            }

            Console.WriteLine("***List of genres and number of videos they include, sorted by the number of videos. Genres with the highest number of videos come first.");

            var genreGroupJoinVideos = context.Genres
                .GroupJoin(context.Videos,
                g => g.Id,
                v => v.GenreId,
                (genre, genreVideos) => new
                {
                    GenreName = genre.Name,
                    VideoCount = genreVideos.Count()
                })
                .OrderByDescending(gv => gv.VideoCount);

            foreach (var gv in genreGroupJoinVideos)
            {
                Console.WriteLine($"\t{gv.GenreName} ({gv.VideoCount})");
            }
        }
    }
}
