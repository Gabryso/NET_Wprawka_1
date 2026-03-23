using System.ComponentModel.DataAnnotations;

namespace WA1.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public ICollection<Hero> Heros { get; set; } = new List<Hero>();
    }
}
