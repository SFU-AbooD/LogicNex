using LogicNexBackend.SecretModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace LogicNexBackend.utils
{
    public static class Utils
    {
        public static void DeleteAuthCookie(this HttpResponse res) {
            res.Cookies.Append("__Host-Forbidden-Token", "", new CookieOptions()
            {
                Secure = true,
                SameSite = SameSiteMode.None,
                HttpOnly = true,
                IsEssential = true,
                MaxAge = TimeSpan.FromDays(-1)
            });
            res.Cookies.Append("__Host-Forbidden-refreshToken","", new CookieOptions()
            {
                Secure = true,
                SameSite = SameSiteMode.None,
                HttpOnly = true,
                IsEssential = true,
                MaxAge = TimeSpan.FromDays(-1)
            });
        }
        public static void RegisterAuthCookie(this HttpResponse res,string token,string refresh_token)
        {
            res.Cookies.Append("__Host-Forbidden-Token", token, new CookieOptions()
            {
                Secure = true,
                SameSite = SameSiteMode.None,
                HttpOnly = true,
                IsEssential=true,
                MaxAge = TimeSpan.FromDays(10)
            });
            res.Cookies.Append("__Host-Forbidden-refreshToken", refresh_token, new CookieOptions()
            {
                Secure = true,
                SameSite = SameSiteMode.None,
                HttpOnly = true,
                IsEssential = true,
                MaxAge = TimeSpan.FromDays(7)
            });
        }
        
        public static ClaimsPrincipal? Validate_token(string? token, IConfiguration _configuration) {
            if (token == null)
                return null;
            TokenValidationParameters parameters = new TokenValidationParameters() { 
                ValidateIssuer = true,
                ValidateAudience = true,    
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["JWT:validIssuer"],
                ValidAudience = _configuration["JWT:validAudience"],
                IssuerSigningKey = new  SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretJwtKey"]!)),
            };
            JwtSecurityTokenHandler token_handler = new JwtSecurityTokenHandler();
            try {
                var principal = token_handler.ValidateToken(token, parameters, out SecurityToken sec);
                return principal;
            }catch (Exception) {
                return null;
            }
        }
        public static string GenerateRefresh_token(IConfiguration _configuration, string email, string token_key)
        {
            Claim[] claims = new Claim[] {
                new Claim(JwtRegisteredClaimNames.Email,email),
                new Claim(JwtRegisteredClaimNames.UniqueName,token_key)
            };
            SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretAuthModel.Key));
            SigningCredentials Credentials = new(SecurityKey, SecurityAlgorithms.HmacSha512);

            JwtSecurityToken refresh_token = new JwtSecurityToken(
                    issuer: _configuration["JWT:validIssuer"],
                    audience: _configuration["JWT:validAudience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(7),
                    signingCredentials: Credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(refresh_token);
        }
        public static string Write_token(IConfiguration _configuration,string email) {
            SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretAuthModel.Key));
            SigningCredentials Credentials = new(SecurityKey, SecurityAlgorithms.HmacSha512);
            Claim[] claims = new Claim[] {
                new Claim(JwtRegisteredClaimNames.Email,email),
            };
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["JWT:validIssuer"],
                claims: claims,
                audience: _configuration["JWT:validAudience"],
                expires: DateTime.UtcNow.AddDays(10),
                signingCredentials: Credentials
                );
            string login_token = new JwtSecurityTokenHandler().WriteToken(token);
            return login_token;
        }
        public static string md5_hash(string password) {
            using MD5 md5 = MD5.Create();
            byte[] inputs = UTF8Encoding.UTF8.GetBytes(password);
            byte[] hash = md5.ComputeHash(inputs);
            return Convert.ToHexString(hash);
        }
        public static string Generate_random_email_validation() {
            StringBuilder builder = new();
            Random rand = new();
            for (int i = 0; i < 120; i++) {
                builder.Append(Convert.ToChar(
                    Convert.ToInt32(Math.Floor(26 * rand.NextDouble()+65)) // between 0 and 1 so and between 26 + 65 at max which is all ascii characters
                    ));
            }
            return builder.ToString();
        }
    }
}
