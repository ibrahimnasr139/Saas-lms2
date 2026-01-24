namespace Application.Contracts.Authentication
{
    public interface ITokenProvider
    {
        void GenerateJwtToken(ApplicationUser user);
        (string token, DateTime expiresOn) GenerateRefreshToken();
    }
}
