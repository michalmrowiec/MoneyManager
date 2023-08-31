﻿namespace MoneyManager.Client.Pages.LoginUsers
{
    public class AuthenticationState
    {
        private bool _loggedIn;
        public event Action? OnChange;

        public bool LoggedIn
        {
            get { return _loggedIn; }
            set
            {
                if (_loggedIn != value)
                {
                    _loggedIn = value;
                    NotifyStateChanged();
                }
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
