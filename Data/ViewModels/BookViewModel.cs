using BookBazaar.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookBazaar.ViewModels
{

    public class BookViewModel
    {
        public Book Book { get; set; }

        [ValidateNever]
        [Display(Name = "Author")]
        public IEnumerable<SelectListItem> Authors { get; set; }

        [ValidateNever]
        [Display(Name = "Category")]
        public IEnumerable<SelectListItem> Categories { get; set; }

        [ValidateNever]
        [Display(Name = "Cover")]
        public IEnumerable<SelectListItem> Covers { get; set; }

    }
}
