using System.ComponentModel.DataAnnotations;
using Domain;

namespace DevOps.Domain;

public class Team : Entity
{
    [Key] public Guid Id { get; private set; }
    public string Name { get; private set; }

    private readonly List<Developer> _developers;
    public IReadOnlyList<Developer> Developers => _developers;

    private Team(Guid id, string name)
    {
        Contracts.Require(!string.IsNullOrWhiteSpace(name), "name cannot be empty");

        _developers = new List<Developer>();
        Id = id;
        Name = name;
    }

    public static Team CreateNew(string name)
    {
        return new Team(Guid.NewGuid(), name);
    }

    public void Join(Developer developer)
    {
        if (_developers.Contains(developer))
            throw new ContractException("dev already in this team");

        _developers.Add(developer);
        developer.TeamId = Id;
    }

    protected override IEnumerable<object> GetIdComponents()
    {
        yield return Id;
    }
}