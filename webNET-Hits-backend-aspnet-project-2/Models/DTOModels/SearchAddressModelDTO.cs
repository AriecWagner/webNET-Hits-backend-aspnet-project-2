using System.ComponentModel.DataAnnotations;
using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;

namespace webNET_Hits_backend_aspnet_project_2.Models.AnotherModels
{
    public class SearchAddressModelDTO
    {
        public long ObjectId { get; set; }
        public Guid ObjectGuid { get; set; }
        public string Text { get; set; }
        public string ObjectLevelText { get; set; }
        public GarAddressLevel ObjectLevel { get; set; }
    }
}
