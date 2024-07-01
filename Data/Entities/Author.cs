using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookBazaar.Entities
{
    [Table("Authors")]
    public class Author
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [MaxLength(100)]
        public string? FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(100)]
        public string? LastName { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        [MaxLength(255)]
        public string? FullName { get; set; }

        public ICollection<Book>? Books { get; set; }
    }
}
