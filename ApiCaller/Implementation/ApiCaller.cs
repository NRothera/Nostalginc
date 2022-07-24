using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Nostalginc.ApiCaller.Models;
using Newtonsoft.Json;

namespace Nostalginc.ApiCaller.Implementation
{
    public interface IApiCaller
    {

    }

    public class ApiCaller
    {
        public HttpClient HttpClient { get; set; }
        public HttpRequestMessage Request { get; set; }
        private RetryPolicy _policy { get; set; }
        public bool _useRetryPolicy { get; set; }

        public ApiCaller()
        {
            _policy = Policies.RetryAllExceptUnauthorised();
            Initialise();
        }

        public ApiCaller(RetryPolicy policy)
        {
            _policy = policy;
        }

        private void Initialise()
        {
            HttpClient = new HttpClient();
            Request = new HttpRequestMessage();
        }

        public void Setup(string baseUrl, bool useRetryPolicy = true)
        {
            Request = new HttpRequestMessage(HttpMethod.Get, baseUrl.TrimEnd('/'));
        }

        public void Setup(string baseUrl, string resourceUrl, bool useRetryPolicy = true)
        {
            Request = new HttpRequestMessage(HttpMethod.Get, baseUrl.TrimEnd('/') + resourceUrl);
        }

        public void AddJsonBody(string bodyText)
        {
            if (bodyText == null)
            {
                throw new Exception("Body text is null");
            }

            Request.Content = new StringContent(bodyText, Encoding.UTF8, "application/json");
        }

        public void AddJsonBody(object model)
        {
            if (model == null)
            {
                throw new Exception("Unable to serialise a null model");
            }
            var bodyText = JsonConvert.SerializeObject(model);

            Request.Content = new StringContent(bodyText, Encoding.UTF8, "application/xml");
        }

        public void AddRequestHeaders(Dictionary<string, string> headers)
        {
            if (headers == null)
            {
                return;
            }

            foreach (var header in headers)
            {
                Request.Headers.Add(header.Key, header.Value);
            }
        }

        public void AddRequestHeaders(string key, string value)
        {
            Request.Headers.Add(key, value);
        }

        public void AddParameters(Dictionary<string, string> parameters)
        {
            Request.Content = new FormUrlEncodedContent(parameters);
        }

        public async Task<ApiResponse> ExecuteAsync(ApiRequest apiRequest)
        {
            var r = await AtomicExecute(apiRequest);

            return Map(r);
        }

        public async Task<ApiResponse> ExecuteAsync<T>(ApiRequest apiRequest)
        {
            var r = await AtomicExecute(apiRequest);

            return Map<T>(r);
        }

