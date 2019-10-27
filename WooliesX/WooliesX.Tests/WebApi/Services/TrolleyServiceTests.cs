using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WooliesX.Common.Domain.Models;
using WooliesX.Common.Extensions;
using WooliesX.Common.ExternalResources;
using WooliesX.WebApi.Services;
using Xunit;

namespace WooliesX.Tests.WebApi.Services
{
    public class TrolleyServiceTests
    {
        private readonly ITrolleyService _target;

        private readonly Fixture _fixture;

        private readonly Mock<IExerciseResourceApi> _exerciseResourceApi;

        public TrolleyServiceTests()
        {
            _fixture = new Fixture();
            _exerciseResourceApi = new Mock<IExerciseResourceApi>();
            var logger = new Mock<ILogger<TrolleyService>>();

            _target = new TrolleyService(_exerciseResourceApi.Object, logger.Object);
        }

        [Fact]
        public async void WhenFindLowestTrolleyTotalAsyncThenExerciseResourceApiIsInvoked()
        {
            var trolley = _fixture.Create<Trolley>();

            await _target.FindLowestTrolleyTotalAsync(trolley);

            _exerciseResourceApi
                .Verify(x =>
                        x.FindLowestTrolleyTotalAsync(trolley),
                    Times.Once);
        }

        [Fact]
        public void WhenCalculateLowestTrolleyTotalExpertThenResultIsCorrect()
        {
            const string trolleyJson = "{\"Products\":[{\"Name\":\"1\",\"Price\":2.0,\"Quantity\":0.0},{\"Name\":\"2\",\"Price\":5.0,\"Quantity\":0.0}],\"Specials\":[{\"Quantities\":[{\"Name\":\"1\",\"Quantity\":3},{\"Name\":\"2\",\"Quantity\":0}],\"Total\":5.0},{\"Quantities\":[{\"Name\":\"1\",\"Quantity\":1},{\"Name\":\"2\",\"Quantity\":2}],\"Total\":10.0}],\"Quantities\":[{\"Name\":\"1\",\"Quantity\":3},{\"Name\":\"2\",\"Quantity\":2}]}";
            var trolley = trolleyJson.FromJsonString<Trolley>();

            var actual = _target.CalculateLowestTrolleyTotalExpert(trolley);

            actual.Should().Be(14m);
        }
    }
}
