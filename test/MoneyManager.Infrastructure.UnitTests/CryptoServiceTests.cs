using FluentAssertions;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Application.Functions.CryptoAssets.Queries;
using MoneyManager.Domain.Entities.CryptoAssets;
using MoneyManager.Infractructure.Services.CryptocurrencyServices;
using Moq;
using System.Net;
using Xunit;

namespace MoneyManager.Infrastructure.UnitTests
{
    public class CryptoServiceTests
    {
        public static IEnumerable<object[]> CryptoTestData => new List<object[]>
        {
            new object[]
            {
                new string[] { "Bitcoin", "Ethereum", "Tether" }, // cryptocurrencies
                "USD", //
                new List<CryptocurrencySimpleData>()
                {
                    new CryptocurrencySimpleData
                    {
                        Id = 1,
                        Name = "Bitcoin",
                        Symbol = "BTC",
                        Price = 103_432.34m,
                        PricePercentChange24h = -5.73m,
                        PricePercentChange7d = -3.53m,
                        MarketCap = 564_751_999_902m,
                        UpdateDate = new DateTime(2023, 6, 26, 11, 39, 41, 123)
                    },
                    new CryptocurrencySimpleData
                    {
                        Id = 2,
                        Name = "Tether",
                        Symbol = "USDT",
                        Price = 1.01m,
                        PricePercentChange24h = -0.08m,
                        PricePercentChange7d = 0.32m,
                        MarketCap = 83_793_479_397m,
                        UpdateDate = new DateTime(2023, 6, 27, 11, 39, 41, 321)
                    }
                }, // in db
                new List<CryptocurrencySimpleData>()
                {
                    new CryptocurrencySimpleData
                    {
                        Id = 1,
                        Name = "Bitcoin",
                        Symbol = "BTC",
                        Price = 103_432.34m,
                        PricePercentChange24h = -5.73m,
                        PricePercentChange7d = -3.53m,
                        MarketCap = 564_751_999_902m,
                        UpdateDate = new DateTime(2023, 6, 27, 11, 39, 41, 123)
                    },
                    new CryptocurrencySimpleData
                    {
                        Id = 2,
                        Name = "Tether",
                        Symbol = "USDT",
                        Price = 0.999921m,
                        PricePercentChange24h = 0.1m,
                        PricePercentChange7d = 0.2m,
                        MarketCap = 83_793_479_397m,
                        UpdateDate = new DateTime(2023, 6, 27, 11, 39, 41, 123)
                    },
                    new CryptocurrencySimpleData
                    {
                        Id = 2,
                        Name = "Ethereum",
                        Symbol = "ETH",
                        Price = 1_830.34m,
                        PricePercentChange24h = -0.87m,
                        PricePercentChange7d = 7.32m,
                        MarketCap = 5.131_691_299m,
                        UpdateDate = new DateTime(2023, 6, 27, 11, 39, 41, 123)
                    }
                }, // from API
                HttpStatusCode.OK, // api response
                new List<CryptocurrencySimpleData>()
                {
                    new CryptocurrencySimpleData
                    {
                        Id = 1,
                        Name = "Bitcoin",
                        Symbol = "BTC",
                        Price = 103_432.34m,
                        PricePercentChange24h = -5.73m,
                        PricePercentChange7d = -3.53m,
                        MarketCap = 564_751_999_902m,
                        UpdateDate = new DateTime(2023, 6, 27, 11, 39, 41, 123)
                    },
                    new CryptocurrencySimpleData
                    {
                        Id = 2,
                        Name = "Tether",
                        Symbol = "USDT",
                        Price = 0.999921m,
                        PricePercentChange24h = 0.1m,
                        PricePercentChange7d = 0.2m,
                        MarketCap = 83_793_479_397m,
                        UpdateDate = new DateTime(2023, 6, 27, 11, 39, 41, 123)
                    },
                    new CryptocurrencySimpleData
                    {
                        Id = 2,
                        Name = "Ethereum",
                        Symbol = "ETH",
                        Price = 1_830.34m,
                        PricePercentChange24h = -0.87m,
                        PricePercentChange7d = 7.32m,
                        MarketCap = 5.131_691_299m,
                        UpdateDate = new DateTime(2023, 6, 27, 11, 39, 41, 123)
                    }
                }, // cryptos result
                ApiResponseStatus.Ok, // result

            }
        };

        [Theory]
        [MemberData(nameof(CryptoTestData))]
        public async void GetSimplePriceForCryptocurrencies_ForValidData_ReturnsCorrectData1(
            string[] cryptocurrencies,
            string currency,
            List<CryptocurrencySimpleData> cryptosInDb,
            List<CryptocurrencySimpleData> cryptosFromApi,
            HttpStatusCode statusFromApi,
            List<CryptocurrencySimpleData> resultCryptos,
            ApiResponseStatus resultStatus)
        {
            var cryptoSimpleDatasRepository = new Mock<ICryptoSimpleDatasRepository>();
            cryptoSimpleDatasRepository.Setup(m => m.GetByNamesAsync(cryptocurrencies))
                .Returns(Task.FromResult(cryptosInDb));

            cryptoSimpleDatasRepository.Setup(m => m.DeleteAllAsync())
                .Callback(() => cryptosInDb.Clear());

            cryptoSimpleDatasRepository.Setup(m => m.AddRangeAsync(It.IsAny<CryptocurrencySimpleData[]>()))
                .Callback<CryptocurrencySimpleData[]>(cryptos => cryptosInDb.AddRange(cryptos));

            var cryptoApiProvider = new Mock<ICryptoApiProvider>();
            cryptoApiProvider.Setup(m => m.GetCryptocurrenciesDataFromApi(200, currency))
                .Returns(Task.FromResult<(HttpStatusCode Status, List<CryptocurrencySimpleData> Value)>((statusFromApi, cryptosFromApi)));

            CryptoService cryptoService = new(cryptoSimpleDatasRepository.Object, cryptoApiProvider.Object);

            var (Status, Value) = await cryptoService.GetSimplePriceForCryptocurrencies(cryptocurrencies, currency);

            Status.Should().Be(resultStatus);

            Value.Should().BeEquivalentTo(resultCryptos);
        }
    }
}
