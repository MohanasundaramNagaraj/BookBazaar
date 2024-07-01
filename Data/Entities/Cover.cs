using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookBazaar.Entities
{
    [Table("Covers")]
    public class Cover
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(55)]
        public string? Name { get; set; }
    }
}
