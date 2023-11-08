using DeliveryFoodBackend.DTO;

namespace DeliveryFoodBackend.Services.Interfaces
{
    public interface IAddressService
    {
        Task<List<SearchAddressModel>> AddressChain(Guid objectGuid);
        Task<List<SearchAddressModel>> AddressSearch(int? parentObj, string? query);
    }
}
