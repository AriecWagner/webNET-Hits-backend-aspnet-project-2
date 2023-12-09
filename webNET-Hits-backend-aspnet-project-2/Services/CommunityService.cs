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

        public CommunityRole? GetTheGreatestRole(Guid communityId, Guid userId)
        {
            var member = _dbContext.CommunityMembers.Where(c => c.UserId == userId && c.CommunityId == communityId).ToList();

            bool isAdmin = member.Exists(m => m.Role == CommunityRole.Administrator);
            bool isSubscriber = member.Exists(m => m.Role == CommunityRole.Subscriber);

            if (isAdmin) return CommunityRole.Administrator;
            if (isSubscriber) return CommunityRole.Subscriber;
            return null;
        }
    }
}
