using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFReview.EntityConfigurations
{
    public class GenreConfiguration : EntityTypeConfiguration<Genre>
    {
        public GenreConfiguration()
        {
            Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(255);

            HasMany(g => g.Videos)
                .WithRequired(v => v.Genres)
                .HasForeignKey(v => v.GenreId)
                .WillCascadeOnDelete(false);
        }
    }
}
