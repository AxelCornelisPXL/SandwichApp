namespace IntegrationEvents.Employee;

public class EmployeeHiredIntegrationEvent : IntegrationEventBase
{
    public string Number { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
}