using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Snap.Genshin.WebApi.Utilities
{
    public static class ControllerExtension
    {
        public static IActionResult Success<T>(this ControllerBase _, string msg, T data)
        {
            return new JsonResult(new ApiResponse<T>(ApiCode.Success, msg, data));
        }

        public static IActionResult Success(this ControllerBase _, string msg)
        {
            return new JsonResult(new ApiResponse<object?>(ApiCode.Success, msg, null));
        }

        public static IActionResult Fail(this ControllerBase _, string msg)
        {
            return new JsonResult(new ApiResponse<object?>(ApiCode.Fail, msg, null));
        }

        public static IActionResult Fail(this ControllerBase _, ApiCode code, string msg)
        {
            return new JsonResult(new ApiResponse<object?>(code, msg, null));
        }
    }

    public class ApiResponse<T>
    {
        public ApiResponse(ApiCode code, string message, T data)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        [JsonPropertyName("retcode")]
        public ApiCode Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }

    public enum ApiCode
    {
        Success = 0,
        Fail = -1,
        // 数据库异常
        DbException = 101,
        // 服务冲突
        ServiceConcurrent = 102,
    }
}
