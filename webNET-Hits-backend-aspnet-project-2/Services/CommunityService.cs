using webNET_Hits_backend_aspnet_project_2.Data;
using webNET_Hits_backend_aspnet_project_2.Models.DbModels;
using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;
using backtewebNET_Hits_backend_aspnet_project_2stDevelop.Models.AnotherModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using webNET_Hits_backend_aspnet_project_2.Models.AnotherModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace webNET_Hits_backend_aspnet_project_2.Services
{
    public class CommunityService
    {
        private readonly AppDbContext _dbContext;

        public CommunityService(AppDbContext context)
        {
            _dbContext = context;
        }

        public int GetSubscribersCount(Guid communityId)
        {
            List<CommunityMember> members = _dbContext.CommunityMembers.Where(item => item.CommunityId == communityId && item.Role == CommunityRole.Subscriber).ToList();
            return members.Count;
        }

        public bool CheckCommunityAvalibity(Guid communityId, Guid userId)
        {
            bool isCommunityOpen = _dbContext.Communities
                .Any(c => c.Id == communityId && !c.IsClosed);

            if (isCommunityOpen) { return true; }

            bool isUserMember = _dbContext.CommunityMembers
                .Any(member => member.CommunityId == communityId && member.UserId == userId);

            return isUserMember;
        }

        public List<CommuntiesDTO> GetCommunities()
        {
            List<CommunityModel> communities = _dbContext.Communities.ToList();
            List<CommuntiesDTO> result = new List<CommuntiesDTO>();

            foreach (var community in communities)
            {
                CommuntiesDTO communiesDTO = new CommuntiesDTO
                {
                    Name = community.Name,
                    Description = community.Description,
                    IsClosed = community.IsClosed,
                    SubscribersCount = GetSubscribersCount(community.Id),
                    Id = community.Id,
                    CreateTime = community.CreateTime,
                };

                result.Add(communiesDTO);
            }

            return result;
        }

        public List<CommunityMemberDTO> GetMembershipsUser(Guid userId)
        {
            List<CommunityMember> MembershipsUser = _dbContext.CommunityMembers.Where(c => c.UserId == userId).ToList();

            List<CommunityMemberDTO> answer = MembershipsUser
                .Select(MembershipsUser => new CommunityMemberDTO
                {
                    CommunityId = MembershipsUser.CommunityId,
                    UserId = userId,
                    Role = MembershipsUser.Role
                })
                .ToList();

            return answer;
        }

        public CommunityDTO GetCommunityDTO(Guid communityId)
        {
            CommunityModel communityModel = _dbContext.Communities.FirstOrDefault(c => c.Id == communityId);

            int membersCount = _dbContext.CommunityMembers.Count(c => c.UserId == communityId && c.Role == CommunityRole.Subscriber);

            List<Guid> AdministratorsId = _dbContext.CommunityMembers.
                Where(c => c.Id == communityId && c.Role == CommunityRole.Administrator).
                Select(c => c.UserId).
                ToList();
            List<UserData> Administrators = _dbContext.Users.Where(user => AdministratorsId.Contains(user.Id)).ToList();

            CommunityDTO answer = new CommunityDTO
            {
                Administrators = Administrators,
                Name = communityModel.Name,
                Description = communityModel.Description,
                IsClosed = communityModel.IsClosed,
                SubscribersCount = membersCount,
                Id = communityId,
                CreateTime = communityModel.CreateTime
            };
            return answer;
        }

        public (List<PostsDTO>, PaginationDTO) GetListOfAvalibleCommunityPosts(FilterOptionsCommunity filterOptions, Guid userId)
        {
            var query = _dbContext.Posts.AsQueryable();

            // Фильтрация по тэгам
            query = query.Where(c => c.CommunityId == filterOptions.CommunityId);

            query = query
                .Where(post =>
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

        public CommunityRole? GetTheGreatestRole(Guid communityId, Guid userId)
        {
            var member = _dbContext.CommunityMembers.Where(c => c.UserId == userId && c.CommunityId == communityId).ToList();

            bool isAdmin = member.Exists(m => m.Role == CommunityRole.Administrator);
            bool isSubscriber = member.Exists(m => m.Role == CommunityRole.Subscriber);

            if (isAdmin) return CommunityRole.Administrator;
            if (isSubscriber) return CommunityRole.Subscriber;
            return null;
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

        public bool CheckCommunityExesists(Guid communityId)
        {
            var answer = _dbContext.Communities.FirstOrDefault(item => item.Id == communityId);

            if (answer != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckOpenityOfCommunity(Guid? communityId)
        {
            return _dbContext.Communities.FirstOrDefault(item => item.Id == communityId).IsClosed;
        }

        public bool CheckMembershipInCommunity(Guid? userId, Guid? communityId)
        {
            var answer = _dbContext.CommunityMembers.FirstOrDefault(item => item.UserId == userId && item.CommunityId == communityId);

            if (answer == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SubscribeToCommunityAsUser(Guid userId, Guid communityId)
        {
            CommunityMember newMember = new CommunityMember
            {
                Id = new Guid(),
                UserId = userId,
                CommunityId = communityId,
                Role = CommunityRole.Subscriber
            };

            _dbContext.CommunityMembers.Add(newMember);
            _dbContext.SaveChanges();
        }

        public void UnsubscribeFromCommunityAsUser(Guid userId, Guid communityId)
        {
            CommunityMember currentMember = _dbContext.CommunityMembers.FirstOrDefault(i => i.UserId == userId && i.CommunityId == communityId);

            _dbContext.CommunityMembers.Remove(currentMember);
            _dbContext.SaveChanges();
        }
    }
}
