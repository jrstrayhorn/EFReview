using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFReview.EntityConfigurations
{
    public class TagConfiguration : EntityTypeConfiguration<Tag>
    {
        public TagConfiguration()
        {
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(255);

            HasMany(t => t.Videos)
                .WithMany(v => v.Tags)
                .Map(m =>
                {
                    m.ToTable("VideoTags");
                    m.MapLeftKey("TagId");
                    m.MapRightKey("VideoId");
                });
        }
    }
}
