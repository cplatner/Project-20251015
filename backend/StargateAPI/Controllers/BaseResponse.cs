using System.Text.Json.Serialization;

namespace StargateAPI.Controllers;

public class BaseResponse
{
    // This status is superfluous.  I would include it only if a client _had_ to have it.
    // public bool Success { get; set; } = true;
    
    // Only show messages when a request is not successful 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }
    
    // This response code is superfluous.  I would include it only if a client _had_ to have it.
    // public int ResponseCode { get; set; } = (int)HttpStatusCode.OK;
}