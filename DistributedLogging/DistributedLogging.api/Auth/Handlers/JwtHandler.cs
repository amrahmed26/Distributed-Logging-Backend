using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DistributedLogging.Common.Settings;
using DistributedLogging.api.Auth.Constants;
using DistributedLogging.common;

namespace DistributedLogging.api.Auth.Handlers
{
    public class JwtHandler
    {
        JWTSettings jwtSettings;
        public JwtHandler()
        {
            jwtSettings = SettingsManager.JWTSettings;
        }
        public SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(jwtSettings.SecurityKey);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        public List<Claim> GetClaims(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(CustomClaimTypes.Name, user.UserName),
                new Claim(CustomClaimTypes.Email, user.Email)
            };
            return claims;
        }
        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings.ValidIssuer,
                audience: jwtSettings.ValidAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings.ExpiryInMinutes)),
                signingCredentials: signingCredentials);
            return tokenOptions;
        }
    }
}
