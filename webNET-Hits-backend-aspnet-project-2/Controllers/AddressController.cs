using backtestDevelop.Models;
using backtestDevelop.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace backtestDevelop.Controllers
{
    [ApiController]
    [Route("api/address")]
    public class AddressController : ControllerBase
    {
        private readonly AddressService _addressService;

        public AddressController(AddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public IActionResult Search(int parentObjectId, string? query)
        {
            try
            {
                var response = _addressService.SearchAddress(parentObjectId, query);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Произошла ошибка сервера " + ex.ToString());
            }
        }

        [HttpGet("chain")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public IActionResult SearchChain(string objectGuid)
        {
            try
            {
                var response = _addressService.SearchAddressChain(Guid.Parse(objectGuid));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Произошла ошибка сервера " + ex.ToString());
            }
        }
    }

}
