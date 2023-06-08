using FluentAssertions;
using MoneyManager.Client.Models;
using MoneyManager.Client.Services;
using MoneyManager.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MoneyManager.Client.UnitTests.Services
{
    public class SortArrayServiceTests
    {
        public static IEnumerable<object[]> RecordsYears => new List<object[]>
        {
            new object[]
            {
                new List<RecordVM>
                {
                    new RecordVM { Name = "bTest", Amount = 100, TransactionDate = new DateTime(2020, 01, 01) },
                    new RecordVM { Name = "nTest", Amount = 210, TransactionDate = new DateTime(2021, 01, 01) },
                    new RecordVM { Name = "aTest", Amount = 19999, TransactionDate = new DateTime(2017, 11, 11) },
                    new RecordVM { Name = "uTest", Amount = -1230, TransactionDate = new DateTime(2019, 04, 22) }
                },
                new List<int> { 2021, 2020, 2019, 2017 }
            },
            new object[]
            {
                new List<RecordVM>
                {
                    new RecordVM { Name = "baTest", Amount = -1, TransactionDate = new DateTime(2015, 01, 01) },
                    new RecordVM { Name = "nbTest", Amount = 777777, TransactionDate = new DateTime(2021, 01, 01) },
                    new RecordVM { Name = "asTest", Amount = 0, TransactionDate = new DateTime(2017, 11, 11) },
                    new RecordVM { Name = "u1Test", Amount = -130, TransactionDate = new DateTime(2019, 04, 22) }
                },
                new List<int> { 2021, 2019, 2017, 2015 }
            },
            new object[]
            {
                new List<RecordVM> { },
                new List<int> { }
            }
        };
        public static IEnumerable<object[]> RecordsMonths => new List<object[]>
        {
            new object[]
            {
                new List<RecordVM>
                {
                    new RecordVM { Name = "bTest", Amount = 100, TransactionDate = new DateTime(2020, 01, 01) },
                    new RecordVM { Name = "nTest", Amount = 210, TransactionDate = new DateTime(2020, 01, 01) },
                    new RecordVM { Name = "aTest", Amount = 19999, TransactionDate = new DateTime(2020, 11, 11) },
                    new RecordVM { Name = "uTest", Amount = -1230, TransactionDate = new DateTime(2020, 04, 22) }
                },
                2020,
                new List<int> { 1, 4, 11 }
            },
            new object[]
            {
                new List<RecordVM>
                {
                    new RecordVM { Name = "baTest", Amount = -1, TransactionDate = new DateTime(2015, 12, 01) },
                    new RecordVM { Name = "nbTest", Amount = 777777, TransactionDate = new DateTime(2015, 08, 01) },
                    new RecordVM { Name = "asTest", Amount = 0, TransactionDate = new DateTime(2015, 10, 11) },
                    new RecordVM { Name = "u1Test", Amount = -130, TransactionDate = new DateTime(2015, 03, 22) }
                },
                2015,
                new List<int> { 3, 8, 10, 12 }
            },
            new object[]
            {
                new List<RecordVM> { },
                0,
                new List<int> { }
            }
        };
        public static IEnumerable<object[]> DataForSortBy => new List<object[]>
        {
            new object[]
            {
                RecordField.Name,
                new List<RecordVM>
                {
                    new RecordVM { Name = "baTest", Amount = -1 },
                    new RecordVM { Name = "nbTest", Amount = 777777 },
                    new RecordVM { Name = "cTest", Amount = 777777 },
                    new RecordVM { Name = "asTest", Amount = 0 },
                    new RecordVM { Name = "12Test", Amount = 0 },
                    new RecordVM { Name = "000000", Amount = -130 }
                },
                new string[] { "", "", "", "" },
                new List<RecordVM>
                {
                    new RecordVM { Name = "000000", Amount = -130 },
                    new RecordVM { Name = "12Test", Amount = 0 },
                    new RecordVM { Name = "asTest", Amount = 0 },
                    new RecordVM { Name = "baTest", Amount = -1 },
                    new RecordVM { Name = "cTest", Amount = 777777 },
                    new RecordVM { Name = "nbTest", Amount = 777777 }
                },
                new string[] { "↑", "", "", "" }
            },
            new object[]
            {
                RecordField.CategoryName,
                new List<RecordVM>
                {
                    new RecordVM { Name = "baTest", Amount = -1, CategoryName = "xvdTestCategory" },
                    new RecordVM { Name = "nbTest", Amount = 777777, CategoryName = "daTestCategory" },
                    new RecordVM { Name = "cTest", Amount = 777777, CategoryName = "odfTestCategory" },
                    new RecordVM { Name = "asTest", Amount = 0, CategoryName = "ahTestCategory" },
                    new RecordVM { Name = "12Test", Amount = 0, CategoryName = "0TestCategory" },
                    new RecordVM { Name = "000000", Amount = -130, CategoryName = "6TestCategory" }
                },
                new string[] { "", "", "", "" },
                new List<RecordVM>
                {
                    new RecordVM { Name = "12Test", Amount = 0, CategoryName = "0TestCategory" },
                    new RecordVM { Name = "000000", Amount = -130, CategoryName = "6TestCategory" },
                    new RecordVM { Name = "asTest", Amount = 0, CategoryName = "ahTestCategory" },
                    new RecordVM { Name = "nbTest", Amount = 777777, CategoryName = "daTestCategory" },
                    new RecordVM { Name = "cTest", Amount = 777777, CategoryName = "odfTestCategory" },
                    new RecordVM { Name = "baTest", Amount = -1, CategoryName = "xvdTestCategory" }
                },
                new string[] { "", "↑", "", "" }
            },
            new object[]
            {
                RecordField.Amount,
                new List<RecordVM>
                {
                    new RecordVM { Name = "mTest", Amount = -1, TransactionDate = new DateTime(2015, 12, 01) },
                    new RecordVM { Name = "reTest", Amount = 7777.07M, TransactionDate = new DateTime(2015, 08, 01) },
                    new RecordVM { Name = "34Test", Amount = 0, TransactionDate = new DateTime(2015, 10, 11) },
                    new RecordVM { Name = "yTest", Amount = -130.65M, TransactionDate = new DateTime(2015, 03, 22) }
                },
                new string[] { "", "", "", "" },
                new List<RecordVM>
                {
                    new RecordVM { Name = "yTest", Amount = -130.65M, TransactionDate = new DateTime(2015, 03, 22) },
                    new RecordVM { Name = "mTest", Amount = -1, TransactionDate = new DateTime(2015, 12, 01) },
                    new RecordVM { Name = "34Test", Amount = 0, TransactionDate = new DateTime(2015, 10, 11) },
                    new RecordVM { Name = "reTest", Amount = 7777.07M, TransactionDate = new DateTime(2015, 08, 01) }
                },
                new string[] { "", "", "↑", "" }
            },
            new object[]
            {
                RecordField.TransactionDate,
                new List<RecordVM>
                {

                    new RecordVM { Name = "34Test", Amount = -1, TransactionDate = new DateTime(2021, 11, 11) },
                    new RecordVM { Name = "34Test", Amount = 0, TransactionDate = new DateTime(2016, 08, 02) },
                    new RecordVM { Name = "34Test", Amount = 16, TransactionDate = new DateTime(2017, 04, 07) },
                    new RecordVM { Name = "34Test", Amount = -48.5M, TransactionDate = new DateTime(2020, 12, 08) },
                    new RecordVM { Name = "34Test", Amount = 45.76M, TransactionDate = new DateTime(2021, 02, 11) },
                    new RecordVM { Name = "34Test", Amount = 18888888M, TransactionDate = new DateTime(2020, 12, 09) },
                    new RecordVM { Name = "34Test", Amount = -99990096.7M, TransactionDate = new DateTime(2015, 09, 14) },
                    new RecordVM { Name = "34Test", Amount = 24555.91M, TransactionDate = new DateTime(2016, 11, 29) },
                    new RecordVM { Name = "34Test", Amount = 0.53M, TransactionDate = new DateTime(2019, 01, 23) }
                },
                new string[] { "", "", "", "" },
                new List<RecordVM>
                {
                    new RecordVM { Name = "34Test", Amount = -99990096.7M, TransactionDate = new DateTime(2015, 09, 14) },
                    new RecordVM { Name = "34Test", Amount = 0, TransactionDate = new DateTime(2016, 08, 02) },
                    new RecordVM { Name = "34Test", Amount = 24555.91M, TransactionDate = new DateTime(2016, 11, 29) },
                    new RecordVM { Name = "34Test", Amount = 16, TransactionDate = new DateTime(2017, 04, 07) },
                    new RecordVM { Name = "34Test", Amount = 0.53M, TransactionDate = new DateTime(2019, 01, 23) },
                    new RecordVM { Name = "34Test", Amount = -48.5M, TransactionDate = new DateTime(2020, 12, 08) },
                    new RecordVM { Name = "34Test", Amount = 18888888M, TransactionDate = new DateTime(2020, 12, 09) },
                    new RecordVM { Name = "34Test", Amount = -1, TransactionDate = new DateTime(2021, 11, 11) },
                    new RecordVM { Name = "34Test", Amount = 45.76M, TransactionDate = new DateTime(2021, 02, 11) }
                },
                new string[] { "", "", "", "↑" }
            }
        };

        [Theory]
        [MemberData(nameof(RecordsYears))]
        public void GetAllYearsFromListOfRecords_GetListOfRecords_ReturnListOfYears(List<RecordVM> records, List<int> expectedAnswear)
        {
            var years = SortArrayService.GetAllYearsFromListOfRecords(records);

            Assert.Equal(expectedAnswear, years);
        }

        [Theory]
        [MemberData(nameof(RecordsMonths))]
        public void GetAllMonthsFromListOfRecords_ForGivenYear_SchouldReturnAllOfMonthsInThisYear(List<RecordVM> records, int givenYear, List<int> expectedAnswear)
        {
            var months = SortArrayService.GetAllMonthsFromListOfRecords(records, givenYear);

            Assert.Equal(expectedAnswear, months);
        }

        [Theory]
        [MemberData(nameof(DataForSortBy))]
        public void SortByTypeTest(RecordField sortBy, List<RecordVM> records, string[] dictionary,
            List<RecordVM> expectedRecords, string[] expectedDictionary)
        {
            SortArrayService.SortByType(sortBy, false, ref records, ref dictionary);


            var param = sortBy.ToString();
            var propertyInfo = typeof(RecordVM).GetProperty(param)!;

            IOrderedEnumerable<RecordVM> sorted;
            sorted = records.OrderBy(x => propertyInfo.GetValue(x));

            records.Should().ContainInOrder(sorted);
            records.Should().BeEquivalentTo(expectedRecords);
            dictionary.Should().BeEquivalentTo(expectedDictionary);
        }
    }
}
