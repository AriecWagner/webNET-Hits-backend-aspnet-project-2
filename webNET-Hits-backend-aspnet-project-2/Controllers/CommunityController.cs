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
        private readonly UsersService _userService;
        private readonly AddressService _addressService;
        private readonly TagService _tagService;
        private readonly IConfiguration _configuration;


        public CommunityController(IConfiguration configuration, CommunityService communityService, PostService postService, UsersService userService, AddressService addressService, TagService tagService)
        {
            _configuration = configuration;
            _communityService = communityService;
            _userService = userService;
            _addressService = addressService;
            _tagService = tagService;
        }

        [HttpGet]


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public IActionResult GetCommunties()
        {
            try
            {
                var communities = _communityService.GetCommunities();

                if (communities != null)
                {
                    return Ok(communities);
                }
                else
                {
                    return NotFound("Что-то пошло не так");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Произошла ошибка сервера");
            }
        }

        [HttpGet("my")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public IActionResult GetUsCommunyList()
        {
            try
            {
                Guid userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                if (!_userService.IsUserAuthenticated(userId, out var errorMessage))
                {
                    return BadRequest(new { errorMessage });
                }

                return Ok(_communityService.GetMembershipsUser(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Произошла ошибка сервера");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public IActionResult GetInformationAboutCommunity(Guid id)
        {
            try
            {
                var community = _communityService.GetCommunityDTO(id);

                if (community != null)
                {
                    return Ok(community);

        public IActionResult GetCommunties()
        {
            try
            {
                var communities = _communityService.GetCommunities();

                if (communities != null)
                {
                    return Ok(communities);
                }
                else
                {
                    return NotFound("Что-то пошло не так");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Произошла ошибка сервера");
            }
        }


        [HttpGet("my")]

        [HttpGet("{id}/posts")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public IActionResult GetUsCommunyList()
        {
            try
            {
                Guid userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                if (!_userService.IsUserAuthenticated(userId, out var errorMessage))
                {
                    return BadRequest(new { errorMessage });
                }

                return Ok(_communityService.GetMembershipsUser(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Произошла ошибка сервера");
            }
        }
 GetCommunityEndpoints
        [HttpGet("{id}/posts")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]

 GetCommunityEndpoints
        public IActionResult GetCommunityPosts(
            Guid id,
            [FromQuery] List<Guid>? tags,
            [FromQuery] PostSorting? sorting,
            [FromQuery] int page = 1,
            [FromQuery] int size = 5)
        {
            try
            {
                Guid userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                if (!_userService.IsUserAuthenticated(userId, out var errorMessage))
                {
                    return BadRequest(new { errorMessage });
                }

                FilterOptionsCommunity filterOptions = new FilterOptionsCommunity
                {
                    CommunityId = id,
                    Tags = tags,
                    Sorting = sorting,
                    Page = page,
                    Size = size
                };

                var community = _communityService.GetCommunityDTO(id);

                var result = _communityService.GetListOfAvalibleCommunityPosts(filterOptions, userId);
                var structResult = new
                {
                    Posts = result.Item1,
                    Pagination = result.Item2
                };
                return Ok(structResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Произошла ошибка сервера");
            }
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

 GetCommunityEndpoints
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

        [HttpPost("{id}/subscribe")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> SubscribeToCommunAsUs(Guid id)
        {
            try
            {
                Guid userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                if (!_userService.IsUserAuthenticated(userId, out var errorMessage))
                {
                    return BadRequest(new { errorMessage });
                }

                if (id == null)
                {
                    return BadRequest("А где community?");
                }

                if (!_communityService.CheckCommunityExesists(id))
                {
                    return BadRequest("Такой группы не существует, дружок");
                }

                if (_communityService.CheckOpenityOfCommunity(id))
                {
                    return BadRequest("Это не для таких как ты");
                }

                if (_communityService.CheckMembershipInCommunity(userId, id))
                {
                    return BadRequest("Вы уже состоите в этом сообществе");
                }

                _communityService.SubscribeToCommunityAsUser(userId, id);
                return Ok("Вы успешно подписаны");
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

        [HttpDelete("{id}/unsubscribe")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> UnsubscribeToCommunAsUs(Guid id)
        {
            try
            {
                Guid userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                if (!_userService.IsUserAuthenticated(userId, out var errorMessage))
                {
                    return BadRequest(new { errorMessage });
                }

                if (id == null)
                {
                    return BadRequest("А где community?");
                }

                if (!_communityService.CheckCommunityExesists(id))
                {
                    return BadRequest("Такой группы не существует, дружок");
                }

                if (!_communityService.CheckMembershipInCommunity(userId, id))
                {
                    return BadRequest("Вы не состоите в этом сообществе, чтобы отписываться от него");
                }

                _communityService.UnsubscribeFromCommunityAsUser(userId, id);
                return Ok("Вы успешно отписаны:(");
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
