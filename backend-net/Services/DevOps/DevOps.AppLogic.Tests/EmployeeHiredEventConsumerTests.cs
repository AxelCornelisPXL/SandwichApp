using DevOps.AppLogic.Events;
using DevOps.AppLogic.Tests.Builders;
using DevOps.Domain;
using DevOps.Domain.Tests.Builders;
using IntegrationEvents.Employee;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Test;

namespace DevOps.AppLogic.Tests;

public class EmployeeHiredEventConsumerTests : TestBase
{
    private Mock<IDeveloperRepository> _developerRepositoryMock = null!;
    private EmployeeHiredEventConsumer _consumer = null!;

    [SetUp]
    public void Setup()
    {
        _developerRepositoryMock = new Mock<IDeveloperRepository>();
        var _loggerMock = new Mock<ILogger<EmployeeHiredEventConsumer>>();

        _consumer = new EmployeeHiredEventConsumer(_developerRepositoryMock.Object, _loggerMock.Object);
    }

    [Test]
    public void Consume_DeveloperWithSameIdAlreadyExists_ShouldDoNothing()
    {
        //Arrange
        var existingDeveloper = new DeveloperBuilder().Build();
        _developerRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(existingDeveloper);
        var @event = new EmployeeHiredIntegrationEventBuilder().Build();

        //Act
        _consumer.Consume(GetContextForEvent(@event)).Wait();

        //Assert
        _developerRepositoryMock.Verify(repo => repo.GetByIdAsync(@event.Number), Times.Once);
        _developerRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Developer>()), Times.Never);
    }

    [Test]
    public void Consume_DeveloperDoesNotExistYet_ShouldAddDeveloper()
    {
        //Arrange
        _developerRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(() => null);
        var @event = new EmployeeHiredIntegrationEventBuilder().Build();

        //Act
        _consumer.Consume(GetContextForEvent(@event)).Wait();

        //Assert
        _developerRepositoryMock.Verify(repo => repo.GetByIdAsync(@event.Number), Times.Once);
        _developerRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Developer>(d => d.Id == @event.Number && d.FirstName == @event.FirstName && d.LastName == @event.LastName && d.Rating == 0.0)), Times.Once);
    }

    private ConsumeContext<EmployeeHiredIntegrationEvent> GetContextForEvent(EmployeeHiredIntegrationEvent @event)
    {
        var consumeContextMock = new Mock<ConsumeContext<EmployeeHiredIntegrationEvent>>();
        consumeContextMock.SetupGet(c => c.Message).Returns(@event);
        return consumeContextMock.Object;
    }
}