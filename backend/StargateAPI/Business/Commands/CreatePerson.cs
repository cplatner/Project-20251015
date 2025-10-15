using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Business.Repositories;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Commands;

public class CreatePerson : IRequest<CreatePersonResult>
{
    public required string Name { get; init; }
}

public class CreatePersonPreProcessor : IRequestPreProcessor<CreatePerson>
{
    private readonly IPeopleRepository _peopleRepository;

    public CreatePersonPreProcessor(IPeopleRepository peopleRepository)
    {
        _peopleRepository = peopleRepository;
    }
    
    public async Task Process(CreatePerson request, CancellationToken cancellationToken)
    {
        if (await _peopleRepository.HasPerson(request.Name, cancellationToken))
        {
            throw new BadHttpRequestException("Cannot create duplicate person", StatusCodes.Status400BadRequest);
        }
    }
}

public class CreatePersonHandler : IRequestHandler<CreatePerson, CreatePersonResult>
{
    private readonly StargateContext _context;
    private readonly IPeopleRepository _peopleRepository;

    public CreatePersonHandler(StargateContext context, IPeopleRepository peopleRepository)
    {
        _context = context;
        _peopleRepository = peopleRepository;
    }
    
    public async Task<CreatePersonResult> Handle(CreatePerson request, CancellationToken cancellationToken)
    {
        var newPerson = new Person()
        {
            Name = request.Name
        };

        await _peopleRepository.CreatePerson(newPerson, cancellationToken);
        
        return new CreatePersonResult()
        {
            Id = newPerson.Id
        };
    }
}

public class CreatePersonResult : BaseResponse
{
    public int Id { get; set; }
}
