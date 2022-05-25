using Microsoft.AspNetCore.Mvc;
using Nostalginc.Data;
using Nostalginc.Data.Repos;
using Nostalginc.Models;

namespace Nostalginc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TopLevelCategoryController : ControllerBase
    {
        private readonly ILogger<TopLevelCategoryController> logger;
        private readonly CategoriesRepo _categoriesRepo;

        public TopLevelCategoryController(ILogger<TopLevelCategoryController> logger, NostalgincContext dbContext,
            CategoriesRepo categoriesRepo)
        {
            _categoriesRepo = categoriesRepo;
            this.logger = logger;
            _dbContext = dbContext;
        }

        private NostalgincContext _dbContext { get; }

        [HttpGet]
        public IEnumerable<TopLevelCategories> Get()
        {
            return _dbContext.TopLevelCategories.ToList();
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
