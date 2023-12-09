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
    }
}
