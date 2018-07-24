namespace MeusPedidos.Application
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
        }

        public int? CategoryId { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public bool IsFavorite { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public decimal Price { get; set; }
    }
}