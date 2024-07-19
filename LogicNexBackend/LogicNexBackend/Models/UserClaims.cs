using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LogicNexBackend.Models
{
    public class UserClaims
    {
        public UserClaims(IHttpContextAccessor httpContext)
        {
            IEnumerable<Claim> claims = httpContext.HttpContext.User.Claims;
            if (claims != null){ //  authenticated
                Email = claims.First(x=>x.Type == JwtRegisteredClaimNames.Email).Value;
            }

        }
        public string? Email { get; set; }
    }
}
