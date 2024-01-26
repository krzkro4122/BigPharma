namespace BigPharmaEngine
{
    public interface IAuthenticator
    {
        bool IsLoggedIn { get; }
        string CurrentUserName { get; }
        bool Login(string username, string password);
        bool Register(string username, string email, string password, string confirmPassword);
        void Logout();
    }
    public class Authenticator : IAuthenticator
    {

        private readonly AuthenticationService authenticationService;

        public Authenticator()
        {
            this.authenticationService = new AuthenticationService();
            this.CurrentUserName = "";
        }

        public bool IsLoggedIn => !string.IsNullOrEmpty(CurrentUserName);
        public string CurrentUserName { get; private set; }

        public bool Login(string username, string password)
        {
            bool loginSuccessful = authenticationService.Login(username, password);
            if (loginSuccessful)
            {
                CurrentUserName = username; 
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Register(string username, string email, string password, string confirmPassword)
        {
            return authenticationService.Register(username, email, password, confirmPassword);
        }
        public void Logout()
        {
            CurrentUserName = "";
        }
    }
}
