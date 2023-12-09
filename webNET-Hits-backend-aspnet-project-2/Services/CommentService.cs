using webNET_Hits_backend_aspnet_project_2.Data;
using webNET_Hits_backend_aspnet_project_2.Models.DbModels;
using Microsoft.Extensions.Hosting;

namespace webNET_Hits_backend_aspnet_project_2.Services
{
    public class CommentService
    {
        private readonly AppDbContext _dbContext;

        public CommentService(AppDbContext context)
        {
            _dbContext = context;
        }

        public bool CommentExists(Guid commentId)
        {
            var answer = _dbContext.Comments.FirstOrDefault(item => item.Id == commentId);

            if (answer != null) { return true; }
            else { return false; }
        }

        public bool CommentAlreadyDeleted(Guid commentId)
        {
            var answer = _dbContext.Comments.FirstOrDefault(item => item.Id == commentId);

            if (answer.DeleteDate != null) { return true; }
            else { return false; }
        }

        public void EditComment(string content, Guid commentId)
        {
            CommentModel currentComment = _dbContext.Comments.FirstOrDefault(c => c.Id == commentId);

            currentComment.Content = content;
            currentComment.ModifiedDate = DateTime.UtcNow;

            _dbContext.Comments.Update(currentComment);
            _dbContext.SaveChanges();
        }
    }
}
