using System.Text.Json.Serialization;

namespace webNET_Hits_backend_aspnet_project_2.Models.EnumModels
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PostSorting
    {
        CreateDesc,
        CreateAsc,
        LikeAsc,
        LikeDesc
    }
}
