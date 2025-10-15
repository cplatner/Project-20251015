using System.Text.Json.Serialization;

namespace StargateAPI.Controllers;

public class BaseResponse
{
    // This status is superfluous
    // public bool Success { get; set; } = true;
    
    // Only show messages when a request is not successful 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }
    
    // This response code is superfluous
    // public int ResponseCode { get; set; } = (int)HttpStatusCode.OK;
}