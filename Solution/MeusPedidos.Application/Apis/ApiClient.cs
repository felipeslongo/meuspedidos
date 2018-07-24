using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MeusPedidos.Application.Apis
{
    /// <summary>
    /// Classe base para os clientes de API externa
    /// </summary>
    internal abstract class ApiClient
    {
        public static Dictionary<string, string> cache = new Dictionary<string, string>();

        public T FetchJson<T>(Uri uri)
        {
            if (cache.ContainsKey(uri.AbsoluteUri))
                return JsonConvert.DeserializeObject<T>(cache[uri.AbsoluteUri]);

            var json = new System.Net.WebClient().DownloadString(uri);
            cache[uri.AbsoluteUri] = json;
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}