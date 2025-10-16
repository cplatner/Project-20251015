using Dapper;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;
using System.Net;
using StargateAPI.Business.Repositories;

namespace StargateAPI.Business.Commands;

public class CreateAstronautDuty : IRequest<CreateAstronautDutyResult>
{
    /// <summary>
    /// Name of the Person to assign this Astronaut Duty to
    /// </summary>
    /// <example>
    /// Dan Sherman
    /// </example>
    public required string Name { get; init; }

    public required string Rank { get; init; }

    public required string DutyTitle { get; init; }

    public DateTime DutyStartDate { get; init; }
}

public class CreateAstronautDutyPreProcessor : IRequestPreProcessor<CreateAstronautDuty>
{
    private readonly StargateContext _context;
    private readonly IPeopleRepository _peopleRepository;
    private readonly IAstronautDutyRepository _astronautDutyRepository;

    public CreateAstronautDutyPreProcessor(StargateContext context, IPeopleRepository  peopleRepository, IAstronautDutyRepository astronautDutyRepository)
    {
        _context = context;
        _peopleRepository = peopleRepository;
        _astronautDutyRepository = astronautDutyRepository;
    }

    public async Task Process(CreateAstronautDuty request, CancellationToken cancellationToken)
    {
        // @todo Move to repo
        // var person = _context.People.AsNoTracking().FirstOrDefault(z => z.Name == request.Name);
        var hasPerson = await _peopleRepository.HasPerson(request.Name, cancellationToken);
        
        if (hasPerson)
        {
            throw new BadHttpRequestException("Person not found", StatusCodes.Status404NotFound);
        }

        // var verifyNoPreviousDuty = _context.AstronautDuties.FirstOrDefault(z => z.DutyTitle == request.DutyTitle && z.DutyStartDate == request.DutyStartDate);
        var hasDuty = await _astronautDutyRepository.HasPreviousDuty(request, cancellationToken);
        if (hasDuty)
        {
            throw new BadHttpRequestException("Has conflicting Astronaut Duty", StatusCodes.Status400BadRequest);
        }
    }
}

public class CreateAstronautDutyHandler : IRequestHandler<CreateAstronautDuty, CreateAstronautDutyResult>
{
    private readonly StargateContext _context;

    public CreateAstronautDutyHandler(StargateContext context)
    {
        _context = context;
    }
    public async Task<CreateAstronautDutyResult> Handle(CreateAstronautDuty request, CancellationToken cancellationToken)
    {
// @todo move to repo

        var query = $"SELECT * FROM [Person] WHERE \'{request.Name}\' = Name";

        var person = await _context.Connection.QueryFirstOrDefaultAsync<Person>(query);

        query = $"SELECT * FROM [AstronautDetail] WHERE {person.Id} = PersonId";

        var astronautDetail = await _context.Connection.QueryFirstOrDefaultAsync<AstronautDetail>(query);

        if (astronautDetail == null)
        {
            astronautDetail = new AstronautDetail();
            astronautDetail.PersonId = person.Id;
            astronautDetail.CurrentDutyTitle = request.DutyTitle;
            astronautDetail.CurrentRank = request.Rank;
            astronautDetail.CareerStartDate = request.DutyStartDate.Date;
            if (request.DutyTitle == "RETIRED")
            {
                astronautDetail.CareerEndDate = request.DutyStartDate.Date;
            }

            await _context.AstronautDetails.AddAsync(astronautDetail);

        }
        else
        {
            astronautDetail.CurrentDutyTitle = request.DutyTitle;
            astronautDetail.CurrentRank = request.Rank;
            if (request.DutyTitle == "RETIRED")
            {
                astronautDetail.CareerEndDate = request.DutyStartDate.AddDays(-1).Date;
            }
            _context.AstronautDetails.Update(astronautDetail);
        }

        query = $"SELECT * FROM [AstronautDuty] WHERE {person.Id} = PersonId Order By DutyStartDate Desc";

        var astronautDuty = await _context.Connection.QueryFirstOrDefaultAsync<AstronautDuty>(query);

        if (astronautDuty != null)
        {
            astronautDuty.DutyEndDate = request.DutyStartDate.AddDays(-1).Date;
            _context.AstronautDuties.Update(astronautDuty);
        }

        var newAstronautDuty = new AstronautDuty()
        {
            PersonId = person.Id,
            Rank = request.Rank,
            DutyTitle = request.DutyTitle,
            DutyStartDate = request.DutyStartDate.Date,
            DutyEndDate = null
        };

        await _context.AstronautDuties.AddAsync(newAstronautDuty);

        await _context.SaveChangesAsync();

        return new CreateAstronautDutyResult()
        {
            Id = newAstronautDuty.Id
        };
    }
}

public class CreateAstronautDutyResult : BaseResponse
{
    public int? Id { get; set; }
}