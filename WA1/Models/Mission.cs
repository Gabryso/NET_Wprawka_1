using System.ComponentModel.DataAnnotations;

namespace WA1.Models
{
    public class Mission
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual ICollection<Hero>? Heroes { get; set; }
    }
}
