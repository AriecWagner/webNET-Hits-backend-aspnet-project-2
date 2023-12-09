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

        public (List<PostsDTO>, PaginationDTO) GetListOfAvaliblePosts(FilterOptions filterOptions, Guid? userId)
        {
            var query = _dbContext.Posts.AsQueryable();

            // Фильтрация по тэгам

            query = query
                .Where(post =>
                    // Пост принадлежит сообществу, и либо сообщество открытое, либо пользователь в нем член
                    (post.CommunityId == null) ||
                    (_dbContext.Communities.Any(c => c.Id == post.CommunityId &&
                    (!c.IsClosed ||
                    (c.IsClosed && _dbContext.CommunityMembers.Any(cm => cm.CommunityId == c.Id && cm.UserId == userId)))))
                );

            if (filterOptions.Tags != null && filterOptions.Tags.Any())
            {
                query = query
                    .Join(
                        _dbContext.TagsPostsJoinTable,
                        post => post.Id,
                        tagJoin => tagJoin.PostId,
                        (post, tagJoin) => new { Post = post, TagId = tagJoin.TagId }
                    )
                    .Where(result => filterOptions.Tags.Contains(result.TagId))
                    .Select(result => result.Post);
            }

            /*if (filterOptions.Tags != null && filterOptions.Tags.Any())
            {
                query = query.Where(post =>
                    _dbContext.TagsPostsJoinTable.Any(tagJoin =>
                        tagJoin.PostId == post.Id && filterOptions.Tags.Contains(tagJoin.TagId)));
            }*/

            // Фильтрация по имени автора

            if (!string.IsNullOrEmpty(filterOptions.PathOfAuthor))
            {
                query = query
                    .Join(
                        _dbContext.Users,
                        post => post.AuthorId,
                        user => user.Id,
                        (post, user) => new { Post = post, AuthorName = user.FullName }
                    )
                    .Where(result => result.AuthorName.Contains(filterOptions.PathOfAuthor))
                    .Select(result => result.Post);

            }
            /*
            if (!string.IsNullOrEmpty(filterOptions.PathOfAuthor))
            {
                query = query.Where(post =>
                    _dbContext.Users.Any(user => user.Id == post.AuthorId && user.FullName.Contains(filterOptions.PathOfAuthor)));
            }*/

            // Фильтрация по времени чтения
            if (filterOptions.minRead.HasValue)
            {
                query = query.Where(post => post.ReadingTime >= filterOptions.minRead.Value);
            }

            if (filterOptions.maxRead.HasValue)
            {
                query = query.Where(post => post.ReadingTime <= filterOptions.maxRead.Value);
            }

            // Фильтрация по группам
            if (filterOptions.OnlyMyCommunities != null && userId != null && filterOptions.OnlyMyCommunities == true)
            {
                query = query
                    .Where(post => _dbContext.CommunityMembers
                        .Any(member => member.UserId == userId && member.CommunityId == post.CommunityId));
            }

            //сортировка по критерию
            switch (filterOptions.Sorting)
            {
                case PostSorting.CreateAsc:
                    query = query.OrderBy(c => c.Title);
                    break;
                case PostSorting.CreateDesc:
                    query = query.OrderByDescending(c => c.Title);
                    break;
                case PostSorting.LikeAsc:
                    query = query
                        .GroupJoin(
                            _dbContext.Likes,
                            post => post.Id,
                            like => like.PostId,
                            (post, likes) => new
                            {
                                Post = post,
                                LikesCount = likes.Count()
                            })
                        .OrderBy(result => result.LikesCount)
                        .Select(result => result.Post);
                    break;
                case PostSorting.LikeDesc:
                    query = query
                        .GroupJoin(
                            _dbContext.Likes,
                            post => post.Id,
                            like => like.PostId,
                            (post, likes) => new
                            {
                                Post = post,
                                LikesCount = likes.Count()
                            })
                        .OrderByDescending(result => result.LikesCount)
                        .Select(result => result.Post);
                    break;
            }

            int numberToSkip = (filterOptions.Page - 1) * filterOptions.Size;
            var resultPage = query.Skip(numberToSkip).Take(filterOptions.Size).ToList();

            var count = query.Count();
            count = count % filterOptions.Size == 0 ? count / filterOptions.Size : count / filterOptions.Size + 1;
            //List<PostData> posts = query.ToList();

            List<PostsDTO> posts = resultPage
                .Select(post => new PostsDTO
                {
                    Id = post.Id,
                    CreateTime = post.CreateTime,
                    Title = post.Title,
                    Description = post.Description,
                    ReadingTime = post.ReadingTime,
                    Image = post.Image,
                    AuthorId = post.AuthorId,
                    AuthorName = _dbContext.Users.FirstOrDefault(u => u.Id == post.AuthorId).FullName,
                    CommunityId = post.CommunityId,
                    CommunityName = post.CommunityId != null ? _dbContext.Communities.FirstOrDefault(c => c.Id == post.CommunityId).Name : null,
                    AddressId = post.AddressId,
                    Likes = _dbContext.Likes.Count(like => like.PostId == post.Id),
                    HasLike = _dbContext.Likes.Any(like => like.PostId == post.Id && like.UserId == userId),
                    CommentsCount = _dbContext.Comments.Count(comment => comment.PostId == post.Id),
                    Tags = _dbContext.TagsPostsJoinTable
                    .Where(tagJoin => tagJoin.PostId == post.Id)
                    .Join(_dbContext.Tags,
                          tagJoin => tagJoin.TagId,
                          tag => tag.Id,
                          (tagJoin, tag) => tag)
                    .ToList()
                })
                .ToList();

            PaginationDTO pagination = new PaginationDTO
            {
                Size = filterOptions.Size,
                Count = count,
                Current = filterOptions.Page
            };

            return (posts, pagination);
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

        public (List<CommentsDTO>, PostsDTO) GetConcretePost(Guid PostId, Guid? userId)
        {
            var post = _dbContext.Posts.FirstOrDefault(c => c.Id == PostId);

            PostsDTO postWithConcrete = new PostsDTO
            {
                Id = post.Id,
                CreateTime = post.CreateTime,
                Title = post.Title,
                Description = post.Description,
                ReadingTime = post.ReadingTime,
                Image = post.Image,
                AuthorId = post.AuthorId,
                AuthorName = _dbContext.Users.FirstOrDefault(u => u.Id == post.AuthorId).FullName,
                CommunityId = post.CommunityId,
                CommunityName = post.CommunityId != null ? _dbContext.Communities.FirstOrDefault(c => c.Id == post.CommunityId).Name : null,
                AddressId = post.AddressId,
                Likes = _dbContext.Likes.Count(like => like.PostId == post.Id),
                HasLike = _dbContext.Likes.Any(like => like.PostId == post.Id && like.UserId == userId),
                CommentsCount = _dbContext.Comments.Count(comment => comment.PostId == post.Id),
                Tags = _dbContext.TagsPostsJoinTable
                    .Where(tagJoin => tagJoin.PostId == post.Id)
                    .Join(_dbContext.Tags,
                          tagJoin => tagJoin.TagId,
                          tag => tag.Id,
                          (tagJoin, tag) => tag)
                    .ToList()
            };

            List<CommentModel> comments = _dbContext.Comments.Where(c => c.PostId == PostId).ToList();
            List<CommentsDTO> commentsDTO = comments
                .Select(comments => new CommentsDTO
                {
                    Content = comments.Content,
                    ModifiedDate = comments.ModifiedDate,
                    DeleteDate = comments.DeleteDate,
                    AuthorId = comments.AuthorId,
                    Author = _dbContext.Users.FirstOrDefault(c => c.Id == comments.AuthorId).FullName,
                    SubComments = _dbContext.Comments.Count(c => c.ParentId == comments.Id),
                    Id = comments.Id,
                    CreateTime = comments.CreateTime,
                })
                .ToList();

            return (commentsDTO, postWithConcrete);
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
