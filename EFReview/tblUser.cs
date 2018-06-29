namespace EFReview
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblUser")]
    public partial class tblUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        public int Password { get; set; }
    }
}
