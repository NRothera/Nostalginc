using Microsoft.AspNetCore.Mvc;
using Nostalginc.Data;
using Nostalginc.Models;

namespace Nostalginc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TopLevelCategoryController : ControllerBase
    {
        public TopLevelCategoryController(NostalgincContext dbContext)
        {
            _dbContext = dbContext;
        }

        private NostalgincContext _dbContext { get; }

        [HttpGet]
        public IEnumerable<TopLevelCategories> Get()
        {
            return _dbContext.TopLevelCategories.ToList();
        }

        [HttpPost]
        public void Post()
        {

        }

    }
}
