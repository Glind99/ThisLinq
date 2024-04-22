using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThisLinq.Models
{
    public class Class
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClassID { get; set; }

        [Required]
        public string ClassName { get; set; }

        public virtual ICollection<ConnectionLink> Connections { get; set; }

    }
}
