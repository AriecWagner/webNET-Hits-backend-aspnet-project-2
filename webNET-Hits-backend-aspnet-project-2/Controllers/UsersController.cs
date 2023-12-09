using Microsoft.AspNetCore.Mvc;
using webNET_Hits_backend_aspnet_project_2.Services;
using Microsoft.AspNetCore.Authorization;
using webNET_Hits_backend_aspnet_project_2.Models.InputModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace webNET_Hits_backend_aspnet_project_2.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class UsersController : Controller
    {
        private readonly UsersService _userService;
        private readonly IConfiguration _configuration;
        public UsersController(IConfiguration configuration, UsersService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Register([FromBody] InputUserRegisterModel user)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    return BadRequest("Вы уже авторизованы.");
                }

                if (!_userService.FUllNameIsValid(user.FullName))
                {
                    return BadRequest("Вы либо из Казахстана, либо принц, либо неправильно написали ФИО");
                }

                if (!_userService.PasswordIsValid(user.Password, out var errorMessage))
                {
                    return BadRequest(new { Message = errorMessage });
                }

                if (!_userService.EmailIsValid(user.Email))
                {
                    return BadRequest("Вы неправильно ввели email");
                }

                if (!_userService.BirthDateIsValid(user.BirthDate))
                {
                    return BadRequest("Вы или слишком юны или слишком стары");
                }

                if (!_userService.GenderIsValid(user.Gender))
                {
                    return BadRequest("Есть только два гендера");
                }

                if (!_userService.PhoneNumberIsValid(user.PhoneNumber))
                {
                    return BadRequest("Что это за номерок у вас интересный?");
                }

                if (_userService.EmailExists(user.Email))
                {
                    return BadRequest("К великому сожалению, пользователь с таким email уже существует");
                }

                AuthOptions authentification = new AuthOptions(_configuration);
                var token = GenerateToken(user, authentification);

                if (token == null)
                {
                    return BadRequest(new { Error = "Не удалось зарегестрировать пользователя" });
                }

                return Ok(new { token = token });
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

        private string GenerateToken(InputUserRegisterModel userModel, AuthOptions authentification)
        {
            var newUser = _userService.RegisterUser(userModel);

            if (newUser == null)
            {
                return null;
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, newUser.Id.ToString())
                };

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            _userService.CreateOrUpdateTokenInfo(newUser.Id, encodedJwt);

            return encodedJwt;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public IActionResult Login([FromBody] UserLoginModel model)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    return BadRequest("Вы уже авторизованы.");
                }

                UserData user = _userService.Authenticate(model.Email, model.Password);

                if (user == null)
                {
                    return BadRequest("Данные неверны");
                }

                AuthOptions authentification = new AuthOptions(_configuration);

                var token = GenerateToken(user, authentification);

                if (token == null)
                {
                    return BadRequest(new { Error = "Не удалось зарегестрировать пользователя" });
                }

                return Ok(new { token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Произошла ошибка сервера" + "\n" + ex.ToString());
            }
        }

        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public IActionResult Logout()
        {
            try
            {

                // Получение идентификатора пользователя из токена
                Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                if (!_userService.IsUserAuthenticated(userId, out var errorMessage))
                {
                    return BadRequest(new { errorMessage });
                }

                // Аннулирование токена для пользователя (в вашем методе сервиса)
                _userService.RevokeToken(userId);

                return Ok("Выход успешно выполнен");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Произошла ошибка сервера" + "\n" + ex.ToString());
            }
        }

        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public IActionResult GetProfile()
        {
            try
            {
                Guid userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                if (!_userService.IsUserAuthenticated(userId, out var errorMessage))
                {
                    return BadRequest(new { errorMessage });
                }

                if (userId != null)
                {
                    var user = _userService.GetUser(userId);

                    if (user != null)
                    {
                        var response = new
                        {
                            id = user.Id,
                            createTime = user.CreateTime,
                            fullName = user.FullName,
                            birthDate = user.BirthDate,
                            gender = user.Gender,
                            email = user.Email,
                            phoneNumber = user.PhoneNumber,
                        };

                        return Ok(response);
                    }
                    else
                    {
                        return NotFound("Пользователь не найден");
                    }
                }
                else
                {
                    return NotFound("Где Id?");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Произошла ошибка сервера");
            }
        }

        [HttpPut("profile")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public IActionResult EditProfile([FromBody] UserProfileEditModel userProfileModel)
        {
            try
            {
                // Получение идентификатора пользователя из токена
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                if (!_userService.IsUserAuthenticated(userId, out var errorMessage))
                {
                    if (errorMessage == "Вы не авторизованы")
                    {
                        return Unauthorized(new { errorMessage });
                    }
                    else if (errorMessage == "Мы не нашли такого пользователя")
                    {
                        return NotFound(new { errorMessage });
                    }
                }

                if (!_userService.EmailIsValid(userProfileModel.Email))
                {
                    return BadRequest("Вы неправильно ввели email");
                }

                if (_userService.EmailForEditExists(userProfileModel.Email, userId))
                {
                    return BadRequest("К великому сожалению, пользователь с таким email уже существует");
                }

                if (!_userService.FUllNameIsValid(userProfileModel.FullName))
                {
                    return BadRequest("Вы либо из Казахстана, либо принц, либо неправильно написали ФИО");
                }

                if (!_userService.BirthDateIsValid(userProfileModel.BirthDate))
                {
                    return BadRequest("Вы или слишком юны или слишком стары");
                }

                if (!_userService.GenderIsValid(userProfileModel.Gender))
                {
                    return BadRequest("Есть только два гендера");
                }

                if (!_userService.PhoneNumberIsValid(userProfileModel.PhoneNumber))
                {
                    return BadRequest("Что это за номерок у вас интересный?");
                }


                // Редактирование профиля пользователя (в методе сервиса)
                var result = _userService.EditUserProfile(userId, userProfileModel);

                if (result)
                {
                    return Ok("Профиль успешно обновлен");
                }
                else
                {
                    return BadRequest("Не удалось обновить профиль пользователя.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Произошла ошибка сервера" + "\n" + ex.ToString());
            }
        }
    }
}
