using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;

namespace webNET_Hits_backend_aspnet_project_2.Models.AnotherModels
{
    public class FilterOptionsCommunity
    {
        public Guid CommunityId { get; set; }
        public List<Guid>? Tags { get; set; }

        public PostSorting? Sorting { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 5;
    }
}
