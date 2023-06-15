using APIFirstProject.Entities.DataModels;
using APIFirstProject.Entities.MissionDataModel;
using APIFirstProject.Entities.User_Records;
using ASPFirstProject.Repositories.IRepository;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;

namespace DemoApiProject.Controllers
{
    [Route("api/Todo")]
    [ApiController]

    public class TodoController : ControllerBase
    {
        private readonly CiplatformContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMissionRepository _missionRepository;
        private readonly IUserAuth _userAuth;
        private readonly string connectionString = "Server=PCA172\\SQL2017;Database=imagesdb;Trusted_Connection=True;MultipleActiveResultSets=true;User ID=sa;Password=Tatva@123;Integrated Security=False; TrustServerCertificate=True";
        public TodoController(CiplatformContext context, IMissionRepository missionRepository, IUserAuth userAuth, IConfiguration configuration)
        {
            _context = context;
            _missionRepository = missionRepository;
            _userAuth = userAuth;
            _configuration = configuration;
        }

        // Get all the Misison data 
        [HttpGet("MissionData")]
        [Authorize]
        public ActionResult<List<AllMissionDataModel>> MissionData()
        {
            try
            {
                var AllmissionData = _missionRepository.AllMissionRecords();
                return AllmissionData;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Get only one misson data by id by passing parameter

        [HttpPost("GetOneMissionData")]
        public ActionResult<AllMissionDataModel> GetOneMissionData([FromBody] MissionIdRequest request)
        {
            try
            {
                long MisionID = request.MissionId;
                var onemissiondata = _missionRepository.OneMissionRecord(MisionID);
                return onemissiondata;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // register new user data
        [HttpPost("InsertUserRecords")]

        public ActionResult<UserRegister> InsertUserRecords([FromBody] UserRegister request)
        {
            try
            {
                string username = request.UserName;
                string userpassword = request.UserPassword;
                var userdata = _userAuth.Registration(username, userpassword);
                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Login User data with validate

        //[HttpPost("LoginUser")]

        //public ActionResult<UserRegister> LoginUser([FromBody] UserRegister data)
        //{
        //    try
        //    {
        //        string username = data.UserName;
        //        string password = data.UserPassword;

        //        int result = _userAuth.AuthenticateUser(username, password);   

        //        if (result == 1)
        //        {
        //            return Ok("User is Valid");
        //        }
        //        else
        //        {
        //            return Unauthorized();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        // generate the token 
        //[HttpPost("LoginUser")]
        //public ActionResult<UserRegister> LoginUser([FromBody] UserRegister data, [FromServices] IUserAuth userAuth, [FromServices] IConfiguration configuration)
        //{
        //    try
        //    {
        //        string username = data.UserName;
        //        string password = data.UserPassword;

        //        string token = _userAuth.AuthenticateUser(username, password);

        //        if (token.ToUpper() == "TRUE")
        //        {
        //            string secretKey = configuration["JwtSettings:SecretKey"]; // Use the configured secret key
        //            string tokengenerate = _userAuth.GenerateToken(username, secretKey, 5); // Set expiration time as desired

        //            HttpContext.Session.SetString("token", tokengenerate);
        //            // Return the token in the response
        //            return Ok(new { Token = tokengenerate });
        //        }
        //        else
        //        {
        //            return Unauthorized();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("An error occurred while generating the JWT token: " + ex.Message);
        //        return BadRequest(ex.Message);
        //    }
        //}


        // generate the access token with refresh token 
        [HttpPost("LoginUser")]
        public ActionResult<UserRegister> LoginUser([FromBody] UserRegister data, [FromServices] IUserAuth userAuth, [FromServices] IConfiguration configuration)
        {
            try
            {
                string username = data.UserName;
                string password = data.UserPassword;

                string token = _userAuth.AuthenticateUser(username, password);

                if (token.ToUpper() == "TRUE")
                {
                    string secretKey = configuration["JwtSettings:SecretKey"];
                    int accessTokenExpirationMinutes = 60; // 5 min
                    int refreshTokenExpirationMinutes = 1440; // 24 hours

                    var (accessToken, refreshToken) = _userAuth.GenerateTokens(username, secretKey, accessTokenExpirationMinutes, refreshTokenExpirationMinutes);

                    HttpContext.Session.SetString("token", accessToken);

                    // Return both access token and refresh token in the response
                    return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
                }
                else
                {
                    return Unauthorized("UserName or Password Incorrect..");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while generating the JWT token: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("RefreshToken")]
        public ActionResult RefreshToken([FromBody] RefreshTokenRequestModel request, [FromServices] IUserAuth userAuth, [FromServices] IConfiguration configuration)
        {
            try
            {
                string refreshToken = request.RefreshToken;
                string username = request.Username;
                // Validate the refresh token (check signature, expiration, etc.)
                // If the refresh token is valid, generate a new access token
                string secretKey = configuration["JwtSettings:SecretKey"];
                int accessTokenExpirationMinutes = 60;
                int refreshTokenExpirationMinutes = 1440;

                string RefreshTokenValidate = _userAuth.ValidateRefreshTooken(username, refreshToken);

                if (RefreshTokenValidate.ToUpper() == "TRUE")
                {
                    string newAccessToken = _userAuth.GenerateToken(username, secretKey, accessTokenExpirationMinutes, refreshTokenExpirationMinutes);

                    // Return the new access token in the response
                    return Ok(new { AccessToken = newAccessToken });
                }
                else if(RefreshTokenValidate.ToUpper() == "EXPIRED")
                {
                    return Unauthorized("Refresh token has expired, Goto login page ");
                }
                else
                {
                    return Unauthorized("UserName or RefreshToken Not Match !!! ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while refreshing the token: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

    }
}