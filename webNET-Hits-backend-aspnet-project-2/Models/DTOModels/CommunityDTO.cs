using webNET_Hits_backend_aspnet_project_2.Models.DbModels;
using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_2.Models.AnotherModels
{
    public class CommunityDTO
    {
        public List<UserData> Administrators { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsClosed { get; set; }
        public int SubscribersCount { get; set; }
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
