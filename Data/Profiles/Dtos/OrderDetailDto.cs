using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookBazaar.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace BookBazaar.Profiles.Dtos
{
    [Table("OrderDetail")]
    public class OrderDetailDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        [ValidateNever]
        public OrderHeader? OrderHeader { get; set; }

        [Required]
        public int BookId { get; set; }

        [ForeignKey("BookId")]
        [ValidateNever]
        public Book? Book { get; set; }

        public int Count { get; set; }

        public double Price { get; set; }
    }
}
