using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Migrations
{
    /// <inheritdoc />
    public partial class AddOriginAndDiscard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DiscardedAt",
                table: "DueDateUnits",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OriginId",
                table: "DueDateUnits",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DueDateUnits_OriginId_ReplacedById_DiscardedAt",
                table: "DueDateUnits",
                columns: new[] { "OriginId", "ReplacedById", "DiscardedAt" });

            migrationBuilder.AddForeignKey(
                name: "FK_DueDateUnits_DueDateUnits_OriginId",
                table: "DueDateUnits",
                column: "OriginId",
                principalTable: "DueDateUnits",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DueDateUnits_DueDateUnits_OriginId",
                table: "DueDateUnits");

            migrationBuilder.DropIndex(
                name: "IX_DueDateUnits_OriginId_ReplacedById_DiscardedAt",
                table: "DueDateUnits");

            migrationBuilder.DropColumn(
                name: "DiscardedAt",
                table: "DueDateUnits");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "DueDateUnits");
        }
    }
}
