using System.Text.RegularExpressions;

namespace HamibotRemoteControl.Tools
{
    internal static class Software
    {
        /// <summary>
        /// 当前版本号
        /// </summary>
        public static string Version => VersionTracking.CurrentVersion;

        /// <summary>
        /// 检查新版本
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> HaveNewVersion()
        {
            try
            {
                // 构建GitHub API的URL
                var apiUrl = "https://raw.githubusercontent.com/laosanyuan/HamibotRemoteControl/master/src/HamibotRemoteControl/HamibotRemoteControl.csproj";
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var pattern = @"<ApplicationDisplayVersion>(.*?)<\/ApplicationDisplayVersion>";

                    var match = Regex.Match(content, pattern);
                    if (match.Success)
                    {
                        return !match.Groups[1].Value.Equals(Version);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }
    }
}
