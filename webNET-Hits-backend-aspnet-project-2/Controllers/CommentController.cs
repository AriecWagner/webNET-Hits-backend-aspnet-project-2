using webNET_Hits_backend_aspnet_project_2.Services;
using Microsoft.AspNetCore.Mvc;
using webNET_Hits_backend_aspnet_project_2.Models.AnotherModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace webNET_Hits_backend_aspnet_project_2.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : Controller
    {
        private readonly UsersService _userService;
        private readonly CommentService _commentService;
        private readonly PostService _postService;
        private readonly IConfiguration _configuration;


        public CommentController(IConfiguration configuration, CommentService commentService, UsersService userService, PostService postService)
        {
            _configuration = configuration;
            _userService = userService;
            _postService = postService;
            _commentService = commentService;
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

                if (comment.Content  == null || comment.Content == "") {
                    return BadRequest("Комментарий пуст");
                }

                if (comment.Content.Length > 1000)
                {
                    return BadRequest("Короче изъясняйся, ежи");
                    //return BadRequest("Комментарий пуст");
                }

                if (!_postService.PostExests(id))
                {
                    return BadRequest("Такого поста не существует");
                }

                if (!_commentService.checkParentIsCorrect(comment.ParentId))
                {
                    return BadRequest("Такого комментария не существует");
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
    }
}
