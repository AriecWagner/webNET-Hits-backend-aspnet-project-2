using webNET_Hits_backend_aspnet_project_2.Data;
using webNET_Hits_backend_aspnet_project_2.Models.DbModels;
using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;
using Microsoft.Extensions.Hosting;
using static System.Net.Mime.MediaTypeNames;
using webNET_Hits_backend_aspnet_project_2.Models.AnotherModels;

namespace webNET_Hits_backend_aspnet_project_2.Services
{
    public class PostService
    {
        private readonly AppDbContext _dbContext;

        public PostService(AppDbContext context)
        {
            _dbContext = context;
        }
        public PostData? CreatePersonalUserPost(CreatePostModel post, Guid authorId)
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
                CommunityId = null,
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

        public void AddLikeToPost(Guid userId, Guid postId)
        {
            LikeModel newLike = new LikeModel
            {
                Id = new Guid(),
                UserId = userId,
                PostId = postId
            };

            _dbContext.Likes.Add(newLike);
            _dbContext.SaveChanges();
        }

        public void DeleteLikeFromPost(Guid userId, Guid postId)
        {
            LikeModel currentLike = _dbContext.Likes.FirstOrDefault(i => i.UserId == userId && i.PostId == postId);

            _dbContext.Likes.Remove(currentLike);
            _dbContext.SaveChanges();
        }
    }
}
