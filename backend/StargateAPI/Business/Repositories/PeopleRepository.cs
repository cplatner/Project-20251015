using Dapper;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Business.Dtos;

namespace StargateAPI.Business.Repositories;

public interface IPeopleRepository
{
    public Task<bool> HasPerson(string name, CancellationToken cancellationToken);

    public Task<List<PersonAstronaut>> GetPersonAstronauts(CancellationToken cancellationToken);

    public Task<Person> CreatePerson(string name, CancellationToken cancellationToken);
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

    public async Task<List<PersonAstronaut>> GetPersonAstronauts(CancellationToken cancellationToken = default)
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

    public async Task<Person> CreatePerson(string name, CancellationToken cancellationToken)
    {
        var newPerson = new Person
        {
            Name = name
        };

        await _context.People.AddAsync(newPerson, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return newPerson;
    }
}