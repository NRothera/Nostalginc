using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nostalginc.ApiCaller.Models
{
    public class ApiRequest
    {
        public string BaseUrl { get; set; }
        public string ResourceUrl { get; set; }
        public string RequestBody { get; set; }
        public RequestBodyType RequestBodyType { get; set; } = RequestBodyType.Json;
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> UrlParameters { get; set; }
        public HttpMethod HttpMethod { get; set; }
    }
}
