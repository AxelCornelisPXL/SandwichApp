using AutoMapper;
using DevOps.Api.Models;
using DevOps.AppLogic;
using DevOps.Domain;
using DevOps.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevOps.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TeamsController : ControllerBase
{
    private ITeamService _service;
    private ITeamRepository _repo;
    private IMapper _map;

    public TeamsController(ITeamService teamService, ITeamRepository teamRepository, IMapper mapper)
    {
        _service = teamService;
        _repo = teamRepository;
        _map = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _repo.GetAllAsync();
        IList<TeamDetailModel> models = new List<TeamDetailModel>();
        foreach (var team in list)
            models.Add(_map.Map<TeamDetailModel>(team));

        return models.Count == 0 ? NotFound() : Ok(models);
    }

    [HttpPost("{id}/assemble")]
    [Authorize(policy: "write")]
    public async Task<IActionResult> AssembleTeam(Guid id, TeamAssembleInputModel model)
    {
        var team = await _repo.GetByIdAsync(id);
        if (team is not null)
            await _service.AssembleDevelopersAsyncFor(team, model.RequiredNumberOfDevelopers);
        return team is null ? NotFound() : Ok();
    }
}