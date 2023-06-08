namespace MoneyManager.Client
{
    public class ErrorMessage
    {
        private string? _message { get; set; }
        public event Action? OnChange;

        public string? Message
        {
            get { return _message; }
            set { _message = value; OnChange?.Invoke(); }
        }
    }
}