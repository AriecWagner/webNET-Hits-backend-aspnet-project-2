using webNET_Hits_backend_aspnet_project_2.Data;
using webNET_Hits_backend_aspnet_project_2.Models.DbModels;
using Microsoft.Extensions.Hosting;
using webNET_Hits_backend_aspnet_project_2.Models.AnotherModels;

namespace webNET_Hits_backend_aspnet_project_2.Services
{
    public class CommentService
    {
        private readonly AppDbContext _dbContext;

        public CommentService(AppDbContext context)
        {
            _dbContext = context;
        }

        public CommentModel? GetCommentById(Guid? id)
        {
            return _dbContext.Comments.FirstOrDefault(item => item.Id == id);
        }

        public bool checkParentIsCorrect(Guid? id)
        {
            if (id == Guid.Empty) { return true; }

            var answer = GetCommentById(id);

            if (answer != null) { return true; }
            else { return false; }
        }

        public void AddCommentToPost(CommentInput commentInput, Guid postId, Guid userId)
        {
            CommentModel comment = new CommentModel
            {
                Id = new Guid(),
                Content = commentInput.Content,
                PostId = postId,
                ParentId = commentInput.ParentId,
                AuthorId = userId,
                CreateTime = DateTime.UtcNow,
                ModifiedDate = null,
                DeleteDate = null
            };

            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();
        }
    }
}
