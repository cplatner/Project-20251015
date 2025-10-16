using Dapper;
using MediatR;
using MediatR.Pipeline;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using StargateAPI.Business.Dtos;
using StargateAPI.Business.Repositories;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Queries;

public class GetAstronautDutiesByName : IRequest<GetAstronautDutiesByNameResult>
{
    public required string Name { get; init; }
}

public class GetAstronautDutiesByNamePreProcessor : IRequestPreProcessor<GetAstronautDutiesByName>
{
    private readonly IPeopleRepository _peopleRepository;

    public GetAstronautDutiesByNamePreProcessor(IPeopleRepository peopleRepository)
    {
        _peopleRepository = peopleRepository;
    }
    
    public async Task Process(GetAstronautDutiesByName request, CancellationToken cancellationToken)
    {
        if (!await _peopleRepository.HasPerson(request.Name, cancellationToken))
        {
            throw new BadHttpRequestException("Person does not exist", StatusCodes.Status404NotFound);
        }
    }
}

public class GetAstronautDutiesByNameHandler : IRequestHandler<GetAstronautDutiesByName, GetAstronautDutiesByNameResult>
{
    private readonly IPeopleRepository _peopleRepository;
    private readonly IAstronautDutyRepository _astronautDutyRepository;

    public GetAstronautDutiesByNameHandler(IPeopleRepository peopleRepository, IAstronautDutyRepository astronautDutyRepository)
    {
        _peopleRepository = peopleRepository;
        _astronautDutyRepository = astronautDutyRepository;
    }

    public async Task<GetAstronautDutiesByNameResult> Handle(GetAstronautDutiesByName request, CancellationToken cancellationToken)
    {
        var person = await _peopleRepository.GetPersonAstronautByName(request.Name, cancellationToken);
        
        var duties = await _astronautDutyRepository.GetAstronautDuties(person.PersonId, cancellationToken);
        // @todo Add PersonAstronautDuty and mapper here
        var result = new GetAstronautDutiesByNameResult()
        {
            Person = person,
            AstronautDuties = duties,
        };

        return result;
    }
}

public class GetAstronautDutiesByNameResult : BaseResponse
{
    public required PersonAstronaut Person { get; init; }
    public required List<AstronautDuty> AstronautDuties { get; set; }
}