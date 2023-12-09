using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace webNET_Hits_backend_aspnet_project_2.Models.DbModels
{
    public class CommunityMember
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CommunityId { get; set; }
        [Column("CommunityRole", TypeName = "text")]
        [EnumDataType(typeof(CommunityRole))]
        public CommunityRole Role { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
