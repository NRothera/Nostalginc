using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nostalginc.Controllers;
using Nostalginc.Models;
using Nostalginc.Repos.Interfaces;

namespace Nostalginc.Data.Repos
{
    public class CategoriesRepo : ICategoriesRepo
    {
        private readonly ILogger<CategoriesRepo> logger;
        private readonly NostalgincContext dbContext;

        public CategoriesRepo(ILogger<CategoriesRepo> logger, IApiCaller)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }
      
    }
}
