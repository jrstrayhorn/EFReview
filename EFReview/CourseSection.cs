namespace EFReview
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CourseSection
    {
        [Key]
        public int SectionID { get; set; }

        public int CourseID { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public virtual Course Cours { get; set; }
    }
}
