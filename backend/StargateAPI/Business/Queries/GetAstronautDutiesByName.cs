using Dapper;
using MediatR;
using StargateAPI.Business.Data;
using StargateAPI.Business.Dtos;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Queries;

public class GetAstronautDutiesByName : IRequest<GetAstronautDutiesByNameResult>
{
    public string Name { get; set; } = string.Empty;
}

public class GetAstronautDutiesByNameHandler : IRequestHandler<GetAstronautDutiesByName, GetAstronautDutiesByNameResult>
{
    private readonly StargateContext _context;

    public GetAstronautDutiesByNameHandler(StargateContext context)
    {
        _context = context;
    }

    public async Task<GetAstronautDutiesByNameResult> Handle(GetAstronautDutiesByName request, CancellationToken cancellationToken)
    {
        // @todo move to repo
        var person = await _context.Connection.QueryFirstOrDefaultAsync<PersonAstronaut>(
            $"""
             SELECT a.Id as PersonId, a.Name, b.CurrentRank, b.CurrentDutyTitle, b.CareerStartDate, b.CareerEndDate 
             FROM [Person] a 
             LEFT JOIN [AstronautDetail] b on b.PersonId = a.Id WHERE '{request.Name}' = a.Name
             """);
// @todo null check person
        var duties = await _context.Connection.QueryAsync<AstronautDuty>(
            $"""
             SELECT * 
             FROM [AstronautDuty] 
             WHERE {person.PersonId} = PersonId 
             ORDER BY DutyStartDate DESC
             """);

        var result = new GetAstronautDutiesByNameResult()
        {
            Person = person,
            AstronautDuties = duties.ToList(),
        };

        return result;
    }
}

public class GetAstronautDutiesByNameResult : BaseResponse
{
    public required PersonAstronaut Person { get; init; }
    public required List<AstronautDuty> AstronautDuties { get; set; }
}