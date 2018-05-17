using Newtonsoft.Json;

namespace XamAppCenterSample2018.Models
{
    //{"error":{"code":400005,
    //"message":"The field Text must be a string or array type with a minimum length of '1'."}}

    [JsonObject("errorResult")]
    public class ErrorResult
    {
        [JsonProperty("error")]
        public Error Error { get; set; }
    }

    [JsonObject("error")]
    public class Error
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
