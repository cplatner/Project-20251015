using MediatR;
using StargateAPI.Business.Dtos;
using StargateAPI.Business.Repositories;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Queries;

public class GetPersonByName : IRequest<GetPersonByNameResult>
{
    public required string Name { get; init; }
}

public class GetPersonByNameHandler : IRequestHandler<GetPersonByName, GetPersonByNameResult>
{
    private readonly IPeopleRepository _repository;

    public GetPersonByNameHandler(IPeopleRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetPersonByNameResult> Handle(GetPersonByName request, CancellationToken cancellationToken)
    {
        var person = await _repository.GetPersonAstronautByName(request.Name, cancellationToken);
        
        if (person is null)
        {
            throw new BadHttpRequestException("Person not found", StatusCodes.Status404NotFound);
        }

        return new GetPersonByNameResult()
        {
            Person = person
        };
    }
}

public class GetPersonByNameResult : BaseResponse
{
    public required PersonAstronaut Person { get; init; }
}