using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenAppInsights.Data.Migrations
{
    /// <inheritdoc />
    public partial class inti : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Metrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Endpoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DurationMs = table.Column<double>(type: "float", nullable: false),
                    MemoryBytes = table.Column<long>(type: "bigint", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnergyEstimates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MetricId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnergyMilliJoules = table.Column<double>(type: "float", nullable: false),
                    EstimatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyEstimates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnergyEstimates_Metrics_MetricId",
                        column: x => x.MetricId,
                        principalTable: "Metrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OptimizationHints",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MetricId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HintText = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptimizationHints", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_OptimizationHints_Metrics_MetricId",
                        column: x => x.MetricId,
                        principalTable: "Metrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnergyEstimates_MetricId",
                table: "EnergyEstimates",
                column: "MetricId");

            migrationBuilder.CreateIndex(
                name: "IX_OptimizationHints_MetricId",
                table: "OptimizationHints",
                column: "MetricId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnergyEstimates");

            migrationBuilder.DropTable(
                name: "OptimizationHints");

            migrationBuilder.DropTable(
                name: "Metrics");
        }
    }
}
