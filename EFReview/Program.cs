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

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
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
