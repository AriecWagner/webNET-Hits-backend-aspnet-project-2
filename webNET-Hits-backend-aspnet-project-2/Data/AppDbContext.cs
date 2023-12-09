using webNET_Hits_backend_aspnet_project_2.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Azure;

namespace webNET_Hits_backend_aspnet_project_2.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserData> Users { get; set; }
        public DbSet<PasswordModel> UserPasswords { get; set; }
        public DbSet<TokenModel> Tokens { get; set; }
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureTagsTable(modelBuilder);
            ConfigureCommunitiesTable(modelBuilder);
        }

        private void ConfigureTagsTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.DbModels.Tag>().HasData(
                new Models.DbModels.Tag
                {
                    Id = new Guid("7dd0e51a-606f-4dea-e6e3-08dbea521a91"),
                    CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc),
                    Name = "история"
                },
                new Models.DbModels.Tag
                {
                    Id = new Guid("d774dd11-2600-46ab-e6e4-08dbea521a91"),
                    CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 839, DateTimeKind.Utc),
                    Name = "еда"
                },
                new Models.DbModels.Tag
                {
                    Id = new Guid("341aa6d9-cf7b-4a99-e6e5-08dbea521a91"),
                    CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 843, DateTimeKind.Utc),
                    Name = "18+"
                },
                new Models.DbModels.Tag
                {
                    Id = new Guid("553f5361-428a-4122-e6e6-08dbea521a91"),
                    CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 853, DateTimeKind.Utc),
                    Name = "приколы"
                },
                new Models.DbModels.Tag
                {
                    Id = new Guid("86acb301-05ff-4822-e6e7-08dbea521a91"),
                    CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 863, DateTimeKind.Utc),
                    Name = "it"
                },
                new Models.DbModels.Tag
                {
                    Id = new Guid("e587312f-4df7-4879-e6e8-08dbea521a91"),
                    CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 873, DateTimeKind.Utc),
                    Name = "интернет"
                },
                new Models.DbModels.Tag
                {
                    Id = new Guid("47aa0a33-2b86-4039-e6e9-08dbea521a91"),
                    CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 883, DateTimeKind.Utc),
                    Name = "теория_заговора"
                },
                new Models.DbModels.Tag
                {
                    Id = new Guid("e463050a-d659-433d-e6ea-08dbea521a91"),
                    CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 892, DateTimeKind.Utc),
                    Name = "соцсети"
                },
                new Models.DbModels.Tag
                {
                    Id = new Guid("0c64569f-5675-484b-e6eb-08dbea521a91"),
                    CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 802, DateTimeKind.Utc),
                    Name = "косплей"
                },
                new Models.DbModels.Tag
                {
                    Id = new Guid("fb1f2ce1-6943-420f-e6ec-08dbea521a91"),
                    CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 812, DateTimeKind.Utc),
                    Name = "преступление"
                }
            );
        }

        private void ConfigureCommunitiesTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.DbModels.CommunityModel>().HasData(
                new Models.DbModels.CommunityModel
                {
                    Name = "Масонская ложа",
                    Description = "Место, помещение, где собираются масоны для проведения своих собраний, чаще называемых работами",
                    IsClosed = true,
                    Id = new Guid("21db62c6-a964-45c1-17e0-08dbea521a96"),
                    CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc)
                },
                new Models.DbModels.CommunityModel
                {
                    Name = "Следствие вели с Л. Каневским",
                    Description = "Без длинных предисловий: мужчина умер",
                    IsClosed = false,
                    Id = new Guid("c5639aab-3a25-4efc-17e1-08dbea521a96"),
                    CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc)
                },
                new Models.DbModels.CommunityModel
                {
                    Name = "IT <3",
                    Description = "Информационные технологии связаны с изучением методов и средств сбора, обработки и передачи данных с целью получения информации нового качества о состоянии объекта, процесса или явления",
                    IsClosed = false,
                    Id = new Guid("b9851a35-b836-47f8-17e2-08dbea521a96"),
                    CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc)
                }
            );
        }
    }

}
