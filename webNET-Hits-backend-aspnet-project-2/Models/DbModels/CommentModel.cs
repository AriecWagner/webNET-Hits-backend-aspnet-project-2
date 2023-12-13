using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_2.Models.DbModels
{
    public class CommentModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid PostId { get; set; }
        public Guid? ParentId { get; set; }
        public Guid AuthorId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
