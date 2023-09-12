using System.Globalization;

namespace MoneyManager.Client.Services
{
    public interface IDisplayService
    {
        string DisplayNameOfMonth(int month);
        string DisplayNameOfMonthAndYear(DateTime dateTime);
    }

    public class DisplayService : IDisplayService
    {
        private readonly CultureInfo _culture;

        public DisplayService(CultureInfo cultureInfo)
        {
            _culture = cultureInfo;
        }

        public string DisplayNameOfMonth(int month)
        {
            if (month < 1 || month > 12)
                return "";

            return _culture.DateTimeFormat.GetMonthName(month);
        }
        public string DisplayNameOfMonthAndYear(DateTime dateTime) => dateTime.ToString("MMMM yyyy", _culture);

    }
}
