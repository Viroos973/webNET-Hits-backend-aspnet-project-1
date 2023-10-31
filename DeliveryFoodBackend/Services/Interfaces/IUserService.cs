using DeliveryFoodBackend.DTO;

namespace DeliveryFoodBackend.Services.Interfaces
{
    public interface IUserService
    {
        Task<TokenResponse> RegisterUser(UserRegisterModel userRegisterModel);
        Task<TokenResponse> Login(LoginCredentials credentials);
        Task LogOut(string token);
        Task<UserDto> GetProfile(Guid id);
        Task EditProfile(UserEditModel editModel, Guid id);
    }
}
