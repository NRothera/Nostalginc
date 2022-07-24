using Microsoft.AspNetCore.Mvc;
using Nostalginc.Data;
using Nostalginc.Data.Repos;
using Nostalginc.Models;
using Nostalginc.Services.Interfaces;

namespace Nostalginc.Controllers
{
    [ApiController]
    [Route("home")]
    public class TopLevelCategoryController : ControllerBase
    {
        private readonly ILogger<TopLevelCategoryController> logger;
        private readonly IHomepageService _service;
        public TopLevelCategoryController(ILogger<TopLevelCategoryController> logger, IHomepageService service)
        {
            _service = service;
            this.logger = logger;
        }


        [HttpGet]
        public Task<ActionResult> Get()
        {
            var responseModel = await _service.
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody] TopLevelCategories topLevelCategory)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("This category is invalid");
                }

                _categoriesRepo.AddTopLevelCategory(topLevelCategory);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                logger.LogError("Unable to create top level category");
                return StatusCode(StatusCodes.Status400BadRequest);
            }

        }

    }
}
