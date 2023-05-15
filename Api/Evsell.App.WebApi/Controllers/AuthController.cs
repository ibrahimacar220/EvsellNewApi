using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Evsell.App.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [Route("Login")]

        public string Login(AuthLoginDto dto)
        {
            //var claims = new[]
            //{
            //    new Claim(ClaimTypes.Name,username),
            //    new Claim(JwtRegisteredClaimNames.Email,username),
            //    new Claim(ClaimTypes.Upn,password)
            //};
            //var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Stc.JwtKey));
            //var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            //var jwtSecurityToken = new JwtSecurityToken(
            //    issuer: Stc.JwtIssuer,
            //    audience: null,
            //    claims: claims,
            //    expires: DateTime.Now.AddDays(20),
            //    notBefore: DateTime.Now,
            //    signingCredentials: credentials
            //    );
            //var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            //return token;
            return JwtTokenBuilder.BuildToken(dto.Username);
        }
        //[HttpGet("ValidateToken")]
        //public bool loginController(string token)
        //{
        //    var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingkey));
        //    try
        //    {
        //        JwtSecurityTokenHandler handler = new();
        //        handler.ValidateToken(token, new TokenValidationParameters()
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = securitykey,
        //            ValidateLifetime = true,
        //            ValidateAudience = false,
        //            ValidateIssuer = false,
        //        }, out SecurityToken validatedToken);


        //        var jwtToken = (JwtSecurityToken)validatedToken;

        //        var claims = jwtToken.Claims.ToList();
        //        return true;
        //    }
        //    catch (System.Exception)
        //    {
        //        return false;
        //    }

        //}
    }

    public class JwtTokenBuilder
    {
        public static string BuildToken(string userName)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, userName)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Stc.JwtKey));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
            Stc.JwtIssuer,
            Stc.JwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddYears(50),
            signingCredentials: creds
        );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    public class AuthLoginDto
    {
        [Required]
        public string Username { get; set; }
    }
}
