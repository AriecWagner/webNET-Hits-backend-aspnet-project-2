using webNET_Hits_backend_aspnet_project_2.Data;
using webNET_Hits_backend_aspnet_project_2.Models.DbModels;
using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using webNET_Hits_backend_aspnet_project_2.Models.AnotherModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using webNET_Hits_backend_aspnet_project_2.Migrations;

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
        public bool CheckOpenityOfCommunity(Guid communityId)
        {
            return _dbContext.Communities.FirstOrDefault(item => item.Id == communityId).IsClosed;
        }

        public bool CheckMembershipInCommunity(Guid userId, Guid communityId)
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
