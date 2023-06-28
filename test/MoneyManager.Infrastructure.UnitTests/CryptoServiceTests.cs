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
                new string[] {"Bitcoin"},
                "USD",
                "Bitcoin",
                new CryptocurrencySimpleData
                {
                    Id = 1,
                    Name = "Bitcoin",
                    Symbol = "BTC",
                    Price = 100_000m,
                    PricePercentChange24h = -2.87m,
                    MarketCap = 4350254917.33m,
                    UpdateDate = new DateTime(2023, 6, 28, 17, 9, 32, 702)
                },
                HttpStatusCode.OK,
                new List<CryptocurrencySimpleData>()
                {
                    new CryptocurrencySimpleData
                    {
                        Id = 1,
                        Name = "Bitcoin",
                        Symbol = "BTC",
                        Price = 100_000m,
                        PricePercentChange24h = -2.87m,
                        MarketCap = 4350254917.33m,
                        UpdateDate = new DateTime(2023, 6, 28, 17, 9, 32, 702)
                    }
                },
                new List<CryptocurrencySimpleData>()
                {
                    new CryptocurrencySimpleData
                    {
                        Id = 1,
                        Name = "Bitcoin",
                        Symbol = "BTC",
                        Price = 100_000m,
                        PricePercentChange24h = -2.87m,
                        MarketCap = 4350254917.33m,
                        UpdateDate = new DateTime(2023, 6, 28, 17, 9, 32, 702)
                    }
                }
            }
        };

        [Theory]
        [MemberData(nameof(CryptoSimpleDatas))]
        public async void GetBasicCrytpoDataInfo(string[] cryptocurrencies,
                                                 string currency,
                                                 string repositoryCryptoName,
                                                 CryptocurrencySimpleData repositoryAnsewar,
                                                 HttpStatusCode httpStatus,
                                                 List<CryptocurrencySimpleData> returnedCrytposDataFromApi,
                                                 List<CryptocurrencySimpleData> returnedAll)
        {
            var cryptoSimpleDatasRepository = new Mock<ICryptoSimpleDatasRepository>();
            cryptoSimpleDatasRepository.Setup(m => m.GetByNameAsync(repositoryCryptoName))
                .Returns(Task.FromResult<CryptocurrencySimpleData?>(repositoryAnsewar));

            var cryptoApiProvider = new Mock<ICryptoApiProvider>();
            cryptoApiProvider.Setup(m => m.GetBasicCryptocurrenciesInfo(cryptocurrencies, currency))
                .Returns(Task.FromResult<(HttpStatusCode Status, List<CryptocurrencySimpleData> Value)>((httpStatus, returnedCrytposDataFromApi)));

            CryptoService cryptoService = new(cryptoSimpleDatasRepository.Object, cryptoApiProvider.Object);

            var (Status, Value) = await cryptoService.GetSimplePriceForCryptocurrencies(cryptocurrencies, currency);

            Status.Should().Be(Application.Functions.CryptoAssets.Queries.ApiResponseStatus.Ok);

            Value.Should().BeEquivalentTo(returnedAll);
        }
    }
}
