namespace MoneyManager.Client.Models
{
    internal class FilterParameters
    {
        public TypeOfRecord TypeOfRecord { get; set; } = TypeOfRecord.ExpensesAndIncomes;
        /// <summary>
        /// CategoryId = 0 means all categories
        /// </summary>
        public int CategoryId { get; set; } = 0;
        public int Year { get; set; } = 0;
        public int Month { get; set; } = 0;
        public string? SearchedPhrase { get; set; }
    }

    internal enum TypeOfRecord
    {
        OnlyExpenses,
        OnlyIncomes,
        ExpensesAndIncomes
    }
}
