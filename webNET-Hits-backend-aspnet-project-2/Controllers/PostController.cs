using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;
using webNET_Hits_backend_aspnet_project_2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace webNET_Hits_backend_aspnet_project_2.Controllers
{
    [ApiController]
    [Route("api/post")]
    public class PostController : Controller
    {
        private readonly PostService _postService;
        private readonly UsersService _userService;
        private readonly AddressService _addressService;
        private readonly TagService _tagService;
        private readonly IConfiguration _configuration;

        public PostController(IConfiguration configuration, PostService postService, UsersService userService, AddressService addressService, TagService tagService)
        {
            _configuration = configuration;
            _postService = postService;
            _userService = userService;
            _addressService = addressService;
            _tagService = tagService;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> CreateUsPersPost([FromBody] CreatePostModel userPost)
        {
            try
            {
                Guid userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                if (!_userService.IsUserAuthenticated(userId, out var errorMessage))
                {
                    return BadRequest(new { errorMessage });
                }

                if (userPost.Title == null)
                {
                    return BadRequest("Название поста должно БЫТЬ!!!");
                }

                if (userPost.Description == null)
                {
                    return BadRequest("Пост должен ИМЕТЬ описание!!!");
                }

                if (userPost.ReadingTime == null || userPost.ReadingTime < 0)
                {
                    return BadRequest("А сколько читать ваш пост???");
                }

                if (!_addressService.checkIsCorrectAddress(userPost.AddressId))
                {
                    return BadRequest("Адрес некоректный");
                }

                if (!_tagService.CheckIsCorrectTags(userPost.Tags))
                {
                    return BadRequest("У вас не те теги");
                }

                _postService.CreatePersonalUserPost(userPost, userId);
                return Ok("Пост успешно создан");
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
