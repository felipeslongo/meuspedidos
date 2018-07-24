using MeusPedidos.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeusPedidos.Application.Apis
{
    internal class SaleClient : ApiClient
    {
        private const string url = "https://pastebin.com/raw/R9cJFBtG";

        //public IEnumerable<SaleViewModel> Get()
        //{
        //    var json = @"[
        //                    {
        //                        ""name"": ""Promoção Oi Me Liga"",
        //                        ""category_id"": 2,
        //                        ""policies"": [
        //                            {
        //                                ""min"": 1,
        //                                ""discount"": 10.0
        //                            }
        //                        ]
        //                    },
        //                    {
        //                        ""name"": ""Promoção #100porcentoselfie"",
        //                        ""category_id"": 5,
        //                        ""policies"": [
        //                            {
        //                                ""min"": 2,
        //                                ""discount"": 10.0
        //                            },
        //                            {
        //                                ""min"": 3,
        //                                ""discount"": 15.0
        //                            }
        //                        ]
        //                    },
        //                    {
        //                        ""name"": ""Promoção de Lavada"",
        //                        ""category_id"": 3,
        //                        ""policies"": [
        //                            {
        //                                ""min"": 10,
        //                                ""discount"": 10.0
        //                            },
        //                            {
        //                                ""min"": 20,
        //                                ""discount"": 20.0
        //                            },
        //                            {
        //                                ""min"": 30,
        //                                ""discount"": 30.0
        //                            }
        //                        ]
        //                    }
        //                ]";

        //    return JsonConvert.DeserializeObject<SaleJson[]>(json)
        //        .Select(jsonObj => new SaleViewModel()
        //        {
        //            CategoryId = jsonObj.category_id,
        //            Name = jsonObj.name,
        //            Policies = jsonObj.policies.Select(policyJsonObj => new SalePolicyViewModel()
        //            {
        //                Discount = policyJsonObj.discount,
        //                Minimum = policyJsonObj.min
        //            })
        //        });
        //}

        public IEnumerable<SaleViewModel> Get()
        {
            return FetchJson<SaleJson[]>(new Uri(url))
                .Select(jsonObj => new SaleViewModel()
                {
                    CategoryId = jsonObj.category_id,
                    Name = jsonObj.name,
                    Policies = jsonObj.policies.Select(policyJsonObj => new SalePolicyViewModel()
                    {
                        Discount = policyJsonObj.discount,
                        Minimum = policyJsonObj.min
                    })
                });
        }

        public class PolicyJson
        {
            public decimal discount { get; set; }
            public int min { get; set; }
        }

        public class SaleJson
        {
            public int category_id { get; set; }
            public string name { get; set; }
            public PolicyJson[] policies { get; set; }
        }
    }
}