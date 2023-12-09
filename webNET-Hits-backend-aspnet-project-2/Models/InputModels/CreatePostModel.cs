using webNET_Hits_backend_aspnet_project_2.Models.DbModels;

namespace webNET_Hits_backend_aspnet_project_2.Models.AnotherModels
{
    public class CreatePostModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int ReadingTime { get; set; }
        public string? Image { get; set; }
        public Guid? AddressId { get; set; }
        public List<Guid>? Tags { get; set; }
    }
}
