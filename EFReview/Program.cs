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
            try
            {
                /*1- Add a new video called “Terminator 1” with genre Action, 
                 * release date 26 Oct, 1984, and Silver classification. 
                 * Ensure the Action genre is not duplicated in the Genres table.
                 */
                //AddNewVideo("Terminator 1", "Action", new DateTime(1984, 10, 26), Classification.Silver);
                //Console.WriteLine("***New Video Added Successfully***");

                /*2- Add two tags “classics” and “drama” to the database. 
                 * Ensure if your method is called twice, 
                 * these tags are not duplicated.
                 */
                //AddNewTag("Classics");
                //AddNewTag("Drama");
                //AddNewTag("Drama");
                //Console.WriteLine("***New Tags Added Successfully***");

                /*3- Add three tags “classics”, “drama” and “comedy” to the video with Id 2 
                 * (The Godfather). Ensure the “classics” and “drama” tags are not 
                 * duplicated in the Tags table. Also, ensure that if your method 
                 * is called twice, these tags are not duplicated in VideoTags table.
                 */
                //AddTagToVideo(2, "Classics");
                //AddTagToVideo(2, "Drama");
                //AddTagToVideo(2, "Comedy");
                //AddTagToVideo(2, "Drama");

                //4- Remove the "comedy" tag from the video with Id 2 (The Godfather)
                //RemoveTagFromVideo(2, "Comedy");

                //5- Remove the video with Id 2 (The Godfather)
                //RemoveVideo(2);

                //6- Remove the genre with Id 2 (Action).  Ensure all videos with this genre
                // are deleted from the database.
                //RemoveGenre(2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static void RemoveGenre(int genreId)
        {
            if (genreId <= 0)
            {
                throw new InvalidOperationException("Please enter a valid genre Id");
            }

            using (var context = new VidzyContext())
            {
                var genre = context.Genres
                    .Include(g => g.Videos)
                    .SingleOrDefault(g => g.Id == genreId);

                if (genre == null)
                {
                    throw new ArgumentNullException("Genre does not exist.");
                }

                // removing Genre's videos FIRST
                context.Videos.RemoveRange(genre.Videos);

                // THEN removing Genre
                context.Genres.Remove(genre);

                context.SaveChanges();
            }
        }

        private static void RemoveVideo(int videoId)
        {
            if (videoId <= 0)
            {
                throw new InvalidOperationException("Please enter a valid video Id");
            }

            using (var context = new VidzyContext())
            {
                var video = context.Videos.Find(videoId);

                if (video == null)
                {
                    throw new ArgumentNullException("Video does not exist.");
                }

                context.Videos.Remove(video);

                context.SaveChanges();
            }
        }

        private static void RemoveTagFromVideo(int videoId, string tagName)
        {
            if (videoId <= 0)
            {
                throw new InvalidOperationException("Please enter a valid video Id");
            }

            if (string.IsNullOrWhiteSpace(tagName))
            {
                throw new InvalidOperationException("Please enter a valid tag name");
            }

            using (var context = new VidzyContext())
            {
                var video = context.Videos.Include(v => v.Tags).SingleOrDefault(v => v.Id == videoId);

                if (video == null)
                {
                    throw new ArgumentNullException("Video does not exists.");
                }

                if (!video.Tags.Select(t => t.Name).Contains(tagName))
                {
                    throw new InvalidOperationException("Tag does not exist on Video.");
                }

                var tag = context.Tags.SingleOrDefault(t => t.Name == tagName);

                if (tag == null)
                {
                    throw new ArgumentNullException("Tag does not exist in database.");
                }

                video.Tags.Remove(tag);

                context.SaveChanges();
            }
        }

        private static void AddTagToVideo(int videoId, string tagName)
        {
            if (videoId <= 0)
            {
                throw new InvalidOperationException("Please enter a valid video Id");
            }

            if (string.IsNullOrWhiteSpace(tagName))
            {
                throw new InvalidOperationException("Please enter a valid tag name");
            }

            using (var context = new VidzyContext())
            {
                var video = context.Videos.Include(v => v.Tags).SingleOrDefault(v => v.Id == videoId);

                if (video == null)
                {
                    throw new ArgumentNullException("Video does not exists.");
                }

                var tagAlreadyExistsInDB = context.Tags.Any(t => t.Name == tagName);

                if (tagAlreadyExistsInDB && video.Tags.Select(t => t.Name).Contains(tagName))
                {
                    throw new InvalidOperationException("Tag has already been added to Video.");
                }

                if (!tagAlreadyExistsInDB)
                {
                    AddNewTag(tagName);
                }

                var tag = context.Tags.Single(t => t.Name == tagName);
                video.Tags.Add(tag);

                context.SaveChanges();
            }
        }

        private static void AddNewTag(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                throw new ArgumentException("Please enter a tag name.");
            }

            using (var context = new VidzyContext())
            {
                if (context.Tags.Any(t => t.Name == tagName))
                {
                    throw new InvalidOperationException("You cannot create duplicate tags.");
                }

                var newTag = new Tag
                {
                    Name = tagName
                };

                context.Tags.Add(newTag);

                context.SaveChanges();
            }
        }

        private static void AddNewVideo(string videoName, string genreName, DateTime releaseDate, Classification classification)
        {
            if (string.IsNullOrWhiteSpace(videoName))
            {
                throw new ArgumentException("Please enter a video name.");
            }

            if (string.IsNullOrWhiteSpace(genreName))
            {
                throw new ArgumentException("Please enter a genre name.");
            }

            using (var context = new VidzyContext())
            {
                var genre = context.Genres.SingleOrDefault(g => g.Name == genreName.Trim());
                if (genre == null)
                {
                    throw new InvalidOperationException("Genre does not exist. Please try again with a genre that exists.");
                }

                var newVideo = new Video
                {
                    Name = videoName,
                    Genres = genre,
                    ReleaseDate = releaseDate,
                    Classification = classification,
                };

                context.Videos.Add(newVideo);

                context.SaveChanges();
            }
        }
    }
}
