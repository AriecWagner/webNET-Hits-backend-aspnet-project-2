using webNET_Hits_backend_aspnet_project_2.Data;
using webNET_Hits_backend_aspnet_project_2.Models.DbModels;
using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;
using Microsoft.Extensions.Hosting;
using static System.Net.Mime.MediaTypeNames;

namespace webNET_Hits_backend_aspnet_project_2.Services
{
    public class PostService
    {
        private readonly AppDbContext _dbContext;

        public PostService(AppDbContext context)
        {
            _dbContext = context;
        }

        
    }
}
