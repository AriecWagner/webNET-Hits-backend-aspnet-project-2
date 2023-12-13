using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace webNET_Hits_backend_aspnet_project_2.Migrations
{
    /// <inheritdoc />
    public partial class Addresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "fias");

            migrationBuilder.CreateTable(
                name: "as_addr_obj",
                schema: "fias",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    objectid = table.Column<long>(type: "bigint", nullable: false),
                    objectguid = table.Column<Guid>(type: "uuid", nullable: false),
                    changeid = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    typename = table.Column<string>(type: "text", nullable: false),
                    level = table.Column<string>(type: "text", nullable: false),
                    opertypeid = table.Column<int>(type: "integer", nullable: false),
                    previd = table.Column<long>(type: "bigint", nullable: false),
                    nextid = table.Column<long>(type: "bigint", nullable: false),
                    updatedate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    startdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    enddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isactual = table.Column<int>(type: "integer", nullable: false),
                    isactive = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_as_addr_obj", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "as_adm_hierarchy",
                schema: "fias",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    objectid = table.Column<long>(type: "bigint", nullable: false),
                    parentobjid = table.Column<long>(type: "bigint", nullable: false),
                    changeid = table.Column<long>(type: "bigint", nullable: false),
                    regioncode = table.Column<string>(type: "text", nullable: false),
                    areacode = table.Column<string>(type: "text", nullable: true),
                    citycode = table.Column<string>(type: "text", nullable: true),
                    placecode = table.Column<string>(type: "text", nullable: true),
                    plancode = table.Column<string>(type: "text", nullable: true),
                    streetcode = table.Column<string>(type: "text", nullable: true),
                    previd = table.Column<long>(type: "bigint", nullable: false),
                    nextid = table.Column<long>(type: "bigint", nullable: false),
                    updatedate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    startdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    enddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isactive = table.Column<int>(type: "integer", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_as_adm_hierarchy", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "as_houses",
                schema: "fias",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    objectid = table.Column<long>(type: "bigint", nullable: false),
                    objectguid = table.Column<Guid>(type: "uuid", nullable: false),
                    changeid = table.Column<long>(type: "bigint", nullable: false),
                    housenum = table.Column<string>(type: "text", nullable: false),
                    addnum1 = table.Column<string>(type: "text", nullable: true),
                    addnum2 = table.Column<string>(type: "text", nullable: true),
                    housetype = table.Column<int>(type: "integer", nullable: false),
                    addtype1 = table.Column<int>(type: "integer", nullable: true),
                    addtype2 = table.Column<int>(type: "integer", nullable: true),
                    opertypeid = table.Column<int>(type: "integer", nullable: false),
                    previd = table.Column<long>(type: "bigint", nullable: false),
                    nextid = table.Column<long>(type: "bigint", nullable: false),
                    updatedate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    startdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    enddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isactual = table.Column<int>(type: "integer", nullable: false),
                    isactive = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_as_houses", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "as_addr_obj",
                schema: "fias");

            migrationBuilder.DropTable(
                name: "as_adm_hierarchy",
                schema: "fias");

            migrationBuilder.DropTable(
                name: "as_houses",
                schema: "fias");
        }
    }
}
