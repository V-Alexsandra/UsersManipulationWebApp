using System.Security.Claims;

namespace UsersManipulation.Business.Services.Common
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);

        Task<IEnumerable<Claim>> GetClaimsAsync(string email);
    }
}
