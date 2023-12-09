using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_2.Models.AnotherModels
{
    public class CommunityMemberDTO
    {
        public Guid UserId { get; set; }
        public Guid CommunityId { get; set; }
        [Column("CommunityRole", TypeName = "text")]
        [EnumDataType(typeof(CommunityRole))]
        public CommunityRole Role { get; set; }
    }
}
