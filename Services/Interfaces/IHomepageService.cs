using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nostalginc.ApiCaller.Models;
using Nostalginc.Models;

namespace Nostalginc.Services.Interfaces
{
    public interface IHomepageService
    {
        Task<ApiResponse<List<TopLevelCategories>>> GetListAsync();
    }
}
