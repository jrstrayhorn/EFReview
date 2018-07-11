using EFReview.EntityConfigurations;
using System;
using System.Collections.Generic;

namespace EFReview
{
    public class Video
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public byte GenreId { get; set; }
        public virtual Genre Genres { get; set; }
        public Classification Classification { get; set; }
        public IList<Tag> Tags { get; set; }
    }
}
