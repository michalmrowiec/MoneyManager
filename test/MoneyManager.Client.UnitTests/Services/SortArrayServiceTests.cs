using MoneyManager.Client.Models;
using MoneyManager.Client.Services;
using MoneyManager.Shared;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MoneyManager.Client.UnitTests.Services
{
    public class SortArrayServiceTests
    {
        public static IEnumerable<object[]> RecordsYears => new List<object[]>
        {
            new object[]
            {
                new List<RecordItemDto>
                {
                    new RecordItemDto { Name = "bTest", Amount = 100, TransactionDate = new DateTime(2020, 01, 01) },
                    new RecordItemDto { Name = "nTest", Amount = 210, TransactionDate = new DateTime(2021, 01, 01) },
                    new RecordItemDto { Name = "aTest", Amount = 19999, TransactionDate = new DateTime(2017, 11, 11) },
                    new RecordItemDto { Name = "uTest", Amount = -1230, TransactionDate = new DateTime(2019, 04, 22) }
                },
                new List<string> { "2021", "2020", "2019", "2017" }
            },
            new object[]
            {
                new List<RecordItemDto>
                {
                    new RecordItemDto { Name = "baTest", Amount = -1, TransactionDate = new DateTime(2015, 01, 01) },
                    new RecordItemDto { Name = "nbTest", Amount = 777777, TransactionDate = new DateTime(2021, 01, 01) },
                    new RecordItemDto { Name = "asTest", Amount = 0, TransactionDate = new DateTime(2017, 11, 11) },
                    new RecordItemDto { Name = "u1Test", Amount = -130, TransactionDate = new DateTime(2019, 04, 22) }
                },
                new List<string> { "2021", "2019", "2017", "2015" }
            },
            new object[]
            {
                new List<RecordItemDto> { },
                new List<string> { }
            }
        };
        public static IEnumerable<object[]> RecordsMonths => new List<object[]>
        {
            new object[]
            {
                new List<RecordItemDto>
                {
                    new RecordItemDto { Name = "bTest", Amount = 100, TransactionDate = new DateTime(2020, 01, 01) },
                    new RecordItemDto { Name = "nTest", Amount = 210, TransactionDate = new DateTime(2020, 01, 01) },
                    new RecordItemDto { Name = "aTest", Amount = 19999, TransactionDate = new DateTime(2020, 11, 11) },
                    new RecordItemDto { Name = "uTest", Amount = -1230, TransactionDate = new DateTime(2020, 04, 22) }
                },
                "2020",
                new List<string> { "January", "April", "November" }
            },
            new object[]
            {
                new List<RecordItemDto>
                {
                    new RecordItemDto { Name = "baTest", Amount = -1, TransactionDate = new DateTime(2015, 12, 01) },
                    new RecordItemDto { Name = "nbTest", Amount = 777777, TransactionDate = new DateTime(2015, 08, 01) },
                    new RecordItemDto { Name = "asTest", Amount = 0, TransactionDate = new DateTime(2015, 10, 11) },
                    new RecordItemDto { Name = "u1Test", Amount = -130, TransactionDate = new DateTime(2015, 03, 22) }
                },
                "2015",
                new List<string> { "March", "August", "October", "December" }
            },
            new object[]
            {
                new List<RecordItemDto> { },
                String.Empty,
                new List<string> { }
            }
        };
        public static IEnumerable<object[]> DataForSortBy => new List<object[]>
        {
            new object[]
            {
                TypesInRecord.Name,
                new List<RecordItemDto>
                {
                    new RecordItemDto { Name = "baTest", Amount = -1 },
                    new RecordItemDto { Name = "nbTest", Amount = 777777 },
                    new RecordItemDto { Name = "cTest", Amount = 777777 },
                    new RecordItemDto { Name = "asTest", Amount = 0 },
                    new RecordItemDto { Name = "12Test", Amount = 0 },
                    new RecordItemDto { Name = "000000", Amount = -130 }
                },
                new string[] { "", "", "", "" },
                new List<RecordItemDto>
                {
                    new RecordItemDto { Name = "000000", Amount = -130 },
                    new RecordItemDto { Name = "12Test", Amount = 0 },
                    new RecordItemDto { Name = "asTest", Amount = 0 },
                    new RecordItemDto { Name = "baTest", Amount = -1 },
                    new RecordItemDto { Name = "cTest", Amount = 777777 },
                    new RecordItemDto { Name = "nbTest", Amount = 777777 }
                },
                new string[] { "↑", "", "", "" }
            },
            new object[]
            {
                TypesInRecord.CategoryName,
                new List<RecordItemDto>
                {
                    new RecordItemDto { Name = "baTest", Amount = -1, CategoryName = "xvdTestCategory" },
                    new RecordItemDto { Name = "nbTest", Amount = 777777, CategoryName = "daTestCategory" },
                    new RecordItemDto { Name = "cTest", Amount = 777777, CategoryName = "odfTestCategory" },
                    new RecordItemDto { Name = "asTest", Amount = 0, CategoryName = "ahTestCategory" },
                    new RecordItemDto { Name = "12Test", Amount = 0, CategoryName = "0TestCategory" },
                    new RecordItemDto { Name = "000000", Amount = -130, CategoryName = "6TestCategory" }
                },
                new string[] { "", "", "", "" },
                new List<RecordItemDto>
                {
                    new RecordItemDto { Name = "12Test", Amount = 0, CategoryName = "0TestCategory" },
                    new RecordItemDto { Name = "000000", Amount = -130, CategoryName = "6TestCategory" },
                    new RecordItemDto { Name = "asTest", Amount = 0, CategoryName = "ahTestCategory" },
                    new RecordItemDto { Name = "nbTest", Amount = 777777, CategoryName = "daTestCategory" },
                    new RecordItemDto { Name = "cTest", Amount = 777777, CategoryName = "odfTestCategory" },
                    new RecordItemDto { Name = "baTest", Amount = -1, CategoryName = "xvdTestCategory" }
                },
                new string[] { "", "↑", "", "" }
            },
            new object[]
            {
                TypesInRecord.Amount,
                new List<RecordItemDto>
                {
                    new RecordItemDto { Name = "mTest", Amount = -1, TransactionDate = new DateTime(2015, 12, 01) },
                    new RecordItemDto { Name = "reTest", Amount = 7777.07M, TransactionDate = new DateTime(2015, 08, 01) },
                    new RecordItemDto { Name = "34Test", Amount = 0, TransactionDate = new DateTime(2015, 10, 11) },
                    new RecordItemDto { Name = "yTest", Amount = -130.65M, TransactionDate = new DateTime(2015, 03, 22) }
                },
                new string[] { "", "", "", "" },
                new List<RecordItemDto>
                {
                    new RecordItemDto { Name = "yTest", Amount = -130.65M, TransactionDate = new DateTime(2015, 03, 22) },
                    new RecordItemDto { Name = "mTest", Amount = -1, TransactionDate = new DateTime(2015, 12, 01) },
                    new RecordItemDto { Name = "34Test", Amount = 0, TransactionDate = new DateTime(2015, 10, 11) },
                    new RecordItemDto { Name = "reTest", Amount = 7777.07M, TransactionDate = new DateTime(2015, 08, 01) }
                },
                new string[] { "", "", "↑", "" }
            },
            new object[]
            {
                TypesInRecord.Date,
                new List<RecordItemDto>
                {

                    new RecordItemDto { Name = "34Test", Amount = -1, TransactionDate = new DateTime(2021, 11, 11) },
                    new RecordItemDto { Name = "34Test", Amount = 0, TransactionDate = new DateTime(2016, 08, 02) },
                    new RecordItemDto { Name = "34Test", Amount = 16, TransactionDate = new DateTime(2017, 04, 07) },
                    new RecordItemDto { Name = "34Test", Amount = -48.5M, TransactionDate = new DateTime(2020, 12, 08) },
                    new RecordItemDto { Name = "34Test", Amount = 45.76M, TransactionDate = new DateTime(2021, 02, 11) },
                    new RecordItemDto { Name = "34Test", Amount = 18888888M, TransactionDate = new DateTime(2020, 12, 09) },
                    new RecordItemDto { Name = "34Test", Amount = -99990096.7M, TransactionDate = new DateTime(2015, 09, 14) },
                    new RecordItemDto { Name = "34Test", Amount = 24555.91M, TransactionDate = new DateTime(2016, 11, 29) },
                    new RecordItemDto { Name = "34Test", Amount = 0.53M, TransactionDate = new DateTime(2019, 01, 23) }
                },
                new string[] { "", "", "", "" },
                new List<RecordItemDto>
                {
                    new RecordItemDto { Name = "34Test", Amount = -99990096.7M, TransactionDate = new DateTime(2015, 09, 14) },
                    new RecordItemDto { Name = "34Test", Amount = 0, TransactionDate = new DateTime(2016, 08, 02) },
                    new RecordItemDto { Name = "34Test", Amount = 24555.91M, TransactionDate = new DateTime(2016, 11, 29) },
                    new RecordItemDto { Name = "34Test", Amount = 16, TransactionDate = new DateTime(2017, 04, 07) },
                    new RecordItemDto { Name = "34Test", Amount = 0.53M, TransactionDate = new DateTime(2019, 01, 23) },
                    new RecordItemDto { Name = "34Test", Amount = -48.5M, TransactionDate = new DateTime(2020, 12, 08) },
                    new RecordItemDto { Name = "34Test", Amount = 18888888M, TransactionDate = new DateTime(2020, 12, 09) },
                    new RecordItemDto { Name = "34Test", Amount = -1, TransactionDate = new DateTime(2021, 11, 11) },
                    new RecordItemDto { Name = "34Test", Amount = 45.76M, TransactionDate = new DateTime(2021, 02, 11) }
                },
                new string[] { "", "", "", "↑" }
            }
        };

        [Theory]
        [MemberData(nameof(RecordsYears))]
        public void GetAllYearsFromListOfRecords_GetListOfRecords_ReturnListOfYears(List<RecordItemDto> records, List<string> expectedAnswear)
        {
            var years = SortArrayService.GetAllYearsFromListOfRecords(records);

            Assert.Equal(expectedAnswear, years);
        }

        [Theory]
        [MemberData(nameof(RecordsMonths))]
        public void GetAllMonthsFromListOfRecords_ForGivenYear_SchouldReturnAllOfMonthsInThisYear(List<RecordItemDto> records, string givenYear, List<string> expectedAnswear)
        {
            var months = SortArrayService.GetAllMonthsFromListOfRecords(records, givenYear);

            Assert.Equal(expectedAnswear, months);
        }

        [Theory]
        [MemberData(nameof(DataForSortBy))]
        public void SortByTypeTest(TypesInRecord sortBy, List<RecordItemDto> records, string[] dictionary,
            List<RecordItemDto> expectedRecords, string[] expectedDictionary)
        {
            SortArrayService.SortByType(sortBy, false, ref records, ref dictionary);


            var param = sortBy.ToString();
            var propertyInfo = typeof(RecordItemDto).GetProperty(param)!;

            IOrderedEnumerable<RecordItemDto> sorted;
            sorted = records.OrderBy(x => propertyInfo.GetValue(x));

            records.Should().ContainInOrder(sorted);
            records.Should().BeEquivalentTo(expectedRecords);
            dictionary.Should().BeEquivalentTo(expectedDictionary);
        }
    }
}
