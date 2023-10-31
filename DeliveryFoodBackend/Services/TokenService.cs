using DeliveryFoodBackend.Data;
using DeliveryFoodBackend.Services.Interfaces;

namespace DeliveryFoodBackend.Services
{
    public class TokenService : ITokenService
    {
        private readonly AppDbContext _context;

        public TokenService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CheckToken(string token)
        {
            var invalidToken = _context.Tokens.Where(x => x.InvalideToken == token).FirstOrDefault();

            if (invalidToken != null)
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
