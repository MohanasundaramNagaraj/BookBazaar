using BookBazaar.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BookBazaar.ViewModels
{
    public class ShoppingCartViewModel
    {
        [ValidateNever]
        public IEnumerable<ShoppingCart> ShoppingCarts { get; set; }

        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }


    }
}
