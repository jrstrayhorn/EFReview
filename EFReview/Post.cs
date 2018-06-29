namespace EFReview
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PostID { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime DatePublished { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(8000)]
        public string Body { get; set; }
    }
}
