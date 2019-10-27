using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WooliesX.Common.Domain.Models;
using WooliesX.Common.Domain.WebApi;
using WooliesX.Common.ExternalResources;
using WooliesX.WebApi.Services;
using Xunit;

namespace WooliesX.Tests.WebApi.Services
{
    public class ProductServiceTests
    {
        private readonly IProductService _target;

        private readonly Mock<IExerciseResourceApi> _exerciseResourceApi;

        public ProductServiceTests()
        {
            _exerciseResourceApi = new Mock<IExerciseResourceApi>();
            
            var logger = new Mock<ILogger<ProductService>>();

            _target = new ProductService(_exerciseResourceApi.Object, logger.Object);
        }
        
        [Fact]
        public async void GiveProductsNotAvailableWhenFindProductsAsyncShouldReturnNull()
        {
            _exerciseResourceApi
                .Setup(x => x.GetProductsAsync())
                .Returns((Task<IEnumerable<Product>>) null);
            
            var actual = await _target.FindProductsAsync(It.IsAny<SortOption>());

            actual.Should().BeNull();
        }

        [Theory]
        [ClassData(typeof(SortProductsScenarios))]
        public async void GiveProductsAvailableWhenFindProductsAsyncShouldSortProductsCorrectly(
            SortOption option,
            IEnumerable<Product> products,
            IEnumerable<ShopperHistory> shopperHistories,
            IEnumerable<Product> sortedProducts)
        {
            _exerciseResourceApi
                .Setup(x => x.GetProductsAsync())
                .ReturnsAsync(products);

            _exerciseResourceApi
                .Setup(x => x.GetShopperHistoriesAsync())
                .ReturnsAsync(shopperHistories);
            
            var actual = await _target.FindProductsAsync(option);

            actual.Should().BeEquivalentTo(sortedProducts);
        }
    }
}
