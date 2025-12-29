public class AuthState
{
    public string? Email { get; private set; }
    public string? Token { get; private set; }

    public bool IsAuthenticated => !string.IsNullOrEmpty(Token);

    public event Action? OnChange;

    public void Login(string email, string token)
    {
        Email = email;
        Token = token;
        NotifyStateChanged();
    }

    public void Logout()
    {
        Email = null;
        Token = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }
}
