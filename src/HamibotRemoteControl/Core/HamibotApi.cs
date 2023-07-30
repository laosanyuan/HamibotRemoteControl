﻿using HamibotRemoteControl.Models;
using System.Text.Json;
using HamibotRemoteControl.Enums;

namespace HamibotRemoteControl.Core
{
    static class HamibotApi
    {
        private static readonly RestClient _client = new RestClient();
        private static readonly string _baseUrl = "https://api.hamibot.com";


        #region [Robots]
        /// <summary>
        /// 获取机器人列表
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Robot>> GetRobotList()
        {
            if (!string.IsNullOrEmpty(UserCenter.Instance.Token))
            {
                var url = $"{_baseUrl}/v1/robots";
                var response = await _client.SendRequest(url, HttpMethod.Get, UserCenter.Instance.Token);
                if (response is { IsSuccess: true })
                {
                    return JsonSerializer.Deserialize<RobotCollection>(response.Json)?.Items;
                }
            }
            return null;
        }

        /// <summary>
        /// 修改机器人信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static async Task<bool> ModifyRobot(string id, ModifyRobot info)
        {
            if (!string.IsNullOrEmpty(UserCenter.Instance.Token))
            {
                var url = $"{_baseUrl}/v1/robots/{id}";
                var response = await _client.SendRequest(url, HttpMethod.Put, UserCenter.Instance.Token);
                return response is { IsSuccess: true };
            }
            return false;
        }

        /// <summary>
        /// 停止机器人的所有正在运行的脚本
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<bool> StopRobotScripts(string id)
        {
            if (!string.IsNullOrEmpty(UserCenter.Instance.Token))
            {
                var url = $"{_baseUrl}/v1/robots/{id}/stop";
                var response = await _client.SendRequest(url, HttpMethod.Put, UserCenter.Instance.Token);
                return response is { IsSuccess: true };
            }
            return false;
        }
        #endregion

        #region [Scripts]
        /// <summary>
        /// 获取脚本列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static async Task<List<Script>> GetScriptList(ScriptType type = ScriptType.Common)
        {
            if (!string.IsNullOrEmpty(UserCenter.Instance.Token))
            {
                var parameter = type switch
                {
                    ScriptType.Developer => "devscripts",
                    _ => "scripts"
                };
                var url = $"{_baseUrl}/v1/{parameter}";

                var response = await _client.SendRequest(url, HttpMethod.Get, UserCenter.Instance.Token);
                if (response is { IsSuccess: true })
                {
                    var result = JsonSerializer.Deserialize<ScriptCollection>(response.Json)?.Items;
                    if (type == ScriptType.Developer)
                    {
                        result?.ForEach(t => t.Type = ScriptType.Developer);
                    }

                    return result;
                }
            }
            return default;
        }

        /// <summary>
        /// 运行/停止脚本
        /// </summary>
        /// <param name="id">脚本id</param>
        /// <param name="robots">被操作机器人</param>
        /// <param name="run">运行/停止</param>
        /// <param name="type">脚本类别</param>
        /// <returns></returns>
        public static async Task<bool> OperateScript(string id, List<BaseRobot> robots, bool run = true, ScriptType type = ScriptType.Common)
        {
            if (!string.IsNullOrEmpty(UserCenter.Instance.Token))
            {
                var tmpType = type switch
                {
                    ScriptType.Developer => "devscripts",
                    _ => "scripts"
                };
                var url = $"{_baseUrl}/v1/{tmpType}/{id}/run";

                var parameters = new Dictionary<string, List<BaseRobot>> { { "robots", robots } };

                var response = await _client.SendRequest(
                    url,
                    run ? HttpMethod.Post : HttpMethod.Delete,
                    UserCenter.Instance.Token,
                    JsonSerializer.Serialize(parameters));

                return response is { IsSuccess: true };
            }
            return false;
        }
        #endregion
    }
}
