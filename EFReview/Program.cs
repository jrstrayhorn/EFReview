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
            var dbContext = new VidzyDbContext();

            dbContext.AddVideo("Black Panther", new DateTime(2018, 2, 16), "Action");
            dbContext.AddVideo("Wedding Crashers", new DateTime(2000, 3, 5), "Comedy");
        }
    }
}
