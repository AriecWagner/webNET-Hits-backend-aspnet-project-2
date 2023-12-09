using webNET_Hits_backend_aspnet_project_2.Services;
using Microsoft.AspNetCore.Mvc;

namespace webNET_Hits_backend_aspnet_project_2.Controllers
{
    [ApiController]
    [Route("api/author")]
    public class AuthorController : Controller
    {
        private readonly AuthorService _authorService;
        private readonly IConfiguration _configuration;


        public AuthorController(IConfiguration configuration, AuthorService authorService)
        {
            _configuration = configuration;
            _authorService = authorService;
        }
    }
}
