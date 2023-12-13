using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;
using webNET_Hits_backend_aspnet_project_2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using webNET_Hits_backend_aspnet_project_2.Models.AnotherModels;
using System.IO;
using webNET_Hits_backend_aspnet_project_2.Migrations;

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
        private readonly CommunityService _communityService;
        private readonly IConfiguration _configuration;

        public PostController(IConfiguration configuration, PostService postService, UsersService userService, AddressService addressService, TagService tagService, CommunityService communityService)
        {
            _configuration = configuration;
            _postService = postService;
            _userService = userService;
            _addressService = addressService;
            _tagService = tagService;
            _communityService = communityService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetPosts(
            [FromQuery] List<Guid>? tags,
            [FromQuery] string? pathOfAuthor,
            [FromQuery] int? minRead,
            [FromQuery] int? maxRead,
            [FromQuery] PostSorting? sorting,
            [FromQuery] bool? onlyMyCommunities,
            [FromQuery] int page = 1,
            [FromQuery] int size = 5)
        {
            try
            {
                if (page < 1)
                {
                    return BadRequest("Извините, но числовое значение страницы может быть только натуральным числом");
                }

                Guid? userId;

                string nameIdentifier = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (nameIdentifier != null)
                {
                    userId = Guid.Parse(nameIdentifier);
                }
                else
                {
                    userId = null;
                }

                FilterOptions filterOptions = new FilterOptions
                {
                    Tags = tags,
                    PathOfAuthor = pathOfAuthor,
                    minRead = minRead,
                    maxRead = maxRead,
                    Sorting = sorting,
                    OnlyMyCommunities = onlyMyCommunities,
                    Page = page,
                    Size = size
                };

                var result = _postService.GetListOfAvaliblePosts(filterOptions, userId);
                var structResult = new
                {
                    Posts = result.Item1,
                    Pagination = result.Item2
                };

                if (structResult.Pagination.Current > structResult.Pagination.Count)
                {
                    return BadRequest("Вы зашли слишком далеко");
                }

                return Ok(structResult);
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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetPost(Guid id)
        {
            try
            {
                Guid? userId;

                string nameIdentifier = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (nameIdentifier != null)
                {
                    userId = Guid.Parse(nameIdentifier);
                }
                else
                {
                    userId = null;
                }

                if (!_postService.PostExests(id))
                {
                    return NotFound("Такого поста не существует");
                }

                var result = _postService.GetConcretePost(id, userId);
                var megaResult = new
                {
                    Comments = result.Item1,
                    Posts = result.Item2
                };

                return Ok(megaResult);
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

        [HttpPost("{postId}/like")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> AddLikeToConretePost(Guid postId)
        {
            try
            {
                Guid userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                if (!_userService.IsUserAuthenticated(userId, out var errorMessage))
                {
                    return BadRequest(new { errorMessage });
                }

                if (_communityService.CheckOpenityOfCommunity(postId) && _communityService.CheckMembershipInCommunity(userId, postId))
                {
                    return StatusCode(403, "Это не для таких как ты сделано");
                }

                if (!_postService.PostExests(postId))
                {
                    return NotFound("Такого поста не существует");
                }

                if (_postService.UserHasLikeOnPost(userId, postId))
                {
                    return BadRequest("Пост уже лайкнут");
                }

                _postService.AddLikeToPost(userId, postId);

                return Ok("Пост успешно лайкнут");
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

        [HttpDelete("{postId}/like")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> DeleteLikeFromConretePost(Guid postId)
        {
            try
            {
                Guid userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                if (!_userService.IsUserAuthenticated(userId, out var errorMessage))
                {
                    return BadRequest(new { errorMessage });
                }

                if (_communityService.CheckOpenityOfCommunity(postId) && _communityService.CheckMembershipInCommunity(userId, postId))
                {
                    return StatusCode(403, "Это не для таких как ты сделано");
                }

                if (!_postService.PostExests(postId))
                {
                    return NotFound("Такого поста не существует");
                }

                if (!_postService.UserHasLikeOnPost(userId, postId))
                {
                    return BadRequest("Лайк с поста уже убран");
                }

                _postService.DeleteLikeFromPost(userId, postId);

                return Ok("Лайк успешно убран");
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
