using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_2.Models.DbModels
{
    public class CommunityModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsClosed { get; set; }
        [Key]
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