        private async Task<HttpResponseMessage> AtomicExecute(ApiRequest apiRequest)
        {
            var request = new HttpRequestMessage(apiRequest.HttpMethod,
                apiRequest.BaseUrl.TrimEnd('/') + "/" + apiRequest.ResourceUrl.TrimStart('/'));

            if (apiRequest.UrlParameters != null)
            {
                request.Content = new FormUrlEncodedContent(apiRequest.UrlParameters);
            }

            if (apiRequest.Headers != null)
            {
                foreach (var h in apiRequest.Headers)
                {
                    request.Headers.Add(h.Key, h.Value);
                }
            }

            if (apiRequest.RequestBody != null)
            {
                switch (apiRequest.RequestBodyType)
                {
                    case RequestBodyType.Json:
                        request.Content = new StringContent(apiRequest.RequestBody, Encoding.UTF8, "application/json");
                        break;
                    case RequestBodyType.Xml:
                        request.Content = new StringContent(apiRequest.RequestBody, Encoding.UTF8, "application/xml");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            HttpResponseMessage response = null;

            if (_useRetryPolicy)
            {

                await _policy.Execute(async () =>
                {
                    response = await HttpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                        return;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        throw new UnauthorizedAccessException(ToErrorMessage(response));

                    throw new ExternalException(ToErrorMessage(response));
                });
            }
            else
            {
                response = await HttpClient.SendAsync(request);
            }

            return response;
        }

        public ApiResponse Get()
        {
            var r = Execute(HttpMethod.Get);
            return Map(r);
        }

        public ApiResponse Post()
        {
            var r = Execute(HttpMethod.Post);
            return Map(r);
        }

        public ApiResponse Post<T>()
        {
            var r = Execute(HttpMethod.Post);
            return Map<T>(r);
        }

        public ApiResponse Patch()
        {
            var r = Execute(HttpMethod.Patch);
            return Map(r);
        }

        public ApiResponse Patch<T>()
        {
            var r = Execute(HttpMethod.Patch);
            return Map<T>(r);
        }

        public ApiResponse Put()
        {
            var r = Execute(HttpMethod.Put);
            return Map(r);
        }
        public ApiResponse Put<T>()
        {
            var r = Execute(HttpMethod.Put);
            return Map<T>(r);
        }
        public ApiResponse Delete()
        {
            var r = Execute(HttpMethod.Put);
            return Map(r);
        }

        public string ToErrorMessage(HttpResponseMessage r)
        {
            return
                $"{DateTime.Now}: Api response error: {Environment.NewLine}StatusCode: {r.StatusCode}{Environment.NewLine}Content:{r.Content}";
        }

        public string ToErrorMessage<T>(ApiResponse<T> r)
        {
            return
                $"{DateTime.Now}: Api response error: {Environment.NewLine}StatusCode: {r.HttpStatus}{Environment.NewLine}Content:{r.Content}";
        }

        private ApiResponse Map(HttpResponseMessage r)
        {
            var headerDict = new List<KeyValuePair<string, object>>();

            foreach (var h in r.Headers)
            {
                headerDict.Add(new KeyValuePair<string, object>(h.Key, string.Join(";", h.Value) ?? String.Empty));
            }

            var content = r.Content.ReadAsStringAsync().Result;

            var apiResponse = new ApiResponse
            {
                Content = content,
                HttpStatus = r.StatusCode,
                Successful = r.IsSuccessStatusCode,
                Failed = !r.IsSuccessStatusCode,
                ResponseHeaders = headerDict
            };

            return apiResponse;
        }

        private ApiResponse Map<T>(HttpResponseMessage r)
        {
            var headerDict = new List<KeyValuePair<string, object>>();

            foreach (var h in r.Headers)
            {
                headerDict.Add(new KeyValuePair<string, object>(h.Key, string.Join(";", h.Value) ?? String.Empty));
            }

            var content = r.Content.ReadAsStringAsync().Result;
            T? t;
            try
            {
                t = JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to deserialise type {typeof(T).Name}{Environment.NewLine}Content: {content}");
            }

            var apiResponse = new ApiResponse<T>
            {
                Content = content,
                HttpStatus = r.StatusCode,
                Successful = r.IsSuccessStatusCode,
                Failed = !r.IsSuccessStatusCode,
                ResponseHeaders = headerDict,
                Data = t
            };

            return apiResponse;
        }

        private HttpResponseMessage Execute(HttpMethod method)
        {
            Request.Method = method;

            var unique =
                $"{GetHashCode()}: {RandomNumberGenerator.GetInt32(1000)}:{Thread.CurrentThread.Name}:{Thread.CurrentThread.ManagedThreadId}";

            HttpResponseMessage response = null;

            if (_useRetryPolicy)
            {
                _policy.Execute(() =>
                {
                    response = HttpClient.Send(Request);

                    if (response.IsSuccessStatusCode)
                        return;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        throw new UnauthorizedAccessException(ToErrorMessage(response));

                    throw new ExternalException(ToErrorMessage(response));
                });
                
            }
            else
            {
                response = HttpClient.Send(Request);
            }

            return response;
        }
    }
  
}
