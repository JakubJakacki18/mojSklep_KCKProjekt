namespace Library.Interfaces
{
    public interface IUserView
    {
        public (string, string) showSignIn();
        public (string, string) showSignUp();
        public bool LandingPage();
    }
}
