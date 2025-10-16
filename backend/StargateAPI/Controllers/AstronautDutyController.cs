using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace StargateAPI.Controllers;

[ApiController]
[Route("astronaut-duties")]
public class AstronautDutyController : ControllerBase
{
    private readonly IMediator _mediator;

    public AstronautDutyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{name}")]
    [SwaggerResponse(200, "List of Astronaut Duties for a Person", typeof(GetAstronautDutiesByNameResult))]
    [SwaggerResponse(404, "The Person was not found")]
    public async Task<IActionResult> GetAstronautDutiesByName(string name)
    {
        try
        {
            var result = await _mediator.Send(new GetAstronautDutiesByName
            {
                Name = name
            });

            return this.GetResponse(result);
        }
        catch (BadHttpRequestException ex)
        {
            return this.GetResponse(new BaseResponse
            {
                Message = ex.Message
            }, ex.StatusCode);
        }
        catch (Exception ex)
        {
            return this.GetResponse(new BaseResponse
            {
                Message = ex.Message
            }, (int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateAstronautDuty([FromBody] CreateAstronautDuty request)
    {
        var result = await _mediator.Send(request);
        return this.GetResponse(result, (int)HttpStatusCode.Created);
    }
}