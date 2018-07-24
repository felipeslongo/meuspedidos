using Android.Views;
using MeusPedidos.Application;
using System.Collections.Generic;
using System.Linq;

namespace MeusPedidos.AndroidApp.Views
{
    internal class CategoryFilter
    {
        public const int AllCategories = 0;
        public const int GroupId = 1234;
        public List<CategoryViewModel> Categories = new List<CategoryViewModel>();

        public CategoryFilter(IMenuItem menuItem)
        {
            this.MenuItem = menuItem;
        }

        public IMenuItem MenuItem { get; }
        public ISubMenu SubMenu => MenuItem.SubMenu;

        public void AddCategory(CategoryViewModel category)
        {
            Categories.Add(category);
            var index = Categories.Count;

            SubMenu.Add(GroupId, category.Id, Menu.None, category.Name);
            var menuItem = SubMenu.GetItem(index);
            menuItem.SetCheckable(true);
            menuItem.SetChecked(false);
        }

        public void AddResetCategory()
        {
            SubMenu.Add(GroupId, AllCategories, Menu.None, Resource.String.filter_list_all_categories);
            SubMenu.GetItem(AllCategories).SetCheckable(true);
            SubMenu.GetItem(AllCategories).SetChecked(true);
        }

        public IEnumerable<CategoryViewModel> GetCategoriesSelected() => Categories.Where(category => SubMenu.GetItem(category.Id).IsChecked);

        internal CategoryViewModel GetCategoryById(int id) => Categories.First(vm => vm.Id == id);

        internal void OptionsItemSelected(IMenuItem item)
        {
            item.SetChecked(!item.IsChecked);
            if (!item.IsChecked)
                return;

            if (item.ItemId == AllCategories)
            {
                foreach (var category in Categories)
                    SubMenu.FindItem(category.Id).SetChecked(false);
                return;
            }

            GetAllCategoriesMenuItem().SetChecked(false);
        }

        private IMenuItem GetAllCategoriesMenuItem() => SubMenu.FindItem(AllCategories);
    }
}