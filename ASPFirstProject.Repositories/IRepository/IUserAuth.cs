using APIFirstProject.Entities.User_Records;

namespace ASPFirstProject.Repositories.IRepository
{
    public interface IUserAuth
    {
        UserRegister Registration(String UserName, String UserPassword);

        public string AuthenticateUser(String UserName, String UserPassword);

        public string ValidateRefreshTooken(string UserName, string RefreshToken);
       public string GenerateToken(string username, string secretKey, int expirationMinutes, int refreshTokenExpirationMinutes);
        public (string accessToken, string refreshToken) GenerateTokens(string username, string secretKey, int accessTokenExpirationMinutes, int refreshTokenExpirationMinutes);
    }
}
