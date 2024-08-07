using Domain;
using HumanResources.Domain.Tests.Builders;
using Test;

namespace HumanResources.Domain.Tests;

public class EmployeeTests : TestBase
{
    [Test]
    public void Dismiss_WithoutNotice_ShouldSetEndDateOnToday()
    {
        //Arrange
        var employee = new EmployeeBuilder().WithEndDate(null).Build();

        //Act
        employee.Dismiss(false);

        //Assert
        Assert.That(employee.EndDate, Is.EqualTo(DateTime.Now).Within(10).Seconds);
    }

    [Test]
    public void Dismiss_WithoutNotice_EmployeeAlreadyHasEndDate_ShouldSetEndDateOnToday()
    {
        //Arrange
        var employee = new EmployeeBuilder().WithEndDate(DateTime.Now.AddDays(5)).Build();

        //Act
        employee.Dismiss(false);

        //Assert
        Assert.That(employee.EndDate, Is.EqualTo(DateTime.Now).Within(10).Seconds);
    }

    [Test]
    public void Dismiss_WithNotice_EmployeeAlreadyHasEndDate_ShouldThrowContractException()
    {
        //Arrange
        var employee = new EmployeeBuilder().WithEndDate(DateTime.Now.AddDays(5)).Build();

        //Act + Assert
        Assert.That(() => employee.Dismiss(true), Throws.InstanceOf<ContractException>());
    }

    [Test]
    public void Dismiss_WithNotice_LessThan3MonthsInService_ShouldSetEndDateInOneWeek()
    {
        //Arrange

        var lessThan3MonthsAgo = DateTime.Now.AddDays(-28);
        var employee = new EmployeeBuilder().WithStartDate(lessThan3MonthsAgo).WithEndDate(null).Build();

        //Act
        employee.Dismiss(true);

        //Assert
        var over1Week = DateTime.Now.AddDays(7);
        Assert.That(employee.EndDate, Is.EqualTo(over1Week).Within(10).Seconds);
    }

    [Test]
    public void Dismiss_WithNotice_LessThan12MonthsInService_ShouldSetEndDateIn2Weeks()
    {
        //Arrange
        var lessThan12MonthsAgo = DateTime.Now.AddMonths(-10);
        var employee = new EmployeeBuilder().WithStartDate(lessThan12MonthsAgo).WithEndDate(null).Build();

        //Act
        employee.Dismiss(true);

        //Assert
        var over2Weeks = DateTime.Now.AddDays(14);
        Assert.That(employee.EndDate, Is.EqualTo(over2Weeks).Within(10).Seconds);
    }

    [Test]
    public void Dismiss_WithNotice_MoreThan12MonthsInService_ShouldSetEndDateIn4Weeks()
    {
        //Arrange
        var moreThan12MonthsAgo = DateTime.Now.AddYears(-1);
        var employee = new EmployeeBuilder().WithStartDate(moreThan12MonthsAgo).WithEndDate(null).Build();

        //Act
        employee.Dismiss(true);

        //Assert
        var over4Weeks = DateTime.Now.AddDays(28);
        Assert.That(employee.EndDate, Is.EqualTo(over4Weeks).Within(10).Seconds);
    }
}