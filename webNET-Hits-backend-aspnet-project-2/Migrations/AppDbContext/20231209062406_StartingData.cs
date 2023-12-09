using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace webNET_Hits_backend_aspnet_project_2.Migrations
{
    /// <inheritdoc />
    public partial class StartingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Communities",
                columns: new[] { "Id", "CreateTime", "Description", "IsClosed", "Name" },
                values: new object[,]
                {
                    { new Guid("21db62c6-a964-45c1-17e0-08dbea521a96"), new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc), "Место, помещение, где собираются масоны для проведения своих собраний, чаще называемых работами", true, "Масонская ложа" },
                    { new Guid("b9851a35-b836-47f8-17e2-08dbea521a96"), new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc), "Информационные технологии связаны с изучением методов и средств сбора, обработки и передачи данных с целью получения информации нового качества о состоянии объекта, процесса или явления", false, "IT <3" },
                    { new Guid("c5639aab-3a25-4efc-17e1-08dbea521a96"), new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc), "Без длинных предисловий: мужчина умер", false, "Следствие вели с Л. Каневским" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreateTime", "Name" },
                values: new object[,]
                {
                    { new Guid("0c64569f-5675-484b-e6eb-08dbea521a91"), new DateTime(2023, 11, 21, 12, 24, 15, 802, DateTimeKind.Utc), "косплей" },
                    { new Guid("341aa6d9-cf7b-4a99-e6e5-08dbea521a91"), new DateTime(2023, 11, 21, 12, 24, 15, 843, DateTimeKind.Utc), "18+" },
                    { new Guid("47aa0a33-2b86-4039-e6e9-08dbea521a91"), new DateTime(2023, 11, 21, 12, 24, 15, 883, DateTimeKind.Utc), "теория_заговора" },
                    { new Guid("553f5361-428a-4122-e6e6-08dbea521a91"), new DateTime(2023, 11, 21, 12, 24, 15, 853, DateTimeKind.Utc), "приколы" },
                    { new Guid("7dd0e51a-606f-4dea-e6e3-08dbea521a91"), new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc), "история" },
                    { new Guid("86acb301-05ff-4822-e6e7-08dbea521a91"), new DateTime(2023, 11, 21, 12, 24, 15, 863, DateTimeKind.Utc), "it" },
                    { new Guid("d774dd11-2600-46ab-e6e4-08dbea521a91"), new DateTime(2023, 11, 21, 12, 24, 15, 839, DateTimeKind.Utc), "еда" },
                    { new Guid("e463050a-d659-433d-e6ea-08dbea521a91"), new DateTime(2023, 11, 21, 12, 24, 15, 892, DateTimeKind.Utc), "соцсети" },
                    { new Guid("e587312f-4df7-4879-e6e8-08dbea521a91"), new DateTime(2023, 11, 21, 12, 24, 15, 873, DateTimeKind.Utc), "интернет" },
                    { new Guid("fb1f2ce1-6943-420f-e6ec-08dbea521a91"), new DateTime(2023, 11, 21, 12, 24, 15, 812, DateTimeKind.Utc), "преступление" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Communities",
                keyColumn: "Id",
                keyValue: new Guid("21db62c6-a964-45c1-17e0-08dbea521a96"));

            migrationBuilder.DeleteData(
                table: "Communities",
                keyColumn: "Id",
                keyValue: new Guid("b9851a35-b836-47f8-17e2-08dbea521a96"));

            migrationBuilder.DeleteData(
                table: "Communities",
                keyColumn: "Id",
                keyValue: new Guid("c5639aab-3a25-4efc-17e1-08dbea521a96"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("0c64569f-5675-484b-e6eb-08dbea521a91"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("341aa6d9-cf7b-4a99-e6e5-08dbea521a91"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("47aa0a33-2b86-4039-e6e9-08dbea521a91"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("553f5361-428a-4122-e6e6-08dbea521a91"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("7dd0e51a-606f-4dea-e6e3-08dbea521a91"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("86acb301-05ff-4822-e6e7-08dbea521a91"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("d774dd11-2600-46ab-e6e4-08dbea521a91"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("e463050a-d659-433d-e6ea-08dbea521a91"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("e587312f-4df7-4879-e6e8-08dbea521a91"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("fb1f2ce1-6943-420f-e6ec-08dbea521a91"));
        }
    }
}
