using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Nostalginc.ApiCaller.Models
{
    public class ApiResponse
    {
        public bool Successful { get; set; }
        public bool Failed { get; set; }
        public string Content { get; init; }
        public HttpStatusCode HttpStatus { get; init; }
        public int HttpStatusCode { get; init; }
        public List<KeyValuePair<string, object>> ResponseHeaders { get; set; }
        public byte[] DownloadedData { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }
    }
}
