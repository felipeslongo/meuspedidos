using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MeusPedidos.Application;

namespace MeusPedidos.AndroidApp.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private const string key = "FavoritedProductsIds";
        private Context context;

        public ProductRepository(Context context)
        {
            this.context = context;
        }

        public void AddToFavorites(ProductViewModel vm)
        {
            var reader = OpenReader();
            var ids = reader.GetStringSet(key, new List<string>());
            //Note that you must not modify the set instance returned by this call.
            //The consistency of the stored data is not guaranteed if you do, nor is your ability to modify the instance at all.
            ids = new List<string>(ids);
            ids.Add(vm.Id.ToString());

            var editor = OpenEditor();
            editor.PutStringSet(key, ids);
            //editor.Apply();
            editor.Apply();
        }

        public bool IsFavorite(ProductViewModel vm)
        {
            var reader = OpenReader();
            var ids = reader.GetStringSet(key, new List<string>());
            return ids.Contains(vm.Id.ToString());
        }

        public void RemoveFromFavorites(ProductViewModel vm)
        {
            var reader = OpenReader();
            var ids = reader.GetStringSet(key, new List<string>());
            ids = new List<string>(ids);//
            ids.Remove(vm.Id.ToString());

            var editor = OpenEditor();
            editor.PutStringSet(key, ids);
            //editor.Apply();
            editor.Apply();
        }

        private ISharedPreferencesEditor OpenEditor() => OpenReader().Edit();

        private ISharedPreferences OpenReader()
        {
            //var reader = context.GetSharedPreferences("MyPrefsFile", FileCreationMode.Private);
            var reader = Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(context);
            return reader;
        }
    }
}