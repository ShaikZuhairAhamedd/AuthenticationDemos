namespace CompletWebApiCourser.IdentityServer.Auth
{
    public interface ICustomTokenManager
    {
        string CreateToken(string UserName);
        bool VerifyToken(string token);
        string GetUserInfoByToken(string token);
    }
}