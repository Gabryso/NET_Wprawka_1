using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetZajW2.Models
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
    }

}
