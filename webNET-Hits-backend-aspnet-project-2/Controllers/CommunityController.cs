using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;
using webNET_Hits_backend_aspnet_project_2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using webNET_Hits_backend_aspnet_project_2.Models.AnotherModels;

//ДОБАВИТЬ ВОЗМОЖНОСТЬ ПОДПИСАТЬ, БУДУЧИ АДМИНОМ, ПОЛЬЗОВАТЕЛЯ НА ЗАКРЫТУЮ COMMUNITY
//ДОБАВИТЬ ВОЗМОЖНОСТЬ СОЗДАТЬ СВОЮ COMMUNITY

namespace webNET_Hits_backend_aspnet_project_2.Controllers
{
    [ApiController]
    [Route("api/community")]
    public class CommunityController : Controller
    {
        private readonly CommunityService _communityService;
        private readonly PostService _postService;
        private readonly UsersService _userService;
        private readonly AddressService _addressService;
        private readonly TagService _tagService;
        private readonly IConfiguration _configuration;


        public CommunityController(IConfiguration configuration, CommunityService communityService, PostService postService, UsersService userService, AddressService addressService, TagService tagService)
        {
            _configuration = configuration;
            _communityService = communityService;
            _postService = postService;
            _userService = userService;
            _addressService = addressService;
            _tagService = tagService;
        }

        [HttpPost("{id}/post")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> CreateCommunityPost([FromBody] CreatePostModel communPost, Guid id)
        {
            try
            {
                Guid userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                if (!_userService.IsUserAuthenticated(userId, out var errorMessage))
                {
                    return BadRequest(new { errorMessage });
                }

                if (communPost.Title == null)
                {
                    return BadRequest("Название поста должно БЫТЬ!!!");
                }

                if (communPost.Description == null)
                {
                    return BadRequest("Пост должен ИМЕТЬ описание!!!");
                }

                if (communPost.ReadingTime == null || communPost.ReadingTime < 0)
                {
                    return BadRequest("А сколько читать ваш пост???");
                }

                if (id == null)
                {
                    return BadRequest("А где community?");
                }

                if (!_communityService.CheckAdmin(userId, id))
                {
                    return BadRequest("Вы не являетесь админом данной группы");
                }

                if (!_addressService.checkIsCorrectAddress(communPost.AddressId))
                {
                    return BadRequest("Адрес некоректный");
                }

                if (!_tagService.CheckIsCorrectTags(communPost.Tags))
                {
                    return BadRequest("У вас не те теги");
                }

                _communityService.CreateCommunityPost(communPost, userId, id);
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
