using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WooliesX.Common.Domain.Models;
using WooliesX.Common.Extensions;
using static WooliesX.Common.ExternalResources.ResourceUrl;

namespace WooliesX.Common.ExternalResources
{
    public class ExerciseResourceApi : IExerciseResourceApi
    {
        private readonly UserProfile _userProfile;

        private readonly HttpClient _httpClient;

        private readonly ILogger<ExerciseResourceApi> _logger;

        public ExerciseResourceApi(
            HttpClient httpClient,
            IOptions<UserProfile> userProfile,
            ILogger<ExerciseResourceApi> logger)
        {
            _httpClient = httpClient;
            _userProfile = userProfile.Value;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            IEnumerable<Product> rv = null;

            var uri = $"{ProductResourceUri}{_userProfile.Token}";

            try
            {
                var response = await _httpClient.GetAsync(uri);

                response.EnsureSuccessStatusCode();

                rv = (await response.Content.ReadAsStringAsync()).FromJsonString<IEnumerable<Product>>();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to list products from {BaseAddress}{uri}. {e}");
            }

            return rv;
        }

        public async Task<IEnumerable<ShopperHistory>> GetShopperHistoriesAsync()
        {
            IEnumerable<ShopperHistory> rv = null;

            var uri = $"{ShopperHistoryUri}{_userProfile.Token}";

            try
            {
                var response = await _httpClient.GetAsync(uri);

                response.EnsureSuccessStatusCode();

                rv = (await response.Content.ReadAsStringAsync()).FromJsonString<IEnumerable<ShopperHistory>>();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to list shopper histories from {BaseAddress}{uri}. {e}");
            }

            return rv;
        }

        public async Task<decimal?> FindLowestTrolleyTotalAsync(Trolley trolley)
        {
            var uri = $"{TrolleyCalculatorUri}{_userProfile.Token}";

            try
            {
                var content = new StringContent(trolley.ToJsonString(), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(uri, content);

                response.EnsureSuccessStatusCode();

                var value = await response.Content.ReadAsStringAsync();

                if (decimal.TryParse(value, out var rv))
                {
                    return rv;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get the lowest trolley total from {BaseAddress}{uri}. {e}");
            }

            return null;
        }
    }
}