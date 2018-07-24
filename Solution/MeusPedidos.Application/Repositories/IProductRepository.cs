using MeusPedidos.Application;

namespace MeusPedidos.AndroidApp.Repositories
{
    public interface IProductRepository
    {
        void AddToFavorites(ProductViewModel vm);

        bool IsFavorite(ProductViewModel vm);

        void RemoveFromFavorites(ProductViewModel vm);
    }
}