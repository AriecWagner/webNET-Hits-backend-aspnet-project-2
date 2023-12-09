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

        public bool CheckIsCorrectTags(List<Guid>? tags)
        {
            if (tags == null)
            {
                return true;
            }

            var tagsList = _dbContext.Tags.ToList();
            foreach (var tag in tags)
            {
                bool flag = false;

                foreach (var tagFromTagList in tagsList)
                {
                    if (tag == tagFromTagList.Id)
                    {
                        tagsList.Remove(tagFromTagList);
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
