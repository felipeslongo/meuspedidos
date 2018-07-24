using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using static Android.Widget.CompoundButton;
using MeusPedidos.Domain;
using System.Linq;
using System.Collections.Generic;
using MeusPedidos.Application;
using MeusPedidos.AndroidApp.Services;
using System.Threading.Tasks;
using Android.Graphics.Drawables;

namespace MeusPedidos.AndroidApp.Views
{
    public class AddRemoveClickEventArgs : CatalogItemRecyclerViewAdapterClickEventArgs
    {
        public bool IsAdd { get; set; }
        public bool IsRemove { get; set; }
    }

    public class CatalogItemRecyclerViewAdapterClickEventArgs : EventArgs
    {
        public int Position { get; set; }
        public View View { get; set; }
    }

    public class CatalogItemRecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        private readonly View itemView;

        public CatalogItemRecyclerViewAdapterViewHolder(View itemView,
                    Action<CatalogItemRecyclerViewAdapterClickEventArgs> clickListener,
            Action<CatalogItemRecyclerViewAdapterClickEventArgs> longClickListener,
            Action<FavoriteCheckedChangeEventArgs> favoriteCheckedChangeListener,
            Action<AddRemoveClickEventArgs> addRemoveClickListener)
            : base(itemView)
        {
            this.itemView = itemView;

            Sale = itemView.FindViewById<AppCompatTextView>(Resource.Id.sale);
            Photo = itemView.FindViewById<AppCompatImageView>(Resource.Id.photo);
            Name = itemView.FindViewById<AppCompatTextView>(Resource.Id.name);
            Discount = itemView.FindViewById<AppCompatTextView>(Resource.Id.discount);
            Price = itemView.FindViewById<AppCompatTextView>(Resource.Id.price);
            Units = itemView.FindViewById<AppCompatTextView>(Resource.Id.units);
            Favorite = itemView.FindViewById<ToggleButton>(Resource.Id.favorite);
            Add = itemView.FindViewById<AppCompatImageButton>(Resource.Id.add);
            Remove = itemView.FindViewById<AppCompatImageButton>(Resource.Id.remove);

            itemView.Click += (sender, e) => clickListener(new CatalogItemRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new CatalogItemRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });

            Favorite.CheckedChange += (sender, e) => favoriteCheckedChangeListener(new FavoriteCheckedChangeEventArgs(e.IsChecked) { View = itemView, Position = AdapterPosition });
            Add.Click += (sender, e) => addRemoveClickListener(new AddRemoveClickEventArgs { IsAdd = true, View = itemView, Position = AdapterPosition });
            Remove.Click += (sender, e) => addRemoveClickListener(new AddRemoveClickEventArgs { IsRemove = true, View = itemView, Position = AdapterPosition });
        }

        public AppCompatImageButton Add { get; }
        public AppCompatTextView Discount { get; }
        public ToggleButton Favorite { get; }
        public AppCompatTextView Name { get; }
        public AppCompatImageView Photo { get; }
        public AppCompatTextView Price { get; }
        public AppCompatImageButton Remove { get; }
        public AppCompatTextView Sale { get; }
        public AppCompatTextView Units { get; }

        internal void HydrateViews(CatalogItemViewModel vm)
        {
            Name.Text = vm.Product.Name;
            Price.Text = $"{vm.Product.Price:n}";
            Favorite.Checked = vm.Product.IsFavorite;
            Units.Text = vm.Units.ToString();
            SetDiscount(vm.DiscountPercent);

            Sale.Text = vm.Header;
            Sale.Visibility = vm.IsHeader ? ViewStates.Visible : ViewStates.Gone;
            //Photo.SetImageResource(Resource.Drawable.baseline_shopping_cart_black_48);
        }

        internal void SetPhoto(Uri uri, Drawable drawable)
        {
            Photo.SetImageDrawable(drawable);
        }

        private void SetDiscount(decimal discountPercent)
        {
            Discount.Text = $"{discountPercent}%";
            Discount.Visibility = discountPercent > 0 ? ViewStates.Visible : ViewStates.Invisible;
        }
    }

    public class FavoriteCheckedChangeEventArgs : CheckedChangeEventArgs
    {
        public FavoriteCheckedChangeEventArgs(bool isChecked) : base(isChecked)
        {
        }

        public int Position { get; set; }
        public View View { get; set; }
    }

    internal class CatalogItemRecyclerViewAdapter : RecyclerView.Adapter
    {
        private CatalogItemViewModel[] catalogItems;

        /// <summary>
        /// Executes a action that runs on the ui thread.
        /// </summary>
        private Action<Action> runOnUIThread;

        public CatalogItemRecyclerViewAdapter(IEnumerable<CatalogItemViewModel> catalogData, Action<Action> runOnUIThread)
        {
            catalogItems = catalogData.ToArray();
            this.runOnUIThread = runOnUIThread;
        }

        public event EventHandler<AddRemoveClickEventArgs> AddRemoveClick;

        public event EventHandler<FavoriteCheckedChangeEventArgs> FavoriteCheckedChange;

        public event EventHandler<CatalogItemRecyclerViewAdapterClickEventArgs> ItemClick;

        public event EventHandler<CatalogItemRecyclerViewAdapterClickEventArgs> ItemLongClick;

        public override int ItemCount => catalogItems.Count();

        public CatalogItemViewModel GetItem(int position)
        {
            return catalogItems[position];
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = GetItem(position);

            // Replace the contents of the view with that element
            var holder = viewHolder as CatalogItemRecyclerViewAdapterViewHolder;
            holder.HydrateViews(catalogItems[position]);

            //var imageService = new ImageLoadingService();
            //Uri uri = new Uri(item.Product.PhotoUrl);
            //new TaskFactory().StartNew(() => imageService.FetchDrawable(uri)).ContinueWith((task) => holder.SetPhoto(task.Result));

            var imageService = new ImageLoadingService();
            Uri uri = new Uri(item.Product.PhotoUrl);
            if (imageService.IsCached(uri))
                holder.SetPhoto(uri, imageService.FetchDrawable(uri));
            else
                //new TaskFactory().StartNew(() => imageService.FetchDrawable(uri)).ContinueWith((drawable) => NotifyItemChanged(position));
                new TaskFactory().StartNew(() => imageService.FetchDrawable(uri))
                    .ContinueWith((task) => runOnUIThread(() => holder.SetPhoto(uri, task.Result)));
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.catalog_item;
            itemView = LayoutInflater.From(parent.Context).
                Inflate(id, parent, false);

            var vh = new CatalogItemRecyclerViewAdapterViewHolder(itemView, OnClick, OnLongClick, OnFavoriteCheckedChange, OnAddRemoveClick);
            return vh;
        }

        internal void UpdateCatalogItems(IEnumerable<CatalogItemViewModel> enumerable)
        {
            catalogItems = enumerable.ToArray();
            NotifyDataSetChanged();
        }

        private void OnAddRemoveClick(AddRemoveClickEventArgs args) => AddRemoveClick?.Invoke(this, args);

        private void OnClick(CatalogItemRecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);

        private void OnFavoriteCheckedChange(FavoriteCheckedChangeEventArgs args) => FavoriteCheckedChange?.Invoke(this, args);

        private void OnLongClick(CatalogItemRecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);
    }
}