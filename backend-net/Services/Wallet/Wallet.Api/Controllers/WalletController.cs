using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Api.Models;
using Wallet.AppLogic;
using Wallet.Infrastructure;

namespace Wallet.Api.Controllers;

public class WalletController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IWalletRepository _repo;
    private readonly IWalletService _service;

    public WalletController(IWalletRepository repo, IWalletService service, IMapper mapper)
    {
        _repo = repo;
        _service = service;
        _mapper = mapper;
    }

    [HttpGet("/wallet/{employeeNumber}")]
    public async Task<ActionResult> getWalletByEmployee(string employeeNumber)
    {
        var wallet = await _repo.getByEmployee(employeeNumber);
        return wallet == null ? NotFound() : Ok(_mapper.Map<WalletDetailModel>(wallet));
    }

    [HttpPost("/wallet")]
    [Authorize("write")]
    public async Task<IActionResult> Add(string employeeNumber)
    {
        var wallet = await _service.CreateNewWallet(employeeNumber);
        var outputWallet = _mapper.Map<WalletDetailModel>(wallet);
        return CreatedAtAction(nameof(getWalletByEmployee), new { employeeNumber = outputWallet.Owner }, outputWallet);
    }
}