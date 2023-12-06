using Microsoft.AspNetCore.Mvc;
using webNET_Hits_backend_aspnet_project_2.Services;
using Microsoft.AspNetCore.Authorization;
using webNET_Hits_backend_aspnet_project_2.Models.InputModels;

namespace webNET_Hits_backend_aspnet_project_2.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class UsersController : Controller
    {
        private readonly UsersService _userService;
        public UsersController(UsersService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel userModel)
        {
            try
            {
                if (!_userService.FUllNameIsValid(userModel.FullName))
                {
                    return BadRequest("Вы либо из Казахстана, либо принц, либо неправильно написали ФИО");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                // Добавьте другие сведения о исключении при необходимости
                throw;
            }
        }

        
    }
}
