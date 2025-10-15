using Dapper;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Business.Dtos;

namespace StargateAPI.Business.Repositories;

public interface IPeopleRepository
{
    public Task<bool> HasPerson(string name, CancellationToken cancellationToken);

    public Task<List<PersonAstronaut>> GetPeopleAstronauts(CancellationToken cancellationToken);

    public Task<PersonAstronaut> GetPersonByName(string name, CancellationToken cancellationToken);

    public Task<Person> CreatePerson(Person newPerson, CancellationToken cancellationToken);
}

public sealed class PeopleRepository : IPeopleRepository
{
    private readonly StargateContext _context;

    public PeopleRepository(StargateContext context)
    {
        _context = context;
    }

    public async Task<bool> HasPerson(string name, CancellationToken cancellationToken)
    {
        var person = await _context.People.AsNoTracking().FirstOrDefaultAsync(z => z.Name == name, cancellationToken);

        return person is not null;
    }

    public async Task<List<PersonAstronaut>> GetPeopleAstronauts(CancellationToken cancellationToken = default)
    {
        var query =
            """
            SELECT a.Id as PersonId, a.Name, b.CurrentRank, b.CurrentDutyTitle, b.CareerStartDate, b.CareerEndDate
            FROM [Person] a
            LEFT JOIN [AstronautDetail] b ON b.PersonId = a.Id
            """;

        var people = await _context.Connection.QueryAsync<PersonAstronaut>(query, cancellationToken);

        return people.ToList();
    }

    public async Task<PersonAstronaut?> GetPersonByName(string name, CancellationToken cancellationToken)
    {
        var query =
            $"""
            SELECT a.Id as PersonId, a.Name, b.CurrentRank, b.CurrentDutyTitle, b.CareerStartDate, b.CareerEndDate
            FROM [Person] a
            LEFT JOIN [AstronautDetail] b on b.PersonId = a.Id
            WHERE '{name}' = a.Name
            """;

        var person = await _context.Connection.QueryAsync<PersonAstronaut>(query, cancellationToken);

        return person?.FirstOrDefault();
    }

    public async Task<Person> CreatePerson(Person newPerson, CancellationToken cancellationToken)
    {
        await _context.People.AddAsync(newPerson, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return newPerson;
    }
}