using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using System.Net;
using Serilog;

namespace StargateAPI.Controllers;

[ApiController]
[Route("people")]
public class PersonController : ControllerBase
{
    private readonly IMediator _mediator;
    public PersonController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetPeople()
    {
        try
        {
            var result = await _mediator.Send(new GetPeople() { });

            return this.GetResponse(result);
        }
        catch (BadHttpRequestException ex)
        {
            Log.Error("Error getting all People, {Message}", ex.Message);
            return this.GetResponse(new BaseResponse()
            {
                Message = ex.Message,
            }, ex.StatusCode);
        }
        catch (Exception ex)
        {
            Log.Error("Error getting all People, {Message}", ex.Message);
            return this.GetResponse(new BaseResponse()
            {
                Message = ex.Message,
            }, (int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetPersonByName(string name)
    {
        try
        {
            var result = await _mediator.Send(new GetPersonByName()
            {
                Name = name
            });

            return this.GetResponse(result);
        }
        catch (BadHttpRequestException ex)
        {
            Log.Error("Error getting Person {name}, {Message}", name, ex.Message);
            return this.GetResponse(new BaseResponse()
            {
                Message = ex.Message,
            }, ex.StatusCode);
        }
        catch (Exception ex)
        {
            Log.Error("Error getting Person {name}, {Message}", name, ex.Message);
            return this.GetResponse(new BaseResponse()
            {
                Message = ex.Message,
            }, (int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("")]
    public async Task<IActionResult> CreatePerson([FromBody] string name)
    {
        try
        {
            var result = await _mediator.Send(new CreatePerson()
            {
                Name = name
            });

            Log.Information("Created Person {Name}", name);
            // 201 is more appropriate here
            return this.GetResponse(result, (int)HttpStatusCode.Created);
        }
        catch (BadHttpRequestException ex)
        {
            Log.Error("Error creating Person {Name}, {Message}", name, ex.Message);
            return this.GetResponse(new BaseResponse()
            {
                Message = ex.Message,
            }, ex.StatusCode);
        }
        catch (Exception ex)
        {
            Log.Error("Error creating Person {Name}, {Message}", name, ex.Message);
            return this.GetResponse(new BaseResponse()
            {
                Message = ex.Message,
            }, (int)HttpStatusCode.InternalServerError);
        }
    }
}
