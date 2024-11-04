namespace Library.Interfaces
{
    public interface IUserView
    {
        public Task<(string, string)> ShowSignIn(bool isValid = true);
        public Task<(string, string)> ShowSignUp();
        public Task<bool> LandingPage();
    }
}
