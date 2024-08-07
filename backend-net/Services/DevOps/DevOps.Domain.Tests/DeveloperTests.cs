using Domain;
using Test;

namespace DevOps.Domain.Tests;

public class DeveloperTests : TestBase
{
    private string _id = null!;

    //About the = null!; -> We tell the compiler that this non-nullable reference type variable
    //will be initialized later and it is ok to be null here. This removes the compiler warning.
    private string _firstName = null!;
    private string _lastName = null!;
    private Percentage _rating = null!;

    [SetUp]
    public void BeforeEachTest()
    {
        _id = Random.NextString();
        _firstName = Random.NextString();
        _lastName = Random.NextString();
        _rating = Random.NextDouble();
    }

    [Test]
    public void CreateNew_ValidInput_ShouldInitializeFieldsCorrectly()
    {
        //Act
        var developer = Developer.CreateNew(_id, _firstName, _lastName, _rating);

        //Assert
        Assert.That(developer.Id, Is.EqualTo(_id));
        Assert.That(developer.FirstName, Is.EqualTo(_firstName));
        Assert.That(developer.LastName, Is.EqualTo(_lastName));
        Assert.That(developer.Rating, Is.EqualTo(_rating));
    }

    [TestCase(null)]
    [TestCase("")]
    public void CreateNew_EmptyId_ShouldThrowContractException(string emptyId)
    {
        Assert.That(() => Developer.CreateNew(emptyId, _firstName, _lastName, _rating), Throws.InstanceOf<ContractException>());
    }

    [TestCase(null)]
    [TestCase("")]
    public void CreateNew_EmptyFirstName_ShouldThrowContractException(string emptyFirstName)
    {
        Assert.That(() => Developer.CreateNew(_id, emptyFirstName, _lastName, _rating), Throws.InstanceOf<ContractException>());
    }

    [TestCase(null)]
    [TestCase("")]
    public void CreateNew_EmptyLastName_ShouldThrowContractException(string emptyLastName)
    {
        Assert.That(() => Developer.CreateNew(_id, _firstName, emptyLastName, _rating), Throws.InstanceOf<ContractException>());
    }
}