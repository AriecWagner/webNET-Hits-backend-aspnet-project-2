using webNET_Hits_backend_aspnet_project_2.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using webNET_Hits_backend_aspnet_project_2.Data;
using webNET_Hits_backend_aspnet_project_2.Models.AnotherModels;

namespace webNET_Hits_backend_aspnet_project_2.Services
{
    public class TagService
    {
        private readonly AppDbContext _dbContext;

        public TagService(AppDbContext context)
        {
            _dbContext = context;
        }

        public List<Tag> GetTags()
        {
            return _dbContext.Tags.ToList();
        }


    }
}
