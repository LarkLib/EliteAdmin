using Infrastructure;
using Infrastructure.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace ZR.Admin.WebApi.Controllers
{
    public partial class BaseController
    {
        /// <summary>
        /// 返回成功封装
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeFormatStr"></param>
        /// <param name="isCamelCaseData"></param>
        /// <returns></returns>
        protected IActionResult SUCCESS(object data, bool isCamelCaseData, string timeFormatStr = "yyyy-MM-dd HH:mm:ss")
        {
            string jsonStr = GetJsonStr(GetApiResult(data != null ? ResultCode.SUCCESS : ResultCode.FAIL, isCamelCaseData, data), timeFormatStr, isCamelCaseData);
            return Content(jsonStr, "application/json");
        }
        private static string GetJsonStr(ApiResult apiResult, string timeFormatStr, bool isCamelCaseData)
        {
            if (string.IsNullOrEmpty(timeFormatStr))
            {
                timeFormatStr = TIME_FORMAT_FULL;
            }
            var serializerSettings = new JsonSerializerSettings
            {
                DateFormatString = timeFormatStr,
            };
            // 设置为驼峰命名
            if (isCamelCaseData) serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            return JsonConvert.SerializeObject(apiResult, Formatting.Indented, serializerSettings);
        }

        /// <summary>
        /// 全局Code使用
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="isCamelCaseData"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected ApiResult GetApiResult(ResultCode resultCode, bool isCamelCaseData, object? data = null)
        {
            var apiResult = new ApiResult((int)resultCode, resultCode.ToString());

            // 设置为驼峰命名
            if (!isCamelCaseData) apiResult.Data = JsonConvert.SerializeObject(data, Formatting.Indented);
            else apiResult.Data = data;

            return apiResult;
        }
    }
}