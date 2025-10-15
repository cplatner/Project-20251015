using MediatR;
using StargateAPI.Business.Dtos;
using StargateAPI.Business.Repositories;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Queries;

public class GetPeople : IRequest<GetPeopleResult>
{
}

public class GetPeopleHandler : IRequestHandler<GetPeople, GetPeopleResult>
{
    private readonly IPeopleRepository _repository;

    public GetPeopleHandler(IPeopleRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetPeopleResult> Handle(GetPeople request, CancellationToken cancellationToken)
    {
        var result = new GetPeopleResult();

        result.People = await _repository.GetPeopleAstronauts(cancellationToken);

        return result;
    }
}

public class GetPeopleResult : BaseResponse
{
    public List<PersonAstronaut> People { get; set; } = [];
}