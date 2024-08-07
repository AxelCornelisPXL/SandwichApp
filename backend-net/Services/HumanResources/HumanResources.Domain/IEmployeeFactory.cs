namespace HumanResources.Domain;

public interface IEmployeeFactory
{
    IEmployee CreateNew(string lastName, string firstName, DateTime start, int sequence);
}