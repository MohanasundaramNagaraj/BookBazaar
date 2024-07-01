using BookBazaar.Entities;

namespace BookBazaar.ViewModels
{
    public class OrderViewModel
    {
        public OrderHeader OrderHeader { get; set; } = new();
        public IEnumerable<OrderDetail>? OrderDetail { get; set; }
    }
}
