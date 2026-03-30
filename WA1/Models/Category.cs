using System.ComponentModel.DataAnnotations;

namespace WA1.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public virtual ICollection<Hero>? Heroes { get; set; }
    }
}
