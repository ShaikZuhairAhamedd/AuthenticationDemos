namespace CompletWebApiCourser.IdentityServer.Auth
{
    public interface ICustomUserManager
    {
        string Authentiate(string userName, string Password);
        
    }
}