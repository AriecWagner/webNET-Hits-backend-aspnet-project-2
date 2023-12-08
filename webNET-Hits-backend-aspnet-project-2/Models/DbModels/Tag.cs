using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_2.Models.DbModels
{
    public class Tag
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string Name { get; set; }
    }
}
