using System.Text;
using HamibotRemoteControl.Common.Extensions;

namespace HamibotRemoteControl.Core
{
    class RestClient
    {
        private readonly HttpClient _httpClient;

        public RestClient()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// 发起请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="token"></param>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<RestClientResponse> SendRequest(string url, HttpMethod method, string token = null, string requestBody = null)
        {
            // 设置请求头
            if (!string.IsNullOrEmpty(token))
            {
                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                }
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            }

            // 设置请求体
            HttpContent httpContent = null;
            if (!string.IsNullOrEmpty(requestBody))
            {
                httpContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
            }

            // 发起请求
            HttpResponseMessage response = null;
            if (method == HttpMethod.Get)
            {
                response = await _httpClient.GetAsync(url);
            }
            else if (method == HttpMethod.Post)
            {
                response = await _httpClient.PostAsync(url, httpContent);
            }
            else if (method == HttpMethod.Put)
            {
                response = await _httpClient.PutAsync(url, httpContent);
            }
            else if (method == HttpMethod.Delete)
            {
                response = await _httpClient.DeleteAsync(url, httpContent);
            }

            // 处理响应
            if (response?.IsSuccessStatusCode == true)
            {
                var result = await response.Content.ReadAsStringAsync();

                return new RestClientResponse()
                {
                    IsSuccess = true,
                    Json = result
                };
            }

            return new RestClientResponse { IsSuccess = false };
        }
    }

    public class RestClientResponse
    {
        public bool IsSuccess { get; set; }
        public string Json { get; set; }
    }
}
