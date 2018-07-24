using MeusPedidos.Domain;

namespace MeusPedidos.Application
{
    public class CartItemViewModel
    {
        public CartItemViewModel()
        {
        }

        internal CartItemViewModel(CartItem cartItem)
        {
            Discount = cartItem.DiscountPercent.ValuePercent;
            Id = cartItem.Product.Id;
            Name = cartItem.Product.Name;
            Price = cartItem.PriceToPay;
            Units = cartItem.Units;
            PhotoUri = cartItem.Product.Photo.AbsoluteUri;
        }

        public decimal Discount { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhotoUri { get; set; }
        public decimal Price { get; set; }
        public int Units { get; set; }
    }
}