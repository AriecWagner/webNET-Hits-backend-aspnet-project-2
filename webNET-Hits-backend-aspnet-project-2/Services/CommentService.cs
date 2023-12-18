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

        public List<CommentsDTO> GetAllNestedComments(Guid commentId)
        {
            List<CommentsDTO> commentsDTOs = new List<CommentsDTO>();
            List<CommentModel> children = GetChildrenId(commentId);

            foreach (CommentModel child in children)
            {
                CommentsDTO elementOfCommentsDTOs = new CommentsDTO
                {
                    Content = child.Content,
                    ModifiedDate = child.ModifiedDate,
                    DeleteDate = child.DeleteDate,
                    AuthorId = child.AuthorId,
                    Author = _dbContext.Users.FirstOrDefault(c => c.Id == child.AuthorId).FullName,
                    SubComments = _dbContext.Comments.Count(c => c.ParentId == child.Id),
                    Id = child.Id,
                    CreateTime = child.CreateTime,
                };

                commentsDTOs.Add(elementOfCommentsDTOs);
            }

            return commentsDTOs;
        }

        public List<CommentModel> GetChildrenId(Guid commentId)
        {
            return _dbContext.Comments.Where(item => item.ParentId == commentId).ToList();
        }

        public bool CommentExists(Guid commentId)
        {
            var answer = _dbContext.Comments.FirstOrDefault(item => item.Id == commentId);

            if (answer != null) { return true; }
            else { return false; }
        }

        public CommentModel? GetCommentById(Guid? id)
        {
            return _dbContext.Comments.FirstOrDefault(item => item.Id == id);
        }

        public Guid? GetCommunityIdByCommentId (Guid commentId)
        {
            Guid? postId = _dbContext.Comments.FirstOrDefault(i => i.Id == commentId)?.PostId;
            Guid? communityId = _dbContext.Posts.FirstOrDefault(i => i.Id == postId)?.CommunityId;

            return communityId;
        }

        public bool CheckOwnerOfComment(Guid commentId, Guid userId)
        {
            return _dbContext.Comments.Any(i => i.Id == commentId && i.AuthorId == userId);
        }

        public bool checkParentIsCorrect(Guid? id)
        {
            if (id == null) { return true; }

            var answer = GetCommentById(id);

            if (answer != null) { return true; }
            else { return false; }
        }


        public void AddCommentToPost(CommentInput commentInput, Guid postId, Guid userId)
        {
            commentInput.ParentId =
                commentInput.ParentId == null ?
                commentInput.ParentId = postId :
                commentInput.ParentId = commentInput.ParentId;

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

        public void DeleteComment(Guid commentId)
        {
            CommentModel currentComment = _dbContext.Comments.FirstOrDefault(c => c.Id == commentId);

            currentComment.Content = "";
            currentComment.DeleteDate = DateTime.UtcNow;

            _dbContext.Comments.Update(currentComment);
            _dbContext.SaveChanges();
        }
    }
}
