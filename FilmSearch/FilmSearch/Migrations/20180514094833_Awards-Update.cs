using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FilmSearch.Migrations
{
    public partial class AwardsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FilmAwards",
                table: "FilmAwards");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PersonRoles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FilmAwards");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Awards",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_FilmAwards",
                table: "FilmAwards",
                columns: new[] { "FilmId", "AwardId" });

            migrationBuilder.CreateIndex(
                name: "IX_FilmAwards_AwardId",
                table: "FilmAwards",
                column: "AwardId");

            migrationBuilder.AddForeignKey(
                name: "FK_FilmAwards_Awards_AwardId",
                table: "FilmAwards",
                column: "AwardId",
                principalTable: "Awards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FilmAwards_Films_FilmId",
                table: "FilmAwards",
                column: "FilmId",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilmAwards_Awards_AwardId",
                table: "FilmAwards");

            migrationBuilder.DropForeignKey(
                name: "FK_FilmAwards_Films_FilmId",
                table: "FilmAwards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FilmAwards",
                table: "FilmAwards");

            migrationBuilder.DropIndex(
                name: "IX_FilmAwards_AwardId",
                table: "FilmAwards");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PersonRoles",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "FilmAwards",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Awards",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FilmAwards",
                table: "FilmAwards",
                column: "Id");
        }
    }
}
