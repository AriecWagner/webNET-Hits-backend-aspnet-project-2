using webNET_Hits_backend_aspnet_project_2.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Azure;

namespace webNET_Hits_backend_aspnet_project_2.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserData> Users { get; set; }
        public DbSet<PasswordModel> UserPasswords { get; set; }
        public DbSet<PostData> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagPostJoin> TagsPostsJoinTable { get; set; }
        public DbSet<LikeModel> Likes { get; set; }
        public DbSet<CommunityModel> Communities { get; set; }
        public DbSet<CommunityMember> CommunityMembers { get; set; }
        public DbSet<CommentModel> Comments { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }

}
