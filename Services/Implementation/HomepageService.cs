using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nostalginc.Models;
using Nostalginc.Repos.Interfaces;
using System.Net;
using Microsoft.AspNetCore.Http;
using Nostalginc.ApiCaller.Models;

namespace Nostalginc.Services.Implementation
{
    public class HomepageService
    {
        private ICategoriesRepo _repo { get; }

        public HomepageService(ICategoriesRepo repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<List<TopLevelCategories>>> GetListAsync()
        {
            var topLevelCategoriesList = _repo.GetTopLevelCategoriesList();


        }
    }
}
