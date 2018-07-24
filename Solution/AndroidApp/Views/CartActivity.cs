using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MeusPedidos.Application;
using MeusPedidos.Application.Services;

namespace MeusPedidos.AndroidApp.Views
{
    [Activity(
        //Name = "AndroidApp.Views.CartActivity",
        Label = "Carrinho")]
    public class CartActivity : AppCompatActivity
    {
        private CartItemsRecyclerViewAdapter adapter;
        private CartService cartService = ServiceFactory.CreateCartService();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_cart);

            // data to populate the RecyclerView with
            var cartItens = cartService.GetCartItems();
            var recyclerView = FindViewById<RecyclerView>(Resource.Id.cartItems);
            recyclerView.SetLayoutManager(new LinearLayoutManager(this));
            adapter = new CartItemsRecyclerViewAdapter(cartItens.ToArray(), (action) => RunOnUiThread(action));
            recyclerView.SetAdapter(adapter);

            var cashOutButton = FindViewById<AppCompatButton>(Resource.Id.bottomActionButton);
            cashOutButton.Click += FinishCashOut;

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            toolbar.NavigationClick += (sender, e) => Finish();

            InitTotals();
        }

        private void FinishCashOut(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Compra Finalizada", ToastLength.Long).Show();
        }

        private void InitTotals()
        {
            var units = FindViewById<AppCompatTextView>(Resource.Id.unitsTotal);
            var priceTotal = FindViewById<AppCompatTextView>(Resource.Id.priceTotal);

            priceTotal.Text = $"R$ {cartService.GetPriceToPay():n}";
            units.Text = $"{cartService.GetUnits()} UN";
        }
    }
}