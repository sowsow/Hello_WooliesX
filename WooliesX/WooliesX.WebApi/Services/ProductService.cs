using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WooliesX.Common.Domain.Models;
using WooliesX.Common.Domain.WebApi;
using WooliesX.Common.ExternalResources;

namespace WooliesX.WebApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IExerciseResourceApi _exerciseResourceApi;

        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IExerciseResourceApi exerciseResourceApi,
            ILogger<ProductService> logger)
        {
            _exerciseResourceApi = exerciseResourceApi;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> FindProductsAsync(SortOption option)
        {
            try
            {
                var products = await _exerciseResourceApi.GetProductsAsync();

                if (products == null)
                {
                    return null;
                }

                switch (option)
                {
                    case SortOption.Low:
                        return products.OrderBy(x => x.Price).ToList();

                    case SortOption.High:
                        return products.OrderByDescending(x => x.Price).ToList();

                    case SortOption.Ascending:
                        return products.OrderBy(x => x.Name).ToList();

                    case SortOption.Descending:
                        return products.OrderByDescending(x => x.Name).ToList();

                    case SortOption.Recommended:
                        var shopperHistories = (await _exerciseResourceApi.GetShopperHistoriesAsync())?.ToList();
                     
                        var productByPopularityDesc = products.Select(
                                p => new
                                {
                                    Product = p,
                                    Popularity = shopperHistories?
                                        .Count(sh => sh.Products.Exists(shp => shp.Name == p.Name)),
                                    HistoricalQuantity = shopperHistories?
                                        .SelectMany(sh => sh.Products)
                                        .Where(shp => shp.Name == p.Name)
                                        .Sum(shp => shp.Quantity),
                                })
                            .OrderByDescending(x => x.Popularity)
                            .ThenByDescending(x => x.HistoricalQuantity)
                            .Select(x => x.Product);
                        
                        return productByPopularityDesc.ToList();

                    default:
                        return null;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Unable to get product with sort option {option}. {e}");

                return null;
            }
        }
    }
}