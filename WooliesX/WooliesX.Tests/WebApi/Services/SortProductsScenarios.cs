using System.Collections;
using System.Collections.Generic;
using WooliesX.Common.Domain.Models;
using WooliesX.Common.Domain.WebApi;

namespace WooliesX.Tests.WebApi.Services
{
    public class SortProductsScenarios : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                SortOption.Low,
                new[]
                {
                    new Product {Name = "B", Price = 2m, Quantity = 2m},
                    new Product {Name = "A", Price = 1m, Quantity = 1m}
                },
                null,
                new[]
                {
                    new Product {Name = "A", Price = 1m, Quantity = 1m},
                    new Product {Name = "B", Price = 2m, Quantity = 2m}
                }
            };

            yield return new object[]
            {
                SortOption.High,
                new[]
                {
                    new Product {Name = "A", Price = 1m, Quantity = 1m},
                    new Product {Name = "B", Price = 2m, Quantity = 2m}
                },
                null,
                new[]
                {
                    new Product {Name = "B", Price = 2m, Quantity = 2m},
                    new Product {Name = "A", Price = 1m, Quantity = 1m}
                }
            };

            yield return new object[]
            {
                SortOption.Ascending,
                new[]
                {
                    new Product {Name = "B", Price = 2m, Quantity = 2m},
                    new Product {Name = "A", Price = 1m, Quantity = 1m}
                },
                null,
                new[]
                {
                    new Product {Name = "A", Price = 1m, Quantity = 1m},
                    new Product {Name = "B", Price = 2m, Quantity = 2m}
                }
            };

            yield return new object[]
            {
                SortOption.Descending,
                new[]
                {
                    new Product {Name = "A", Price = 1m, Quantity = 1m},
                    new Product {Name = "B", Price = 2m, Quantity = 2m}
                },
                null,
                new[]
                {
                    new Product {Name = "B", Price = 2m, Quantity = 2m},
                    new Product {Name = "A", Price = 1m, Quantity = 1m}
                }
            };

            yield return new object[]
            {
                SortOption.Recommended,
                new[]
                {
                    new Product {Name = "A", Price = 1m, Quantity = 1m},
                    new Product {Name = "B", Price = 2m, Quantity = 2m},
                    new Product {Name = "C", Price = 3m, Quantity = 3m}
                },
                new[]
                {
                    new ShopperHistory
                    {
                        CustomerId = "1",
                        Products = new List<Product>
                        {
                            new Product {Name = "A", Price = 1m, Quantity = 4m},
                            new Product {Name = "B", Price = 2m, Quantity = 5m}
                        }
                    },
                    new ShopperHistory
                    {
                        CustomerId = "2",
                        Products = new List<Product>
                        {
                            new Product {Name = "B", Price = 2m, Quantity = 6m},
                            new Product {Name = "C", Price = 3m, Quantity = 7m}
                        }
                    }
                },
                new[]
                {
                    new Product {Name = "B", Price = 2m, Quantity = 2m},
                    new Product {Name = "C", Price = 3m, Quantity = 3m},
                    new Product {Name = "A", Price = 1m, Quantity = 1m}
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
