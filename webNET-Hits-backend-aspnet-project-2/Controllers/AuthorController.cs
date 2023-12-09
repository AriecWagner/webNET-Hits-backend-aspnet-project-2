using webNET_Hits_backend_aspnet_project_2.Services;
using Microsoft.AspNetCore.Mvc;
using webNET_Hits_backend_aspnet_project_2.Models.AnotherModels;

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

        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetAuthorsList()
        {
            try
            {
                List<AuthorDTO> authors = _authorService.GetAuthorDTOs();
                return Ok(authors);
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
