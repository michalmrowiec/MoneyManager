using FluentAssertions;
using MoneyManager.Client.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace MoneyManager.Client.UnitTests.Services
{
    public class DisplayServiceTests
    {
        [Theory]
        [InlineData("en-US", 1, "January")]
        [InlineData("en-US", 2, "February")]
        [InlineData("en-US", 3, "March")]
        [InlineData("en-US", 4, "April")]
        [InlineData("en-US", 12, "December")]
        [InlineData("pl-PL", 1, "styczeń")]
        [InlineData("pl-PL", 2, "luty")]
        [InlineData("pl-PL", 3, "marzec")]
        [InlineData("pl-PL", 11, "listopad")]
        [InlineData("es-ES", 1, "enero")]
        [InlineData("es-ES", 2, "febrero")]
        [InlineData("es-ES", 3, "marzo")]
        [InlineData("fr-FR", 6, "juin")]
        [InlineData("fr-FR", 9, "septembre")]
        [InlineData("it-IT", 10, "ottobre")]
        [InlineData("it-IT", 11, "novembre")]
        [InlineData("de-DE", 5, "Mai")]
        [InlineData("de-DE", 7, "Juli")]
        public void DisplayNameOfMonth_ForValidData_ReturnValidString(string culture, int numberOfMonth, string result)
        {
            IDisplayService displayService = new DisplayService(new CultureInfo(culture));

            string nameOfMonth = displayService.DisplayNameOfMonth(numberOfMonth);

            nameOfMonth.Should().Be(result);
        }

        [Theory]
        [InlineData("en-US", -1, "")]
        [InlineData("pl-PL", 0, "")]
        [InlineData("fr-FR", 13, "")]
        [InlineData("it-IT", 100, "")]
        public void DisplayNameOfMonth_ForInvalidData_ReturnEmptyString(string culture, int numberOfMonth, string result)
        {
            IDisplayService displayService = new DisplayService(new CultureInfo(culture));

            string nameOfMonth = displayService.DisplayNameOfMonth(numberOfMonth);

            nameOfMonth.Should().Be(result);
        }

        public static IEnumerable<object[]> TestDataForDisplayNameOfMonthAndYear => new List<object[]>
        {
            new object[]
            {
                "en-US",
                new DateTime(0001, 01, 01),
                "January 0001"
            },
            new object[]
            {
                "it-IT",
                new DateTime(1999, 11, 01),
                "novembre 1999"
            },
            new object[]
            {
                "de-DE",
                new DateTime(2020, 07, 12),
                "Juli 2020"
            },
            new object[]
            {
                "es-ES",
                new DateTime(2198, 03, 29),
                "marzo 2198"
            }
        };

        [Theory]
        [MemberData(nameof(TestDataForDisplayNameOfMonthAndYear))]
        public void DisplayNameOfMonthAndYear_ForValidData_ReturnFormatedString(string culture, DateTime dateTime, string result)
        {
            IDisplayService displayService = new DisplayService(new CultureInfo(culture));

            string nameOfMonth = displayService.DisplayNameOfMonthAndYear(dateTime);

            nameOfMonth.Should().Be(result);
        }

    }
}
