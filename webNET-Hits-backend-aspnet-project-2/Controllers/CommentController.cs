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
    }
}
