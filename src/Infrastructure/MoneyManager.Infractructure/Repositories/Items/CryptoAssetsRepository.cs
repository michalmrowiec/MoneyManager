using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Domain.Entities.CryptoAssets;

namespace MoneyManager.Infractructure.Repositories.Items
{
    internal class CryptoAssetsRepository : ItemRepositoryBase<CryptoAsset>, ICryptoAssetsRepository
    {
        public CryptoAssetsRepository(MoneyManagerContext moneyManagerContext) : base(moneyManagerContext)
        { }
    }
}
