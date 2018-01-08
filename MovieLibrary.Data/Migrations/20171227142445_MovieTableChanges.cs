using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MovieLibrary.Data.Migrations
{
    public partial class MovieTableChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieSongWriters");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Movies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Movies",
                maxLength: 10000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MovieSongWriters",
                columns: table => new
                {
                    MovieId = table.Column<int>(nullable: false),
                    SongWriterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieSongWriters", x => new { x.MovieId, x.SongWriterId });
                    table.ForeignKey(
                        name: "FK_MovieSongWriters_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieSongWriters_Persons_SongWriterId",
                        column: x => x.SongWriterId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieSongWriters_SongWriterId",
                table: "MovieSongWriters",
                column: "SongWriterId");
        }
    }
}
