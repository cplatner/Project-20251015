using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
            Log.Error("Error getting Astronaut Duty for Person {Name}, {Message}", name, ex.Message);
            return this.GetResponse(new BaseResponse
            {
                Message = ex.Message
            }, ex.StatusCode);
        }
        catch (Exception ex)
        {
            Log.Error("Error getting Astronaut Duty for Person {Name}, {Message}", name, ex.Message);
            return this.GetResponse(new BaseResponse
            {
                Message = ex.Message
            }, (int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateAstronautDuty([FromBody] CreateAstronautDuty request)
    {
        try
        {
            var result = await _mediator.Send(request);
            Log.Information("Created Astronaut Duty {DutyTitle} for Person {Name}", request.DutyTitle, request.Name);
            return this.GetResponse(result, (int)HttpStatusCode.Created);
        }
        catch (BadHttpRequestException ex)
        {
            Log.Error("Error creating Astronaut Duty {DutyTitle} for Person {Name}, {Message}", request.DutyTitle, request.Name, ex.Message);
            return this.GetResponse(new BaseResponse
            {
                Message = ex.Message
            }, ex.StatusCode);
        }
        catch (Exception ex)
        {
            Log.Error("Error creating Astronaut Duty {DutyTitle} for Person {Name}, {Message}", request.DutyTitle, request.Name, ex.Message);
            return this.GetResponse(new BaseResponse
            {
                Message = ex.Message
            }, (int)HttpStatusCode.InternalServerError);
        }
    }
}