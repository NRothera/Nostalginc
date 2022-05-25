using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nostalginc.Controllers;
using Nostalginc.Models;

namespace Nostalginc.Data.Repos
{
    public class CategoriesRepo
    {
        private readonly ILogger<CategoriesRepo> logger;
        private readonly NostalgincContext dbContext;

        public CategoriesRepo(ILogger<CategoriesRepo> logger, NostalgincContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }
        public void AddTopLevelCategory(TopLevelCategories category)
        {
            try
            {
                dbContext.Add(category);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError($"Failed to save changed: {e}");
            }
        }
    }
}
