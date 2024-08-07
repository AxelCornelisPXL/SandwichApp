using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanResources.Domain;

public interface IEmployee
{
    [Key]
    [StringLength(11, MinimumLength = 1)]
    EmployeeNumber? Number { get; }

    [Required] string? LastName { get; }
    [Required] string? FirstName { get; }
    [Required] DateTime StartDate { get; }
    DateTime? EndDate { get; }

    void Dismiss(bool withNotice = true);
}