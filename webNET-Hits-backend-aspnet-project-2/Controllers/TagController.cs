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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public IActionResult GetProfile()
        {
            try
            {
                var tags = _tagService.GetTags();

                if (tags != null)
                {
                    return Ok(tags);
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
    }
}
