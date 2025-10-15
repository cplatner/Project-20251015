using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace StargateAPI.Controllers;

public static class ControllerBaseExtensions
{
    public static IActionResult GetResponse(this ControllerBase controllerBase, BaseResponse response,
        int statusCode = (int)HttpStatusCode.OK)
    {
        var httpResponse = new ObjectResult(response);
        httpResponse.StatusCode = statusCode;
        return httpResponse;
    }
}