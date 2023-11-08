namespace DeliveryFoodBackend.Services.Interfaces
{
    public interface ITokenService
    {
        Task CheckToken(string token);
    }
}
