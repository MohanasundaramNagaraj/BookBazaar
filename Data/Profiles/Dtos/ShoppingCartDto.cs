using BookBazaar.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookBazaar.Profiles.Dtos
{
    public class ShoppingCartDto
    {
        public Guid Id { get; set; }

        public int BookId { get; set; }

        [ForeignKey("BookId")]
        [ValidateNever]
        public Book? Book { get; set; }


        [Range(1, 1000, ErrorMessage = "Please enter a valur between 1 and 1000")]
        public int Count { get; set; }

        public string? ApplicationUserId { get; set; }


        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
