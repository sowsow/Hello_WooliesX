using System;
using System.Collections.Generic;
using System.Net;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using WooliesX.Common.Domain.Models;
using WooliesX.Common.Domain.WebApi;
using WooliesX.WebApi.Controllers;
using WooliesX.WebApi.Services;
using Xunit;

namespace WooliesX.Tests.WebApi.Controllers
{
    public class AnswersControllerTests
    {
        private readonly AnswersController _target;

        private readonly Fixture _fixture;

        private readonly Mock<IProductService> _productService;

        private readonly Mock<ITrolleyService> _trolleyService;

        public AnswersControllerTests()
        {
            _fixture = new Fixture();

            var userProfileOption = new Mock<IOptions<UserProfile>>();
            _productService = new Mock<IProductService>();
            _trolleyService = new Mock<ITrolleyService>();

            userProfileOption.Setup(x => x.Value)
                .Returns(_fixture.Create<UserProfile>());

            _target = new AnswersController(
                userProfileOption.Object,
                _productService.Object,
                _trolleyService.Object);
        }

        [Fact]
        public void WhenUserProfileIsAvailableThenReturnOk()
        {
           var actual = _target.Get();
            
           (actual.Result as StatusCodeResult)?
               .StatusCode.Should()
               .Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async void GivenValidSortOptionWhenProductsAvailableThenGetAsyncReturnOk()
        {
            var sortOption = _fixture.Create<SortOption>();
            var products = _fixture.Create<IEnumerable<Product>>();

            _productService
                .Setup(x => x.FindProductsAsync(sortOption))
                .ReturnsAsync(products);
            
            var actual = await _target.GetAsync(sortOption.ToString());

            (actual.Result as StatusCodeResult)?
                .StatusCode.Should()
                .Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async void GivenInValidSortOptionThenGetAsyncReturnBadRequest()
        {
            var sortOption = Guid.NewGuid();

            var actual = await _target.GetAsync(sortOption.ToString());

            (actual.Result as StatusCodeResult)?
                .StatusCode.Should()
                .Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void GivenValidSortOptionWhenProductsNotAvailableThenGetAsyncReturnNotFound()
        {
            var sortOption = _fixture.Create<SortOption>();
         
            _productService
                .Setup(x => x.FindProductsAsync(sortOption))
                .ReturnsAsync((IEnumerable<Product>) null);

            var actual = await _target.GetAsync(sortOption.ToString());

            (actual.Result as StatusCodeResult)?
                .StatusCode.Should()
                .Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async void WhenLowestTrolleyTotalAvailableThenPostAsyncReturnOk()
        {
            var trolley = _fixture.Create<Trolley>();
            var lowestTotal = _fixture.Create<decimal>();

           _trolleyService
                .Setup(x => x.FindLowestTrolleyTotalAsync(trolley))
                .ReturnsAsync(lowestTotal);

           _trolleyService
               .Setup(x => x.CalculateLowestTrolleyTotalExpert(trolley))
               .Returns(lowestTotal);

            var actual = await _target.PostAsync(trolley);

            (actual.Result as StatusCodeResult)?
                .StatusCode.Should()
                .Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async void WhenLowestTrolleyTotalNotAvailableThenPostAsyncReturnOk()
        {
            var trolley = _fixture.Create<Trolley>();

            _trolleyService
                .Setup(x => x.FindLowestTrolleyTotalAsync(trolley))
                .ReturnsAsync((decimal?) null);

            var actual = await _target.PostAsync(trolley);

            (actual.Result as StatusCodeResult)?
                .StatusCode.Should()
                .Be((int)HttpStatusCode.NotFound);
        }
    }
}
