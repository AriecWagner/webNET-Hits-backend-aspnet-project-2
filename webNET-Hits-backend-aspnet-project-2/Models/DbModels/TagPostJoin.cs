using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_2.Models.DbModels
{
    public class TagPostJoin
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid TagId { get; set; }
    }
}
