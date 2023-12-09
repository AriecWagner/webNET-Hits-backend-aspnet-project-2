using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;
using webNET_Hits_backend_aspnet_project_2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet("{id}/role")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public IActionResult GetGreatestUsRole(Guid id)
        {
            try
            {
                Guid userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                if (!_userService.IsUserAuthenticated(userId, out var errorMessage))
                {
                    return BadRequest(new { errorMessage });
                }

                return Ok(_communityService.GetTheGreatestRole(id, userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Произошла ошибка сервера");
            }
        }
    }
}
