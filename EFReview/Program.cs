using System;
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
                Console.WriteLine(v.Name);
            }
        }
    }
}
