using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Migrations {
    /// <inheritdoc />
    public partial class AddDueDateUnits : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                name: "TargetUnits",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pandaid = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TargetName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    BelongedTo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_TargetUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DueDateUnits",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TargetUnitId = table.Column<int>(type: "int", nullable: false),
                    CategoryCode = table.Column<short>(type: "smallint", nullable: false),
                    ReplacedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_DueDateUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DueDateUnits_DueDateUnits_ReplacedById",
                        column: x => x.ReplacedById,
                        principalTable: "DueDateUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DueDateUnits_TargetUnits_TargetUnitId",
                        column: x => x.TargetUnitId,
                        principalTable: "TargetUnits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DueDateUnits_ReplacedById",
                table: "DueDateUnits",
                column: "ReplacedById",
                unique: true,
                filter: "[ReplacedById] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DueDateUnits_TargetUnitId",
                table: "DueDateUnits",
                column: "TargetUnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "DueDateUnits");

            migrationBuilder.DropTable(
                name: "TargetUnits");
        }
    }
}
