using webNET_Hits_backend_aspnet_project_2.Data;
using webNET_Hits_backend_aspnet_project_2.Models.DbModels;
using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using webNET_Hits_backend_aspnet_project_2.Models.AnotherModels;

namespace webNET_Hits_backend_aspnet_project_2.Services
{
    public class CommunityService
    {
        private readonly AppDbContext _dbContext;

        public CommunityService(AppDbContext context)
        {
            _dbContext = context;
        }

        public bool CheckAdmin(Guid userId, Guid communityId)
        {
            var answer = _dbContext.CommunityMembers.
                FirstOrDefault(item => item.UserId == userId && item.CommunityId == communityId && item.Role == CommunityRole.Administrator);

            if (answer != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public PostData? CreateCommunityPost(CreatePostModel post, Guid authorId, Guid CommunityId)
        {
            Guid postId = Guid.NewGuid();

            PostData fullPost = new PostData
            {
                Id = postId,
                CreateTime = DateTime.UtcNow,
                Title = post.Title,
                Description = post.Description,
                ReadingTime = post.ReadingTime,
                Image = post.Image,
                AuthorId = authorId,
                CommunityId = CommunityId,
                AddressId = post.AddressId
            };

            if (post.Tags != null && post.Tags.Count > 0)
            {
                foreach (var tag in post.Tags)
                {
                    TagPostJoin tagPostJoin = new TagPostJoin
                    {
                        Id = new Guid(),
                        PostId = postId,
                        TagId = tag
                    };
                    _dbContext.TagsPostsJoinTable.Add(tagPostJoin);
                }
            }


            _dbContext.Posts.Add(fullPost);
            _dbContext.SaveChanges();

            return fullPost;
        }
    }
}
