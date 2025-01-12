using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagementService.DAL.Context;
using UserManagementService.Enums;
using UserManagementService.Models;

namespace UserManagementService.TokenHandlers
{
    public class TokenHandler : ITokenHandler
    {
        public string GenerateGuidBasedToken()
        {
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return token;
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("Role", Role.User.ToString()),
                new Claim(ClaimTypes.Role, Role.User.ToString())
            };

            if (user.Role == Role.Admin)
            {
                claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim("Role", Role.Admin.ToString()),
                    new Claim("Role", Role.User.ToString()),
                    new Claim(ClaimTypes.Role, Role.Admin.ToString()),
                    new Claim(ClaimTypes.Role, Role.User.ToString())
                };
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("7CCA9FF4A0D5427FBB382974B5E2AC092BB38974B5E4B5E2AC092BB38974B5E2AC092AC09"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "issuer",
                audience: "audience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
