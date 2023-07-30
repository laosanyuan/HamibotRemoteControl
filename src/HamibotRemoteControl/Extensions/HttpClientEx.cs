#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace HamibotRemoteControl.Extensions
{
    internal static class HttpClientEx
    {
        /// <summary>
        /// 带Content的delete
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> DeleteAsync(
            this HttpClient client,
            [StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri,
            HttpContent? content)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUri)
            {
                Version = HttpVersion.Version11,
                VersionPolicy = HttpVersionPolicy.RequestVersionOrLower,
                Content = content
            };

            return await client.SendAsync(request, CancellationToken.None);
        }
    }
}
