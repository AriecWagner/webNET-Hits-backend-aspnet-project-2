using webNET_Hits_backend_aspnet_project_2.Data;
using webNET_Hits_backend_aspnet_project_2.Models.DbModels;
using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
    }
}
