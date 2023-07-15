using FluentAssertions;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Domain.Entities.CryptoAssets;
using MoneyManager.Infractructure.Services.CryptocurrencyServices;
using Moq;
using System.Net;
using Xunit;

namespace MoneyManager.Infrastructure.UnitTests
{
    public class CryptoServiceTests
    {
        public static IEnumerable<object[]> CryptoSimpleDatas => new List<object[]>
        {
            new object[]
            {
                new string[] {"Bitcoin"}, // cryptocurrencies
                "USD", //
                "Bitcoin", // repo crypto name?
                new CryptocurrencySimpleData
                {
                    Id = 1,
                    Name = "Bitcoin",
                    Symbol = "BTC",
                    Price = 100_000m,
                    PricePercentChange24h = -2.87m,
                    PricePercentChange7d = -1.40m,
                    MarketCap = 4350254917.33m,
                    UpdateDate = new DateTime(2023, 6, 28, 17, 9, 32, 702)
                }, // repo response
                HttpStatusCode.OK, // repo response
                new List<CryptocurrencySimpleData>()
                {
                    new CryptocurrencySimpleData
                    {
                        Id = 1,
                        Name = "Bitcoin",
                        Symbol = "BTC",
                        Price = 100_000m,
                        PricePercentChange24h = -2.87m,
                        PricePercentChange7d = -1.40m,
                        MarketCap = 4350254917.33m,
                        UpdateDate = new DateTime(2023, 6, 28, 17, 9, 32, 702)
                    }
                }, // response from api
                new List<CryptocurrencySimpleData>()
                {
                    new CryptocurrencySimpleData
                    {
                        Id = 1,
                        Name = "Bitcoin",
                        Symbol = "BTC",
                        Price = 100_000m,
                        PricePercentChange24h = -2.87m,
                        PricePercentChange7d = -1.40m,
                        MarketCap = 4350254917.33m,
                        UpdateDate = new DateTime(2023, 6, 28, 17, 9, 32, 702)
                    }
                } // finally result
            }
        };

        [Theory]
        [MemberData(nameof(CryptoSimpleDatas))]
        public async void GetSimplePriceForCryptocurrencies_ForValidData_ReturnsCorrectData(string[] cryptocurrencies,
                                                 string currency,
                                                 string repositoryCryptoName,
                                                 CryptocurrencySimpleData repositoryResponse,
                                                 HttpStatusCode httpStatus,
                                                 List<CryptocurrencySimpleData> returnedCrytposDataFromApi,
                                                 List<CryptocurrencySimpleData> returnedAll)
        {
            var cryptoSimpleDatasRepository = new Mock<ICryptoSimpleDatasRepository>();
            cryptoSimpleDatasRepository.Setup(m => m.GetByNameAsync(repositoryCryptoName))
                .Returns(Task.FromResult<CryptocurrencySimpleData?>(repositoryResponse));

            var cryptoApiProvider = new Mock<ICryptoApiProvider>();
            cryptoApiProvider.Setup(m => m.GetCryptocurrenciesDataFromApi(1, currency))
                .Returns(Task.FromResult<(HttpStatusCode Status, List<CryptocurrencySimpleData> Value)>((httpStatus, returnedCrytposDataFromApi)));

            CryptoService cryptoService = new(cryptoSimpleDatasRepository.Object, cryptoApiProvider.Object);

            var (Status, Value) = await cryptoService.GetSimplePriceForCryptocurrencies(cryptocurrencies, currency);

            Status.Should().Be(Application.Functions.CryptoAssets.Queries.ApiResponseStatus.Ok);

            Value.Should().BeEquivalentTo(returnedAll);
        }
    }
}
