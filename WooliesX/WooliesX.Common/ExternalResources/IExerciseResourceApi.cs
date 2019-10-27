using System.Collections.Generic;
using System.Threading.Tasks;
using WooliesX.Common.Domain.Models;

namespace WooliesX.Common.ExternalResources
{
    public interface IExerciseResourceApi
    {
        Task<IEnumerable<Product>> GetProductsAsync();

        Task<IEnumerable<ShopperHistory>> GetShopperHistoriesAsync();

        Task<decimal?> FindLowestTrolleyTotalAsync(Trolley trolley);
    }
}
