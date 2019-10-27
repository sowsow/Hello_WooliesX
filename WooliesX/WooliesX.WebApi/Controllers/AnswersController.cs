using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WooliesX.Common.Domain.Models;
using WooliesX.Common.Domain.WebApi;
using WooliesX.WebApi.Services;

namespace WooliesX.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly UserProfile _userProfile;

        private readonly IProductService _productService;

        private readonly ITrolleyService _trolleyService;

        public AnswersController(
            IOptions<UserProfile> options,
            IProductService productService,
            ITrolleyService trolleyService)
        {
            _userProfile = options.Value;
            _productService = productService;
            _trolleyService = trolleyService;
        }

        // GET api/answers/user
        [HttpGet("user")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<UserProfile> Get()
        {
            if (_userProfile == null)
            {
                return NotFound();
            }

            return Ok(_userProfile);
        }

        // GET api/answers/sort
        [HttpGet("sort")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Product>>> GetAsync([FromQuery] string sortOption)
        {
            IEnumerable<Product> result;
            
            if (Enum.TryParse<SortOption>(sortOption, true, out var option))
            {
                result = await _productService.FindProductsAsync(option);
            }
            else
            {
                return BadRequest($"Sorting option {sortOption} is not supported.");
            }

            if (result == null || !result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST api/answers/trolleyTotal
        [HttpPost("trolleyTotal")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<decimal>> PostAsync([FromBody] Trolley trolley)
        {
            //var result = await _trolleyService.FindLowestTrolleyTotalAsync(trolley);

            var result = await Task.Run(() => _trolleyService.CalculateLowestTrolleyTotalExpert(trolley));

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
