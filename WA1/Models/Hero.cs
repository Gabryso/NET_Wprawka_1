using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WA1.Models
{
    public class Hero
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(256)]
        public string Bio { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}