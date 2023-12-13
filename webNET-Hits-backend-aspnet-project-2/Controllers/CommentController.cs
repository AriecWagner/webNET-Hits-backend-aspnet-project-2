using webNET_Hits_backend_aspnet_project_2.Services;
using Microsoft.AspNetCore.Mvc;
using webNET_Hits_backend_aspnet_project_2.Models.AnotherModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using webNET_Hits_backend_aspnet_project_2.Migrations;


namespace webNET_Hits_backend_aspnet_project_2.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : Controller
    {
        private readonly UsersService _userService;
        private readonly CommentService _commentService;
        private readonly PostService _postService;
        private readonly CommunityService _communityService;
        private readonly IConfiguration _configuration;


        public CommentController(IConfiguration configuration, CommentService commentService, UsersService userService, PostService postService, CommunityService communityService)
        {
            _configuration = configuration;
            _userService = userService;
            _postService = postService;
            _commentService = commentService;
            _communityService = communityService;
        }

        [HttpGet("comment/{id}/tree")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetCommentsTree(Guid id)
        {
            try
            {
                if (!_commentService.CommentExists(id))
                {
                    return NotFound("Такого коммента не существует");
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

                Guid? communityId = _commentService.GetCommunityIdByCommentId(id);

                if (communityId != null && _communityService.CheckOpenityOfCommunity(communityId) && _communityService.CheckMembershipInCommunity(userId, communityId))
                {
                    return StatusCode(403, "Это не для таких как ты сделано");
                }

                return Ok(_commentService.GetAllNestedComments(id));
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

        [HttpPost("/api/post/{id}/comment")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> AddComment([FromBody] CommentInput comment, Guid id)
        {
            try
            {
                Guid userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                if (!_userService.IsUserAuthenticated(userId, out var errorMessage))
                {
                    return BadRequest(new { errorMessage });
                }

                if (comment.Content == null || comment.Content == "")
                {
                    return BadRequest("Комментарий пуст");
                }

                if (comment.Content.Length > 1000)
                {
                    return BadRequest("Короче изъясняйся, ежи");
                    //return BadRequest("Комментарий пуст");
                }

                if (!_postService.PostExests(id))
                {
                    return NotFound("Такого поста не существует");
                }

                if (!_commentService.checkParentIsCorrect(comment.ParentId))
                {
                    return NotFound("Такого комментария не существует");
                }

                Guid? communityId = _commentService.GetCommunityIdByCommentId(id);

                if (communityId != null && _communityService.CheckOpenityOfCommunity(communityId) && _communityService.CheckMembershipInCommunity(userId, communityId))
                {
                    return StatusCode(403, "НЕЕЕЕЕЛЬЗЯ");
                }

                _commentService.AddCommentToPost(comment, id, userId);
                return Ok("Комментарий успешно написан");
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

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> EditComment([FromBody] CommentContent comment, Guid id)
        {
            try
            {
                Guid userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                if (!_userService.IsUserAuthenticated(userId, out var errorMessage))
                {
                    return BadRequest(new { errorMessage });
                }

                if (comment.Content == null || comment.Content == "")
                {
                    return BadRequest("Вы отредактировали комментарий до удаления:)");
                    //return BadRequest("Комментарий пуст");
                }

                if (comment.Content.Length > 1000)
                {
                    return BadRequest("Короче изъясняйся, ежи");
                    //return BadRequest("Комментарий пуст");
                }

                if (!_commentService.CommentExists(id))
                {
                    return NotFound("Такого комментария не существует");
                }

                if (_commentService.CommentAlreadyDeleted(id))
                {
                    return BadRequest("Нельзя изменить то, чего нет");
                }

                if (!_commentService.CheckOwnerOfComment(id, userId))
                {
                    return StatusCode(403, "Так делать нехорошо");
                }

                _commentService.EditComment(comment.Content, id);
                return Ok("Комментарий успешно отредактирован");
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

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            try
            {
                Guid userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                if (!_userService.IsUserAuthenticated(userId, out var errorMessage))
                {
                    return BadRequest(new { errorMessage });
                }

                if (!_commentService.CommentExists(id))
                {
                    return NotFound("Такого комментария не существует");
                }

                if (_commentService.CommentAlreadyDeleted(id))
                {
                    return BadRequest("Нельзя удалить то, чего нет");
                }

                if (!_commentService.CheckOwnerOfComment(id, userId))
                {
                    return StatusCode(403, "Так делать нехорошо");
                }

                _commentService.DeleteComment(id);
                return Ok("Комментарий успешно удалён");
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
