using HumanResources.Domain;
using IntegrationEvents.Employee;
using MassTransit;
using Moq;
using Test;

namespace HumanResources.AppLogic.Tests;

public class EmployeeServiceTests : TestBase
{
    private Mock<IEmployeeFactory> _employeeFactoryMock;
    private Mock<IEmployeeRepository> _employeeRepositoryMock;
    private Mock<IPublishEndpoint> _publishMock;

    private EmployeeService _service;

    [SetUp]
    public void Setup()
    {
        _employeeFactoryMock = new Mock<IEmployeeFactory>();
        _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        _publishMock = new Mock<IPublishEndpoint>();

        _service = new EmployeeService(_employeeRepositoryMock.Object, _employeeFactoryMock.Object, _publishMock.Object);
    }

    [Test]
    public void HireNewAsync_Should_RetrieveNumberOfStarters_CreateTheEmployee_AndSaveIt()
    {
        //Arrange
        var lastName = Random.NextString();
        var firstName = Random.NextString();
        var startDate = DateTime.Now;

        var numberOfStartersOnStartDate = Random.Next(1, 1000);
        _employeeRepositoryMock.Setup(repo => repo.GetNumberOfStartersOnAsync(It.IsAny<DateTime>())).ReturnsAsync(numberOfStartersOnStartDate);

        var createdEmployeeMock = new Mock<IEmployee>();
        createdEmployeeMock.SetupGet(e => e.Number).Returns(new EmployeeNumber(startDate, 1));
        createdEmployeeMock.SetupGet(e => e.FirstName).Returns(firstName);
        createdEmployeeMock.SetupGet(e => e.LastName).Returns(lastName);
        var createdEmployee = createdEmployeeMock.Object;

        _employeeFactoryMock.Setup(factory => factory.CreateNew(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(createdEmployee);

        //Act
        var result = _service.HireNewAsync(lastName, firstName, startDate).Result;

        //Assert
        _employeeRepositoryMock.Verify(repo => repo.GetNumberOfStartersOnAsync(startDate), Times.Once);
        var expectedSequence = numberOfStartersOnStartDate + 1;
        _employeeFactoryMock.Verify(factory => factory.CreateNew(lastName, firstName, startDate, expectedSequence), Times.Once);
        _employeeRepositoryMock.Verify(repo => repo.AddAsync(createdEmployee), Times.Once);

        _publishMock.Verify(bus => bus.Publish(It.Is<EmployeeHiredIntegrationEvent>(@event => @event.Number == createdEmployee.Number && @event.FirstName == firstName && @event.LastName == lastName), CancellationToken.None), Times.Once);

        Assert.That(result, Is.SameAs(createdEmployee));
    }

    [Test]
    public void DismissAsync_Should_RetrieveEmployeeFromRepository_DismissTheEmployee_AndSaveTheChanges()
    {
        //Arrange
        var employeeNumber = new EmployeeNumber(DateTime.Now, 1);

        Mock<IEmployee> employeeToDismissMock = new();
        var employeeToDismiss = employeeToDismissMock.Object;

        _employeeRepositoryMock.Setup(repo => repo.GetByNumberAsync(It.IsAny<EmployeeNumber>())).ReturnsAsync(employeeToDismiss);

        //Act
        _service.DismissAsync(employeeNumber, true).Wait();

        //Assert
        _employeeRepositoryMock.Verify(repo => repo.GetByNumberAsync(employeeNumber), Times.Once);
        employeeToDismissMock.Verify(e => e.Dismiss(true), Times.Once);
        _employeeRepositoryMock.Verify(repo => repo.CommitTrackedChangesAsync(), Times.Once);
    }
}