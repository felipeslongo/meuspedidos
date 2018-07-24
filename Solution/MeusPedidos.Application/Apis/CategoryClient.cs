using MeusPedidos.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeusPedidos.Application.Apis
{
    internal class CategoryClient : ApiClient
    {
        private const string url = "http://pastebin.com/raw/YNR2rsWe";

        //public IEnumerable<CategoryViewModel> Get()
        //{
        //    var json = @"[
        //                      {
        //                        ""id"": 1,
        //                        ""name"": ""Televisores""
        //                      },
        //                      {
        //                        ""id"": 2,
        //                        ""name"": ""Celulares""
        //                      },
        //                      {
        //                        ""id"": 3,
        //                        ""name"": ""Lava-roupas""
        //                      },
        //                      {
        //                        ""id"": 4,
        //                        ""name"": ""Notebooks""
        //                      },
        //                      {
        //                        ""id"": 5,
        //                        ""name"": ""Câmeras fotográficas""
        //                      }
        //                    ]";

        //    return JsonConvert.DeserializeObject<CategoryJson[]>(json)
        //        .Select(jsonObj => new MeusPedidos.Application.CategoryViewModel()
        //        {
        //            Id = jsonObj.id,
        //            Name = jsonObj.name
        //        });
        //}

        public IEnumerable<CategoryViewModel> Get()
        {
            return FetchJson<CategoryJson[]>(new Uri(url))
                .Select(jsonObj => new MeusPedidos.Application.CategoryViewModel()
                {
                    Id = jsonObj.id,
                    Name = jsonObj.name
                });
        }

        private class CategoryJson
        {
            public int id { get; set; }
            public string name { get; set; }
        }
    }
}