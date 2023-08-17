using System.ComponentModel;

namespace MoneyManager.Client
{
    public class NameOfSubpage
    {
        private NamesOfSubpageEnum _namesOfSubpage;
        public event Action? OnChange;

        public NamesOfSubpageEnum NamesOfSubpage
        {
            get { return _namesOfSubpage; }
            set
            {
                if(_namesOfSubpage != value)
                _namesOfSubpage = value;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}

public enum NamesOfSubpageEnum
{
    [Description("Main Dashboard")]
    MainDashboard,
    [Description("Add")]
    Add,
    [Description("Financial Dashboard")]
    FinancialDashboard,
    [Description("Recurring Records")]
    RecurringRecords,
    [Description("Planned Budgets")]
    PlannedBudgets,
    [Description("Categories")]
    Categories,
    [Description("Settings")]
    Settings,
    [Description("Forgot Password")]
    ForgotPassword,
    [Description("Crypto Assets")]
    CrytpoAssets,
    [Description("")]
    Empty
}
