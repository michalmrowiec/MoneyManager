using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.Domain.Entities.CryptoAssets;

namespace MoneyManager.Infractructure.Repositories.Items
{
    internal class CryptoAssetsRepository : ItemRepositoryBase<CryptoAsset>
    {
        public CryptoAssetsRepository(MoneyManagerContext moneyManagerContext) : base(moneyManagerContext)
        { }
    }
}
