using System.Collections.Generic;
using System.Threading.Tasks;
using WooliesX.Common.Domain.Models;
using WooliesX.Common.Domain.WebApi;

namespace WooliesX.WebApi.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> FindProductsAsync(SortOption sortOption);
    }
}
