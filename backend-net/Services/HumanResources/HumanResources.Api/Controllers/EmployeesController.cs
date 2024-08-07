using AutoMapper;
using HumanResources.Api.Models;
using HumanResources.AppLogic;
using HumanResources.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HumanResources.Api.Controllers;

[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _repo;
    private readonly IEmployeeService _service;
    private readonly IMapper _mapper;

    public EmployeesController(IEmployeeRepository repo, IEmployeeService service, IMapper mapper)
    {
        _repo = repo;
        _service = service;
        _mapper = mapper;
    }

    [HttpGet("/employees/{number}")]
    public async Task<ActionResult> getEmployeeByNumber(string number)
    {
        var employee = await _repo.GetByNumberAsync(number);
        return employee == null ? NotFound() : Ok(_mapper.Map<EmployeeDetailModel>(employee));
    }

    [HttpPost("/employees")]
    [Authorize(policy:"write")]
    public async Task<IActionResult> Add(EmployeeCreateModel model)
    {
        var hiredEmployee = await _service.HireNewAsync(model.LastName, model.FirstName, model.StartDate);
        var outputModel = _mapper.Map<EmployeeDetailModel>(hiredEmployee);
        return CreatedAtAction(nameof(getEmployeeByNumber), new { number = outputModel.Number }, outputModel);
    }

    [HttpPost("/employees/{number}/dismiss")]
    public async Task<IActionResult> Dismiss(string number, [FromQuery] bool withNotice = true)
    {
        await _service.DismissAsync(number, withNotice);
        return Ok();
    }
}