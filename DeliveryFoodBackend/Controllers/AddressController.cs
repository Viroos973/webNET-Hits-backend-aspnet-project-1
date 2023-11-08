using DeliveryFoodBackend.DTO;
using DeliveryFoodBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryFoodBackend.Controllers
{
    [ApiController]
    [Route("api/address")]
    public class AddressController : ControllerBase
    {
        public readonly IAddressService _addressSevise;
        public readonly ITokenService _tokenService;

        public AddressController(IAddressService addressSevise, ITokenService tokenService)
        {
            _addressSevise = addressSevise;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> AddressChain(int? parentObj, string? query)
        {
            try
            {
                var addressSearch = await _addressSevise.AddressSearch(parentObj, query);
                return Ok(addressSearch);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response
                {
                    Status = "Error",
                    Message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("chain")]
        public async Task<IActionResult> AddressChain(Guid objectGuid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var addressChain = await _addressSevise.AddressChain(objectGuid);
                return Ok(addressChain);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(new Response
                {
                    Status = "Error",
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response
                {
                    Status = "Error",
                    Message = ex.Message
                });
            }
        }
    }
}
