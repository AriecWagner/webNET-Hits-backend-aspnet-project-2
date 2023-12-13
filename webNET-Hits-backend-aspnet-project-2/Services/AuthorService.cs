using webNET_Hits_backend_aspnet_project_2.Data;
using webNET_Hits_backend_aspnet_project_2.Models.AnotherModels;
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

        public List<AuthorDTO> GetAuthorDTOs()
        {
            List<AuthorDTO> authors = new List<AuthorDTO>();

            List<UserData> users = _dbContext.Users.ToList();

            var userLikesCounts = _dbContext.Likes
                .Join(
                    _dbContext.Posts,
                    like => like.PostId,
                    post => post.Id,
                    (like, post) => new { UserId = post.AuthorId, PostId = post.Id }
                )
                .GroupBy(result => result.UserId)
                .ToDictionary(group => group.Key, group => group.Count());

            var userPostsCounts = _dbContext.Posts
                .GroupBy(post => post.AuthorId)
                .ToDictionary(group => group.Key, group => group.Count());

            foreach (var user in users)
            {
                int postsCount = userPostsCounts.ContainsKey(user.Id) ? userPostsCounts[user.Id] : 0;

                int likesCount = userLikesCounts.ContainsKey(user.Id) ? userLikesCounts[user.Id] : 0;

                AuthorDTO author = new AuthorDTO
                {
                    FullName = user.FullName,
                    BirthDate = user.BirthDate,
                    Gender = user.Gender,
                    Posts = postsCount,
                    Likes = likesCount,
                    Created = user.CreateTime
                };

                authors.Add(author);
            }

            return authors;
        }
    }
}
