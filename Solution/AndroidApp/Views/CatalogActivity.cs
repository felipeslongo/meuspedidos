using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using MeusPedidos.Application;
using MeusPedidos.Application.Services;
using System.Globalization;

namespace MeusPedidos.AndroidApp.Views
{
    [Activity(
        //Name = "AndroidApp.Views.CartActivity",
        Label = "Catálogo")]
    public class CatalogActivity : AppCompatActivity
    {
        private CatalogItemRecyclerViewAdapter adapter;
        private AppCompatButton buyActionButton;
        private CartService cartService = ServiceFactory.CreateCartService();
        private CatalogService catalogService = ServiceFactory.CreateCatalogService();
        private CategoryFilter categoryFilter;
        private CategoryService categoryService = ServiceFactory.CreateCategoryService();
        private ProductService productService = ServiceFactory.CreateProductService();

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            CreateCategoryFilter(menu);

            var output = base.OnCreateOptionsMenu(menu);
            return output;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.GroupId != CategoryFilter.GroupId)
                return base.OnOptionsItemSelected(item);
            ApplyUserSelectedFilters(item);
            return true;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_catalog);
            Startup();

            // data to populate the RecyclerView with
            InitCatalogRecyclerView();

            buyActionButton = FindViewById<AppCompatButton>(Resource.Id.bottomActionButton);
            UpdateTotalPrice();
            buyActionButton.Click += (sender, e) => StartActivity(typeof(CartActivity));

            //Toolbar
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
        }

        private void ApplyUserSelectedFilters(IMenuItem item)
        {
            categoryFilter.OptionsItemSelected(item);
            var catalogItems = catalogService.FilterCatalogItems(categoryFilter.GetCategoriesSelected());

            adapter.UpdateCatalogItems(catalogItems);
        }

        private void CreateCategoryFilter(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.action_bar_menu, menu);
            var menuItem = menu.FindItem(Resource.Id.action_bar_menu_filter_list);

            categoryFilter = new CategoryFilter(menuItem);
            categoryFilter.AddResetCategory();
            foreach (var category in categoryService.GetCategories())
                categoryFilter.AddCategory(category);
        }

        private void HandleAddRemove(object sender, AddRemoveClickEventArgs e)
        {
            var vm = adapter.GetItem(e.Position);
            if (e.IsAdd)
                cartService.AddProduct(vm.Product, vm.Sale);
            else
                cartService.RemoveProduct(vm.Product);

            vm.Units = cartService.GetUnits(vm.Product);
            vm.DiscountPercent = cartService.GetDiscountPercent(vm.Product);
            adapter.NotifyItemChanged(e.Position);
            UpdateTotalPrice();
        }

        private void HandleFavoriteCheckedChange(object sender, FavoriteCheckedChangeEventArgs e)
        {
            ProductViewModel vm = adapter.GetItem(e.Position).Product;
            vm.IsFavorite = e.IsChecked;
            if (e.IsChecked)
                productService.AddToFavorites(vm);
            else
                productService.RemoveFromFavorites(vm);
        }

        private void InitCatalogRecyclerView()
        {
            var catalogItems = catalogService.GetCatalogItems();
            var recyclerView = FindViewById<RecyclerView>(Resource.Id.catalogItems);
            LinearLayoutManager layoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(layoutManager);
            adapter = new CatalogItemRecyclerViewAdapter(catalogItems, (action) => RunOnUiThread(action));
            recyclerView.SetAdapter(adapter);

            adapter.FavoriteCheckedChange += HandleFavoriteCheckedChange;
            adapter.AddRemoveClick += HandleAddRemove;
        }

        private void Startup()
        {
            RepositoryFactory.InjectProductRepositoryFactory(new Repositories.ProductRepository(this));
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("pt-BR");
        }

        private void UpdateTotalPrice()
        {
            var total = cartService.GetPriceToPay();
            buyActionButton.Text = $"COMPRAR R$ {total:n}";
        }
    }
}