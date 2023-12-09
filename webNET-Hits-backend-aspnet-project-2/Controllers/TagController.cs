using webNET_Hits_backend_aspnet_project_2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace webNET_Hits_backend_aspnet_project_2.Controllers
{
    [ApiController]
    [Route("api/tag")]
    public class TagController : Controller
    {
        private readonly TagService _tagService;
        private readonly IConfiguration _configuration;

        public TagController(IConfiguration configuration, TagService tagService)
        {
            _configuration = configuration;
            _tagService = tagService;
        }

        
    }
}
