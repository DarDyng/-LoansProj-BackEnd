using Google.Apis.Auth;
using LoansAppWebApi.Core;
using LoansAppWebApi.Core.Constants;
using LoansAppWebApi.Core.Filters;
using LoansAppWebApi.Core.Services;
using LoansAppWebApi.Models.Configuration;
using LoansAppWebApi.Models.DbModels;
using LoansAppWebApi.Models.DTO_s.Requests;
using LoansAppWebApi.Models.DTO_s.Resposnes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoansAppWebApi.Core.Interfaces;

namespace LoansAppWebApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly JWTConfiguration jWTConfiguration;
        private readonly JwtGenerator jwtGenerator;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AuthenticationController(
            IConfiguration configuration,
            IOptions<JWTConfiguration> options,
            JwtGenerator jwtGenerator,
            IUserService userService)
        {
            _configuration = configuration;
            this.jWTConfiguration = options.Value;
            this.jwtGenerator = jwtGenerator;
            _userService = userService;
        }

        [HttpPost]
        [Route("google")]
        public async Task<ActionResult> Google(GoogleAuthenticateRequest google)
        {
            try
            {
                GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();

                settings.Audience = new List<string> { _configuration.GetValue<string>("Google:ClientId")! };
                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(google.IdToken, settings);

                // check if user exists by email in db

                var user = await _userService.GetUserByEmail(payload.Email);

                if (user == null)
                {
                    // create user with such email and type google

                    user = new User()
                    {
                        Id = Guid.NewGuid(),
                        UserName = payload.Email,
                        Email = payload.Email,
                        EmailConfirmed = true,
                        AuthType = AuthType.Google,
                    };

                    var result = await _userService.CreateUser(user, Guid.NewGuid().ToString());

                    if (!result.Succeeded)
                        return BadRequest("O-ops, something went wrong");

                }

                if (user.AuthType == AuthType.Normal)
                    return Unauthorized("This user already registered!");

                return Ok(new AuthenticatedUserResposne
                {
                    Token = jwtGenerator.GenerateToken(payload.Email, user.Id.ToString()),
                    Expiration = DateTime.Now.AddMinutes(jWTConfiguration.AccessTokenExpirationMinutes)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        [ApiExceptionFilter]
        public async Task<ActionResult> Authenticate(
            LoginModel loginModel)
        {
            var userFromContextUser = HttpContext.User;

            var user = await _userService.GetUserByEmail(loginModel.Email);

            if (user == null)
            {
                return Unauthorized("User not exists, register yourself!");
            }

            if (user.AuthType == AuthType.Google)
            {
                return Unauthorized("Use google authentication insted");
            }

            if (!await _userService.CheckPasswordAsync(user, loginModel.Password))
            {
                return BadRequest("Invalid credentionals");
            }

            var roles = await _userService.GetUserRoles(user);

            var authClaims = new List<Claim>
                {
                    new Claim(AuthConstants.ClaimNames.Id, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            // adding all roles which was found
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigngingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTConfiguration.AccessTokenSecret));

            var token = new JwtSecurityToken(
                issuer: jWTConfiguration.Issuer,
                audience: jWTConfiguration.Audience,
                expires: DateTime.Now.AddMinutes(jWTConfiguration.AccessTokenExpirationMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigngingKey, SecurityAlgorithms.HmacSha256)
                );

            return Ok(new AuthenticatedUserResposne
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });
        }

        [HttpPost]
        //[HttpResponseFilter]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            // could be changed, but good for now
            if (await _userService.CheckUserExistsByEmail(model.Email))
                return BadRequest("User is already registered with such email");

            if (await _userService.CheckUserExistsByUsername(model.Username))
                return BadRequest("User is already registered with such username");

            User user = new User()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                AuthType = AuthType.Normal
            };

            var result = await _userService.CreateUser(user, model.Password);

            // here
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!await _roleService.CheckRoleExists(AuthConstants.UserRoles.User))
            {
                await _roleService.CreateRole(new Role { Name = AuthConstants.UserRoles.User });
            }

            await _userService.AddUserToRole(user, AuthConstants.UserRoles.User);

            return Ok("You successfully registered!");
        }
    }
}
