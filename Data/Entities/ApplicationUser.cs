using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookBazaar.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(55)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(55)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string? StreetAddress { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? State { get; set; }
        [Required]
        public string? PostalCode { get; set; }

        [ForeignKey("CompanyId")]
        [ValidateNever]
        public Company? Company { get; set; }
        public int? CompanyId { get; set; }
    }
}

