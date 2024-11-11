using System.IdentityModel.Tokens.Jwt;

namespace SocialFrontEnd.Helper
{
    public class JwtHelper
    {
        public static string GetEmailFromJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            // Validate that the token can be read as a valid JWT
            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);
                var emailClaim = jwtToken.Claims.FirstOrDefault(c =>
                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
                return emailClaim?.Value;
            }
            else
            {
                throw new ArgumentException("Invalid JWT token.");
            }
        }
    }
}
