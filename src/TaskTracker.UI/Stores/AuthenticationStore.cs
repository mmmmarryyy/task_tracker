namespace TaskTracker.UI.Services
{
    public class AuthenticationStore
    {
        public Domain.Entities.User? CurrentUser { get; private set; }
        public bool IsLoggedIn => CurrentUser != null;
        public void SetCurrentUser(Domain.Entities.User user) => CurrentUser = user;
        public void Logout() => CurrentUser = null;
    }
}
