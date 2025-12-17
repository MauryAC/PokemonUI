public class AuthState
{
    public string? Email { get; private set; }
    public bool IsAuthenticated => !string.IsNullOrEmpty(Email);

    public event Action? OnChange;

    public void Login(string email)
    {
        Email = email;
        NotifyStateChanged();
    }

    public void Logout()
    {
        Email = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }
}
