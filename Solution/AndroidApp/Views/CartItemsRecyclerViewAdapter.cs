using Android.Graphics.Drawables;
using Android.Support.V7.Widget;
using Android.Views;
using MeusPedidos.AndroidApp.Services;
using MeusPedidos.Application;
using System;
using System.Threading.Tasks;

namespace MeusPedidos.AndroidApp.Views
{
    public class CartItemsRecyclerViewAdapterClickEventArgs : EventArgs
    {
        public int Position { get; set; }
        public View View { get; set; }
    }

    public class CartItemsRecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        public CartItemsRecyclerViewAdapterViewHolder(View itemView, Action<CartItemsRecyclerViewAdapterClickEventArgs> clickListener,
                            Action<CartItemsRecyclerViewAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Name = ItemView.FindViewById<AppCompatTextView>(Resource.Id.name);
            Units = ItemView.FindViewById<AppCompatTextView>(Resource.Id.units);
            Price = ItemView.FindViewById<AppCompatTextView>(Resource.Id.price);
            Discount = ItemView.FindViewById<AppCompatTextView>(Resource.Id.discount);
            Photo = ItemView.FindViewById<AppCompatImageView>(Resource.Id.photo);

            itemView.Click += (sender, e) => clickListener(new CartItemsRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new CartItemsRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }

        public AppCompatTextView Discount { get; set; }
        public AppCompatTextView Name { get; set; }
        public AppCompatImageView Photo { get; set; }
        public AppCompatTextView Price { get; set; }
        public AppCompatTextView Units { get; set; }

        public void SetDiscount(decimal discount)
        {
            Discount.Text = $"{discount} %";
            Discount.Visibility = discount > 0 ? ViewStates.Visible : ViewStates.Invisible;
        }

        public void SetPrice(decimal price)
        {
            Price.Text = $"R$ {price:n}";
        }

        public void SetUnits(int units)
        {
            Units.Text = $"{units} UN";
        }

        internal void HydrateViews(CartItemViewModel cartItemVM)
        {
            Name.Text = cartItemVM.Name;
            SetDiscount(cartItemVM.Discount);
            SetPrice(cartItemVM.Price);
            SetUnits(cartItemVM.Units);
        }

        internal void SetPhoto(Uri uri, Drawable drawable)
        {
            Photo.SetImageDrawable(drawable);
        }
    }

    internal class CartItemsRecyclerViewAdapter : RecyclerView.Adapter
    {
        private CartItemViewModel[] items;

        /// <summary>
        /// Executes a action that runs on the ui thread.
        /// </summary>
        private Action<Action> runOnUIThread;

        public CartItemsRecyclerViewAdapter(CartItemViewModel[] data, Action<Action> runOnUIThread)
        {
            items = data;
            this.runOnUIThread = runOnUIThread;
        }

        public event EventHandler<CartItemsRecyclerViewAdapterClickEventArgs> ItemClick;

        public event EventHandler<CartItemsRecyclerViewAdapterClickEventArgs> ItemLongClick;

        public override int ItemCount => items.Length;

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var cartItemVM = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as CartItemsRecyclerViewAdapterViewHolder;
            holder.HydrateViews(cartItemVM);

            var imageService = new ImageLoadingService();
            Uri uri = new Uri(cartItemVM.PhotoUri);
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
            var id = Resource.Layout.cart_item;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new CartItemsRecyclerViewAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        private void OnClick(CartItemsRecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);

        private void OnLongClick(CartItemsRecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);
    }
}