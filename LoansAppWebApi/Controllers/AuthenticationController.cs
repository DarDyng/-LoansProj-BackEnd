using Google.Apis.Auth;
using LoansAppWebApi.Core.Constants;
using LoansAppWebApi.Core.Services;
using LoansAppWebApi.Models.Configuration;
using LoansAppWebApi.Models.DbModels;
using LoansAppWebApi.Models.DTO_s.Requests;
using LoansAppWebApi.Models.DTO_s.Resposnes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoansAppWebApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IConfiguration _configuration;
        private readonly JWTConfiguration jWTConfiguration;
        private readonly JwtGenerator jwtGenerator;

        public AuthenticationController(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IConfiguration configuration,
            IOptions<JWTConfiguration> options,
            JwtGenerator jwtGenerator)
        {
            _userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            this.jWTConfiguration = options.Value;
            this.jwtGenerator = jwtGenerator;
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

                return Ok(new AuthenticatedUserResposne
                {
                    Token = jwtGenerator.GenerateToken(payload.Email),
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
        public async Task<ActionResult> Authenticate(
            LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(AuthConstants.ClaimNames.Id, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
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

            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return BadRequest();

            User user = new User()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!await roleManager.RoleExistsAsync(AuthConstants.UserRoles.User))
            {
                await roleManager.CreateAsync(new Role { Name = AuthConstants.UserRoles.User });
            }

            await _userManager.AddToRoleAsync(user, AuthConstants.UserRoles.User);

            return Ok();
        }
    }
}
