using DevOps.Domain;
using DevOps.Domain.Tests.Builders;
using Moq;
using Test;

namespace DevOps.AppLogic.Tests;

public class TeamServiceTests : TestBase
{
    private Mock<IDeveloperRepository> _developerRepositoryMock = null!;
    private TeamService _service = null!;

    [SetUp]
    public void Setup()
    {
        _developerRepositoryMock = new Mock<IDeveloperRepository>();
        _service = new TeamService(_developerRepositoryMock.Object);
    }

    [Test]
    public void AssembleDevelopersAsyncFor_EnoughDevelopersAvailable_ShouldRandomlyAddRequiredNumberOfDevelopers()
    {
        //Arrange
        var requiredNumberOfDevelopers = 2;
        var team = new TeamBuilder().Build();

        var freeDevelopers = new List<Developer>();
        for (var i = 0; i < requiredNumberOfDevelopers + 2; i++) freeDevelopers.Add(new DeveloperBuilder().WithoutTeam().Build());

        _developerRepositoryMock.Setup(repo => repo.FindDevelopersWithoutATeamAsync()).ReturnsAsync(freeDevelopers);

        //Act
        _service.AssembleDevelopersAsyncFor(team, requiredNumberOfDevelopers).Wait();

        //Assert
        _developerRepositoryMock.Verify(repo => repo.FindDevelopersWithoutATeamAsync(), Times.Once);
        Assert.That(team.Developers, Has.Count.EqualTo(requiredNumberOfDevelopers));
        Assert.That(team.Developers.All(d => freeDevelopers.Contains(d)));
        _developerRepositoryMock.Verify(repo => repo.CommitTrackedChangesAsync(), Times.Once);
    }

    [Test]
    public void AssembleDevelopersAsyncFor_NotEnoughDevelopersAvailable_ShouldAddAllAvailableDevelopers()
    {
        //Arrange
        var requiredNumberOfDevelopers = 5;
        var team = new TeamBuilder().Build();

        var numberOfAvailableDevelopers = requiredNumberOfDevelopers - 2;
        var freeDevelopers = new List<Developer>();
        for (var i = 0; i < numberOfAvailableDevelopers; i++) freeDevelopers.Add(new DeveloperBuilder().WithoutTeam().Build());

        _developerRepositoryMock.Setup(repo => repo.FindDevelopersWithoutATeamAsync()).ReturnsAsync(freeDevelopers);

        //Act
        _service.AssembleDevelopersAsyncFor(team, requiredNumberOfDevelopers).Wait();

        //Assert
        _developerRepositoryMock.Verify(repo => repo.FindDevelopersWithoutATeamAsync(), Times.Once);
        Assert.That(team.Developers, Has.Count.EqualTo(numberOfAvailableDevelopers));
        Assert.That(team.Developers.All(d => freeDevelopers.Contains(d)));
        _developerRepositoryMock.Verify(repo => repo.CommitTrackedChangesAsync(), Times.Once);
    }
}