using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThisLinq.Models
{
        public class ConnectionLink
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required]
            [ForeignKey("Classes")]
            public int FK_ClassId { get; set; }
            public virtual Class Classes { get; set; }

            [Required]
            [ForeignKey("Courses")]
            public int FK_CourseId { get; set; }
            public virtual Course Courses { get; set; }

            [Required]
            [ForeignKey("Students")]
            public int FK_StudentId { get; set; }
            public virtual Student Students { get; set; }

            [Required]
            [ForeignKey("Teachers")]
            public int FK_TeacherId { get; set; }
            public virtual Teacher Teachers { get; set; }
        }
}
