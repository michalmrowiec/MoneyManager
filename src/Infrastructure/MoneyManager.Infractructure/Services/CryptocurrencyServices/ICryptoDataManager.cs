namespace MoneyManager.Infractructure.Services.CryptocurrencyServices
{
    internal interface ICryptoDataManager
    {
        Task CheckDataFreshness();
        Task UpdateCryptoDataInDb();
    }
}
