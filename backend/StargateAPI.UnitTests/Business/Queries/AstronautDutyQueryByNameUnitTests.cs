using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using StargateAPI.Business.Dtos;
using StargateAPI.Business.Queries;
using StargateAPI.Business.Repositories;

namespace StargateAPI.UnitTests.Business.Queries;

public class AstronautDutyQueryByNameUnitTests
{
#if false
    [Fact]
    public async Task GetByName_3ItemsInDb_NameFound_Returns1()
    {
        // Arrange
        var mockPeopleRepo = new Mock<IPeopleRepository>();
        var mockDutyRepo = new Mock<IAstronautDutyRepository>();
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
        mockPeopleRepo.Setup(repo => repo.GetPeopleAstronauts(CancellationToken.None))
            .ReturnsAsync(people);
        mockDutyRepo.Setup(repo => repo.GetPeopleAstronauts(CancellationToken.None))
            .ReturnsAsync(people);

        var handler = new GetPersonByNameHandler(mockRepo.Object);

        // Act
        var result = await handler.Handle(new GetPersonByName() { Name = "Dan Sherman"}, CancellationToken.None);

        // Assert
        result.Message.Should().BeNull();
        result.Should().BeEquivalentTo(new PersonAstronaut()
            {
                PersonId = 1,
                Name = "Dan Sherman",
                CurrentRank = "E7",
                CurrentDutyTitle = "Sergeant First Class",
                CareerStartDate = new DateTime(2016, 7, 2),
                CareerEndDate = new DateTime(2016, 7, 2)
            
        });
    }
#endif
}