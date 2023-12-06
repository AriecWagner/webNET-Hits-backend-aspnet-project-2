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
        public async Task<IActionResult> Register([FromBody] InputUserRegisterModel userModel)
        {
            try
            {
                if (!_userService.FUllNameIsValid(userModel.FullName))
                {
                    return BadRequest("Вы либо из Казахстана, либо принц, либо неправильно написали ФИО");
                }

                if (!_userService.PasswordIsValid(userModel.Password, out var errorMessage))
                {
                    return BadRequest(new { Message = errorMessage });
                }

                if (!_userService.EmailIsValid(userModel.Email))
                {
                    return BadRequest("Вы неправильно ввели email");
                }

                if (!_userService.BirthDateIsValid(userModel.BirthDate))
                {
                    return BadRequest("Вы или слишком юны или слишком стары");
                }

                if (!_userService.GenderIsValid(userModel.Gender))
                {
                    return BadRequest("Есть только два гендера");
                }

                if (!_userService.PhoneNumberIsValid(userModel.PhoneNumber))
                {
                    return BadRequest("Что это за номерок у вас интересный?");
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
