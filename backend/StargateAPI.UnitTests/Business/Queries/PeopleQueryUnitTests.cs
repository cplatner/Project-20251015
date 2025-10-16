using FluentAssertions;
using Moq;
using StargateAPI.Business.Dtos;
using StargateAPI.Business.Queries;
using StargateAPI.Business.Repositories;

namespace StargateAPI.UnitTests.Business.Queries;

public class PeopleQueryUnitTests
{
    [Fact]
    public async Task GetAll_3Items_Returns3()
    {
        // Arrange
        // var services = new ServiceCollection();
        // services.AddTransient<GetPeopleResult, GetPeopleResult>();
        // services.AddTransient<GetPeopleHandler, GetPeopleHandler>();

        var mockRepo = new Mock<IPeopleRepository>();
        var people = new List<PersonAstronaut>
        {
            new()
            {
                PersonId = 1,
                Name = "Dan Sherman",
                CurrentRank = "E7",
                CurrentDutyTitle = "Sergeant First Class",
                CareerStartDate = new DateTime(2016, 7, 2),
                CareerEndDate = new DateTime(2016, 7, 2)
            },
            new()
            {
                PersonId = 2,
                Name = "Alice Bell",
                CurrentRank = "E7",
                CurrentDutyTitle = "Sergeant First Class",
                CareerStartDate = new DateTime(2016, 7, 2)
            },
            new()
            {
                PersonId = 3,
                Name = "Steve Pierce"
            }
        };
        mockRepo.Setup(repo => repo.GetPeopleAstronauts(CancellationToken.None))
            .ReturnsAsync(people);

        // services.AddTransient<IPeopleRepository, PeopleRepository>(context =>
        // {
        // return (PeopleRepository)mockRepo.Object;
        // });
        // var serviceProvider = services.BuildServiceProvider();

        // var mediator = new Mediator(serviceProvider);
        // var controller = new PersonController(mediator);
        var handler = new GetPeopleHandler(mockRepo.Object);

        // Act
        var result = await handler.Handle(new GetPeople(), CancellationToken.None);

        // Assert
        result.Message.Should().BeNull();
        result.People.Should().BeEquivalentTo(new List<PersonAstronaut>
        {
            new()
            {
                PersonId = 1,
                Name = "Dan Sherman",
                CurrentRank = "E7",
                CurrentDutyTitle = "Sergeant First Class",
                CareerStartDate = new DateTime(2016, 7, 2),
                CareerEndDate = new DateTime(2016, 7, 2)
            },
            new()
            {
                PersonId = 2,
                Name = "Alice Bell",
                CurrentRank = "E7",
                CurrentDutyTitle = "Sergeant First Class",
                CareerStartDate = new DateTime(2016, 7, 2)
            },
            new()
            {
                PersonId = 3,
                Name = "Steve Pierce"
            }
        });
    }

    [Fact]
    public async Task GetAll_EmptyDatabase_ReturnsEmptyList()
    {
        // Arrange
        var mockRepo = new Mock<IPeopleRepository>();
        mockRepo.Setup(repo => repo.GetPeopleAstronauts(CancellationToken.None))
            .ReturnsAsync(new List<PersonAstronaut>());
        var handler = new GetPeopleHandler(mockRepo.Object);

        // Act
        var result = await handler.Handle(new GetPeople(), CancellationToken.None);

        // Assert
        result.Message.Should().BeNull();
        result.People.Should().BeEquivalentTo(new List<PersonAstronaut>());
    }
}