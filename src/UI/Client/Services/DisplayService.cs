using System.Globalization;

namespace MoneyManager.Client.Services
{
    public interface IDisplayService
    {
        string DisplayMonth(int month);
    }

    public class DisplayService : IDisplayService
    {
        private readonly CultureInfo _culture;

        public DisplayService(CultureInfo cultureInfo)
        {
            _culture = cultureInfo;
        }

        public string DisplayMonth(int month) => _culture.DateTimeFormat.GetMonthName(month);
    }
}
