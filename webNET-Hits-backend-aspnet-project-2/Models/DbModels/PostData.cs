using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_2.Models.DbModels
{
    public class PostData
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ReadingTime { get; set; }
        public string? Image { get; set; }
        public Guid AuthorId { get; set; }
        public Guid? CommunityId { get; set; }
        public Guid? AddressId { get; set; }
    }
}
