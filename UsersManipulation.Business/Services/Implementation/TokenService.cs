using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsersManipulation.Business.Exceptions;
using UsersManipulation.Business.Services.Common;
using UsersManipulation.Data.Repositories.Contracts;

namespace UsersManipulation.Business.Services.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public TokenService(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync(string email)
        {
            var user = await _userRepository.FindByEmailAsync(email);

            if (user == null)
            {
                throw new NotFoundException(nameof(user));
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            return claims;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            var JWTToken = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(Convert.ToInt32(_configuration["AuthSettings:AccessTokenHours"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            string AccessToken = new JwtSecurityTokenHandler().WriteToken(JWTToken);

            return AccessToken;
        }
    }
}
