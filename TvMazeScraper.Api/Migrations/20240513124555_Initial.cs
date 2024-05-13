using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvMazeScraper.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CastMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CastMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TvShowCastMembers",
                columns: table => new
                {
                    TvShowId = table.Column<int>(type: "int", nullable: false),
                    CastMemberId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvShowCastMembers", x => new { x.TvShowId, x.CastMemberId });
                });

            migrationBuilder.CreateTable(
                name: "TvShows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvShows", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CastMembers");

            migrationBuilder.DropTable(
                name: "TvShowCastMembers");

            migrationBuilder.DropTable(
                name: "TvShows");
        }
    }
}
