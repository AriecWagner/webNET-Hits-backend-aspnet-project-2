namespace webNET_Hits_backend_aspnet_project_2.Models.AnotherModels
{
    public class CommentsDTO
    {
        public string Content { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Guid AuthorId { get; set; }
        public string Author { get; set; }
        public int SubComments { get; set; }
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
