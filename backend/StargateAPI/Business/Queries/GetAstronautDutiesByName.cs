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
        // @todo move to repo
//         var person = await _context.Connection.QueryFirstOrDefaultAsync<PersonAstronaut>(
//             $"""
//              SELECT a.Id as PersonId, a.Name, b.CurrentRank, b.CurrentDutyTitle, b.CareerStartDate, b.CareerEndDate 
//              FROM [Person] a 
//              LEFT JOIN [AstronautDetail] b on b.PersonId = a.Id WHERE '{request.Name}' = a.Name
//              """);
        var person = await _peopleRepository.GetPersonByName(request.Name, cancellationToken);
        
//         var duties = await _context.Connection.QueryAsync<AstronautDuty>(
//             $"""
//              SELECT * 
//              FROM [AstronautDuty] 
//              WHERE {person.PersonId} = PersonId 
//              ORDER BY DutyStartDate DESC
//              """);
        var duties = await _astronautDutyRepository.GetAstronautDuties(person.PersonId, cancellationToken);
        
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