using System.Threading.Tasks;
using WooliesX.Common.Domain.Models;

namespace WooliesX.WebApi.Services
{
    public interface ITrolleyService
    {
        Task<decimal?> FindLowestTrolleyTotalAsync(Trolley trolley);

        decimal? CalculateLowestTrolleyTotalExpert(Trolley trolley);
    }
}
