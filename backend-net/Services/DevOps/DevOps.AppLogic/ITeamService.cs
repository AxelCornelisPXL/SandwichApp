namespace DevOps.AppLogic;

using Domain;

public interface ITeamService
{
    Task AssembleDevelopersAsyncFor(Team team, int requiredNumberOfDevelopers);
}