using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;

namespace webNET_Hits_backend_aspnet_project_2.Models.AnotherModels
{
    public class FilterOptions
    {
        public List<Guid>? Tags { get; set; }
        public string? PathOfAuthor { get; set; }
        public int? minRead { get; set; }
        public int? maxRead { get; set;}
        public PostSorting? Sorting { get; set; }
        public bool? OnlyMyCommunities { get; set; } = false;
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 5;
    }
}
