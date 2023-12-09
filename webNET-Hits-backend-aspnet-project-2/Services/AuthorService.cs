using webNET_Hits_backend_aspnet_project_2.Data;
using webNET_Hits_backend_aspnet_project_2.Models.DbModels;

namespace webNET_Hits_backend_aspnet_project_2.Services
{
    public class AuthorService
    {
        private readonly AppDbContext _dbContext;

        public AuthorService(AppDbContext context)
        {
            _dbContext = context;
        }

    }
}
