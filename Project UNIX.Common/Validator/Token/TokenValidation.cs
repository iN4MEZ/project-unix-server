using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_UNIX.Common.Validator.Token
{
    public class TokenValidation 
    {

        public TokenValidation() { 
            
        }

        public JwtSecurityToken DecodeJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // เข้าถึงข้อมูลใน token
            var claims = jwtToken.Claims;
            var audience = jwtToken.Audiences;
            var issuer = jwtToken.Issuer;

            return jwtToken;
        }

        public bool ValidateJwt(string token, string secretKey)
        {
            // Define validation parameters
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateIssuer = false, // You can set these to true if you want to validate issuer
                ValidateAudience = false // and audience
            };

            try
            {
                // Try to validate the token
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (SecurityTokenException)
            {
                // Token validation failed
                return false;
            }
        }

        public bool IsTokenValid(string token, string secretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateIssuer = false, // You can set these to true if you have specific requirements
                ValidateAudience = false
            };

            try
            {
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    // Check token expiration
                    return jwtToken.ValidTo > DateTime.UtcNow;
                }
                return false;
            }
            catch (Exception)
            {
                // Token validation failed
                return false;
            }
        }
    }
}
