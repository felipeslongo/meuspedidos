using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MeusPedidos.AndroidApp.Services
{
    public class ImageLoadingService
    {
        private static Dictionary<string, Drawable> cache = new Dictionary<string, Drawable>();

        public Drawable FetchDrawable(Uri url)
        {
            if (cache.ContainsKey(url.AbsoluteUri))
                return cache[url.AbsoluteUri];

            var data = new System.Net.WebClient().OpenRead(url);
            var drawable = Drawable.CreateFromStream(data, "src");
            cache[url.AbsoluteUri] = drawable;
            return drawable;
        }

        public async Task<Drawable> FetchDrawableAsync(Uri url)
        {
            return await new TaskFactory<Drawable>().StartNew(() => FetchDrawable(url));

            if (cache.ContainsKey(url.AbsoluteUri))
                return cache[url.AbsoluteUri];

            var data = await new System.Net.WebClient().OpenReadTaskAsync(url);
            var drawable = Drawable.CreateFromStream(data, "src");
            return drawable;
        }

        internal bool IsCached(Uri uri) => cache.ContainsKey(uri.AbsoluteUri);
    }
}