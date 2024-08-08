using AutoMapper;
using Wallet.Domain;

namespace Wallet.Api.Models;

public class WalletDetailModel
{
    public string Owner { get; set; } = string.Empty;
    public decimal Coins { get; set; }

    public IList<Transaction> Transactions { get; set; } =
        new List<Transaction>(); //TODO maybe this needs a detail model too. investigate upon error

    private class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Wallet, WalletDetailModel>();
        }
    }
}