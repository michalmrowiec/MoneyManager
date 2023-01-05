using MoneyManager.Client.Models;
using MoneyManager.Client.Models.ViewModels.Interfaces;
using MoneyManager.Client.ViewModels;
using MoneyManager.Client.ViewModels.Interfaces;

namespace MoneyManager.Client.Services
{
    internal static class SortArrayService
    {
        internal static List<T> FilterRecordListWithParameters<T>(List<T> list, FilterParameters parameters) where T : RecordVM
        {
            var operations = new Dictionary<TypeOfRecord, Action>();
            operations[TypeOfRecord.OnlyIncomes] = () => { list = list.Where(x => x.Amount >= 0).ToList(); };
            operations[TypeOfRecord.OnlyExpenses] = () => { list = list.Where(x => x.Amount < 0).ToList(); };
            operations[TypeOfRecord.ExpensesAndIncomes] = () => { };

            if (parameters.Year != null)
            {
                list = list.Where(x => x.TransactionDate.Year == parameters.Year).ToList();

                if (parameters.Month != null)
                    list = list.Where(x => x.TransactionDate.Month == parameters.Month).ToList();
            }

            operations[parameters.TypeOfRecord].Invoke();

            if (parameters.CategoryId != 0)
                list = list.Where(x => x.CategoryId == parameters.CategoryId).ToList();

            return list;
        }

        internal static List<int> GetAllYearsFromListOfRecords<T>(List<T> records) where T : IRecord, IRecordWithDate
        {
            var years = records.Select(x => x.TransactionDate.Year)
                .Distinct()
                .OrderByDescending(x => x)
                .ToList();
            return years;
        }

        internal static List<int> GetAllMonthsFromListOfRecords<T>(List<T> records, int year) where T : IRecord, IRecordWithDate
        {
            var months = records.Where(x => x.TransactionDate.Year == year)
                .OrderBy(x => x.TransactionDate)
                .Select(x => x.TransactionDate.Month)
                .Distinct()
                .ToList();
            return months;
        }

        internal static void SortByType<T>(RecordField sortBy, bool descending, ref List<T> listOfRecords, ref string[] str) where T : IId
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

        /// <summary>
        /// Set sort arrow in name of row
        /// </summary>
        /// <param name="type">Sort on the given type</param>
        /// <param name="descending">true - descending | false - ascending</param>
        /// <param name="str">Array of strings for arrow</param>
        private static void SetArrow(RecordField type, bool descending, ref string[] str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = "";
                if ((RecordField)i == type)
                    continue;
            }

            if (descending)
                str[(int)type] = "↓";
            else
                str[(int)type] = "↑";
        }
    }
}
