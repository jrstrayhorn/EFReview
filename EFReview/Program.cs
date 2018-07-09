using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFReview
{
    public enum Classification : byte
    {
        Silver = 1,
        Gold = 2,
        Platinum = 3
    }

    public class Video
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Genre Genres { get; set; }
        public Classification Classification { get; set; }
    }

    public class Genre
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public IList<Video> Videos { get; set; }
    }

    public class VidzyContext : DbContext
    {
        public DbSet<Video> Videos { get; set; }
        public DbSet<Genre> Genres { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
