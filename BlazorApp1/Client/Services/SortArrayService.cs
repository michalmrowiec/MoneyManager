﻿using BlazorApp1.Client.Models;
using BlazorApp1.Shared;
using System.Globalization;
using System.Linq;

namespace BlazorApp1.Client.Services
{
    internal static class SortArrayService
    {
        internal static List<string> GetAllYearsFromListOfRecords(List<RecordItemDto> records)
        {
            var years = records.Select(x => x.Date.ToString("yyyy"))
                .Distinct()
                .OrderByDescending(x => x)
                .ToList();
            return years;
        }

        internal static List<string> GetAllMonthsFromListOfRecords(List<RecordItemDto> records, string year)
        {
            var months = records.Where(x => x.Date.ToString("yyyy") == year)
                .OrderBy(x => x.Date)
                .Select(x => x.Date.ToString("MMMM", CultureInfo.GetCultureInfo("en-US")))
                .Distinct()
                .ToList();
            return months;
        }

        internal static void SortByType<T>(TypesInRecord sortBy, bool descending, ref List<T> listOfRecords, ref string[] str) where T : IRecord
        {
            if (listOfRecords is null) return;

            SetArrow(sortBy, descending, ref str);

            var obj = listOfRecords.FirstOrDefault();
            if (obj == null) return;
            var objType = obj.GetType();
            var prop = objType.GetProperty(sortBy.ToString());
            if (prop == null) return;
            var propValue = prop.GetValue(obj);
            if (propValue == null) return;
            var propType = propValue.GetType();

            var propertyInfo = typeof(T).GetProperty(sortBy.ToString());
            if (propertyInfo == null) return;
            if (descending)
                listOfRecords = listOfRecords.OrderByDescending(x => propertyInfo.GetValue(x)).ToList<T>();
            else
                listOfRecords = listOfRecords.OrderBy(x => propertyInfo.GetValue(x)).ToList<T>();
        }

        private static void SetArrow(TypesInRecord type, bool descending, ref string[] str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = "";
                if ((TypesInRecord)i == type)
                    continue;
            }

            if (descending)
                str[(int)type] = "↓";
            else
                str[(int)type] = "↑";
        }
    }
}
