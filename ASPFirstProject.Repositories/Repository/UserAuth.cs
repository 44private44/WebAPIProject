using APIFirstProject.Entities.User_Records;
using ASPFirstProject.Repositories.IRepository;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASPFirstProject.Repositories.Repository
{
    public class UserAuth : IUserAuth
    {
        private readonly string connectionString = "Server=PCA172\\SQL2017;Database=imagesdb;Trusted_Connection=True;MultipleActiveResultSets=true;User ID=sa;Password=Tatva@123;Integrated Security=False; TrustServerCertificate=True";

        private readonly IConfiguration _configuration;

        public UserAuth(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // New User Registration
        UserRegister IUserAuth.Registration(String UserName, String UserPassword)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new
                {
                    username = UserName,
                    userpassword = UserPassword,
                };

                //also add like dynamic 
                //var parameters = new DynamicParameters();
                //parameters.Add("@Username", UserName);
                //parameters.Add("@Password", UserPassword);

                UserRegister userdata = connection.QueryFirstOrDefault<UserRegister>("InsertUserData", parameters, commandType: CommandType.StoredProcedure);
                return userdata;
            }
        }

        // User Login Validate

        //public int AuthenticateUser(String UserName, String UserPassword)
        //{
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        var parameters = new
        //        {
        //            Username = UserName,
        //            Password = UserPassword,
        //        };

        //        //int result = connection.QueryFirstOrDefault<int>("ValidateUser", parameters, commandType: CommandType.StoredProcedure);
        //        int result = connection.ExecuteScalar<int>("ValidateUser", parameters, commandType: CommandType.StoredProcedure);

        //        // above both are more accurate but when we get return only one value then use ExecuteScalar
        //        // if we want return multiple column of one row t time use the QueryFirstOrDefault method...

        //        return result;
        //    }
        //}


        public string AuthenticateUser(string UserName, string UserPassword)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new
                {
                    Username = UserName,
                    Password = UserPassword,
                };
                string tokenResult = connection.QueryFirstOrDefault<string>("ValidateUser", parameters, commandType: CommandType.StoredProcedure);
                return tokenResult;
            }
        }

        // validate the refresh token 
        public string ValidateRefreshTooken(string UserName, string RefreshToken)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new
                {
                    @userName = UserName,
                    @RequestToken = RefreshToken,
                };
                string tokenResult = connection.QueryFirstOrDefault<string>("RefreshTokenValidate", parameters, commandType: CommandType.StoredProcedure);
                return tokenResult;
            }
        }

        // Generate the token
        public string GenerateToken(string username,string secretKey, int expirationMinutes, int refreshTokenExpirationMinutes)
        {
            // Create the security key using your secret key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // Create signing credentials using the security key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create claims for the token (you can add more if needed)
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //unique identifier 
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()) // issued at time 
            };

            // Create the token with the specified claims, signing credentials, and expiration time
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"], // Use the configured issuer
                audience: _configuration["JwtSettings:Audience"], // Use the configured audience
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: creds
            );

            // Serialize the token to a string
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new
                {
                    userName = username,
                    expireTime = refreshTokenExpirationMinutes
                };
                string tokenResult = connection.QueryFirstOrDefault<string>("Update_refresh_expiration_time", parameters, commandType: CommandType.StoredProcedure);
            }

            return tokenString;
        }

        // generate the access tooken and Refresh token both
        public (string accessToken, string refreshToken) GenerateTokens(string username, string secretKey, int accessTokenExpirationMinutes, int refreshTokenExpirationMinutes)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessTokenClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            };

            var refreshTokenClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            };

            var accessToken = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: accessTokenClaims,
                expires: DateTime.UtcNow.AddMinutes(accessTokenExpirationMinutes),
                signingCredentials: creds
            );

            var refreshToken = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: refreshTokenClaims,
                expires: DateTime.UtcNow.AddMinutes(refreshTokenExpirationMinutes),
                signingCredentials: creds
            );


            var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);
            var refreshTokenString = new JwtSecurityTokenHandler().WriteToken(refreshToken);

            // Update Refresh token in table 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new
                {
                    Username = username,
                    RequestToken = refreshTokenString,
                    ExpirationTimeInMinutes = refreshTokenExpirationMinutes
                };
                string tokenResult = connection.QueryFirstOrDefault<string>("Request_token_crud", parameters, commandType: CommandType.StoredProcedure);
            }

            return (accessTokenString, refreshTokenString);
        }


    }
}
