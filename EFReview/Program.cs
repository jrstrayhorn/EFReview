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
            var context = new PlutoContext();

            // EF will send query after this line w/ .Single method
            var course = context.Courses.Single(c => c.Id == 2);

            // will send another call to Database for the call (not loaded immedately)
            // Tags not loaded until needed
            foreach (var tag in course.Tags)
            {
                Console.WriteLine(tag.Name);
            }
        
        }
    }
}
