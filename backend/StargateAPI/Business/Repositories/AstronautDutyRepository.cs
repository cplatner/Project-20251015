using Dapper;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;

namespace StargateAPI.Business.Repositories;

public interface IAstronautDutyRepository
{
    public Task<bool> HasPreviousDuty(CreateAstronautDuty request, CancellationToken cancellationToken);
    
    public Task<List<AstronautDuty>> GetAstronautDuties(int personId, CancellationToken cancellationToken);

    public Task CreateAstronautDetail(AstronautDetail astronautDetail, CancellationToken cancellationToken);
    
    public Task UpdateAstronautDetail(AstronautDetail astronautDetail, CancellationToken cancellationToken);

    public Task<AstronautDuty?> GetLastAstronautDuty(int personId, CancellationToken cancellationToken);

    public Task<AstronautDetail?> GetAstronautDetailByPersonId(int personId, CancellationToken cancellationToken);
}

public class AstronautDutyRepository : IAstronautDutyRepository
{
    private readonly StargateContext _context;

    public AstronautDutyRepository(StargateContext context)
    {
        _context = context;
    }

    public async Task<bool> HasPreviousDuty(CreateAstronautDuty request, CancellationToken cancellationToken)
    {
        var verifyNoPreviousDuty = await _context.AstronautDuties.FirstOrDefaultAsync(z =>
            z.DutyTitle == request.DutyTitle
            && z.Rank == request.Rank
            && z.DutyStartDate == request.DutyStartDate, cancellationToken);

        return verifyNoPreviousDuty is not null;
    }

    public async Task<List<AstronautDuty>> GetAstronautDuties(int personId, CancellationToken cancellationToken)
    {
        var duties = await _context.Connection.QueryAsync<AstronautDuty>(
            $"""
             SELECT * 
             FROM [AstronautDuty] 
             WHERE {personId} = PersonId 
             ORDER BY DutyStartDate DESC
             """, cancellationToken);

        return duties.ToList();
    }

    // @todo Create Duty
// public async Task<CreateAstronautDutyResult> Handle(CreateAstronautDuty request, CancellationToken cancellationToken)
//     {
// // @todo move to repo
//
//         var query = $"SELECT * FROM [Person] WHERE \'{request.Name}\' = Name";
//
//         var person = await _context.Connection.QueryFirstOrDefaultAsync<Person>(query);
//
//         query = $"SELECT * FROM [AstronautDetail] WHERE {person.Id} = PersonId";
//
//         var astronautDetail = await _context.Connection.QueryFirstOrDefaultAsync<AstronautDetail>(query);
//
public async Task  CreateAstronautDetail(AstronautDetail astronautDetail, CancellationToken cancellationToken)
{
        await _context.AstronautDetails.AddAsync(astronautDetail, cancellationToken);
 
        await _context.SaveChangesAsync(cancellationToken);
}

public async Task  UpdateAstronautDetail(AstronautDetail astronautDetail, CancellationToken cancellationToken)
{
     _context.AstronautDetails.Update(astronautDetail);
 
    await _context.SaveChangesAsync(cancellationToken);
}



//         if (astronautDetail == null)
//         {
//             astronautDetail = new AstronautDetail();
//             astronautDetail.PersonId = person.Id;
//             astronautDetail.CurrentDutyTitle = request.DutyTitle;
//             astronautDetail.CurrentRank = request.Rank;
//             astronautDetail.CareerStartDate = request.DutyStartDate.Date;
//             if (request.DutyTitle == "RETIRED")
//             {
//                 astronautDetail.CareerEndDate = request.DutyStartDate.Date;
//             }
//
//             await _context.AstronautDetails.AddAsync(astronautDetail);
//
//         }
//         else
//         {
//             astronautDetail.CurrentDutyTitle = request.DutyTitle;
//             astronautDetail.CurrentRank = request.Rank;
//             if (request.DutyTitle == "RETIRED")
//             {
//                 astronautDetail.CareerEndDate = request.DutyStartDate.AddDays(-1).Date;
//             }
//             _context.AstronautDetails.Update(astronautDetail);
//         }
//
//         query = $"SELECT * FROM [AstronautDuty] WHERE {person.Id} = PersonId Order By DutyStartDate Desc";

    public async Task<AstronautDuty?> GetLastAstronautDuty(int personId, CancellationToken cancellationToken)
    {
        // Let the database do the work here...
        var query = $"""
            SELECT * FROM [AstronautDuty] 
            WHERE {personId} = PersonId 
            ORDER BY DutyStartDate DESC
            LIMIT 1
            """;

        return await _context.Connection.QueryFirstOrDefaultAsync<AstronautDuty?>(query);
    }
    //
//         var astronautDuty = await _context.Connection.QueryFirstOrDefaultAsync<AstronautDuty>(query);
//
//         if (astronautDuty != null)
//         {
//             astronautDuty.DutyEndDate = request.DutyStartDate.AddDays(-1).Date;
//             _context.AstronautDuties.Update(astronautDuty);
//         }
//
//         var newAstronautDuty = new AstronautDuty()
//         {
//             PersonId = person.Id,
//             Rank = request.Rank,
//             DutyTitle = request.DutyTitle,
//             DutyStartDate = request.DutyStartDate.Date,
//             DutyEndDate = null
//         };
//
//         await _context.AstronautDuties.AddAsync(newAstronautDuty);
//
//         await _context.SaveChangesAsync();
//
//         return new CreateAstronautDutyResult()
//         {
//             Id = newAstronautDuty.Id
//         };
    // }

    // @todo: Move this to an AstronautDetailRepository
    public async Task<AstronautDetail?> GetAstronautDetailByPersonId(int personId, CancellationToken cancellationToken)
    {
        var query = $"SELECT * FROM [AstronautDetail] WHERE {personId} = PersonId";

        return await _context.Connection.QueryFirstOrDefaultAsync<AstronautDetail?>(query);
    }
}
