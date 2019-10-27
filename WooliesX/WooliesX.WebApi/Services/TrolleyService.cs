using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WooliesX.Common.Domain.Models;
using WooliesX.Common.Extensions;
using WooliesX.Common.ExternalResources;

namespace WooliesX.WebApi.Services
{
    public class TrolleyService : ITrolleyService
    {
        private readonly IExerciseResourceApi _exerciseResourceApi;

        private readonly ILogger<TrolleyService> _logger;

        public TrolleyService(
            IExerciseResourceApi exerciseResourceApi,
            ILogger<TrolleyService> logger)
        {
            _exerciseResourceApi = exerciseResourceApi;
            _logger = logger;
        }

        public Task<decimal?> FindLowestTrolleyTotalAsync(Trolley trolley)
        {
            _logger.LogInformation(trolley.ToJsonString());
            return _exerciseResourceApi.FindLowestTrolleyTotalAsync(trolley);
        }

        public decimal? CalculateLowestTrolleyTotalExpert(Trolley trolley)
        {
            if (trolley.Products == null || !trolley.Products.Any() ||
                trolley.Quantities == null || !trolley.Quantities.Any())
            {
                return 0m;
            }

            if (trolley.Specials == null || !trolley.Specials.Any())
            {
                var total = trolley.Quantities
                    .Sum(q => q.Quantity * trolley.Products.FirstOrDefault(p => p.Name == q.Name)?.Price);

                return total;
            }

            var possibleTotals = new List<decimal?>();
            
            foreach (var special in trolley.Specials)
            {
                var minNumOfSpecials = trolley.Quantities.Min(order =>
                {
                    var onSpecial = special.Quantities.FirstOrDefault(sq => sq.Name == order.Name);

                    if (onSpecial == null || onSpecial.Quantity == 0)
                    {
                        return int.MaxValue;
                    }

                    return order.Quantity / onSpecial.Quantity;
                });

                var specialTotal = minNumOfSpecials * special.Total;

                var remainedTotal = trolley.Quantities.Sum(order =>
                {
                    var onSpecialQuantity = special.Quantities.FirstOrDefault(sq => sq.Name == order.Name)?.Quantity ?? 0;
                    var price = trolley.Products.FirstOrDefault(p => p.Name == order.Name)?.Price;
                   
                    return (order.Quantity - minNumOfSpecials * onSpecialQuantity) * price;
                });

                possibleTotals.Add(specialTotal + remainedTotal);
            }

            return possibleTotals.Min();
        }
    }
}
