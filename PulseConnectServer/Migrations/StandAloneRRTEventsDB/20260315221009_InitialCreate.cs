using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseConnectServer.Migrations.StandAloneRRTEventsDB
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ListOfStandaloneEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoscAchieved = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RRTEventType = table.Column<int>(type: "int", nullable: false),
                    TimeToRosc = table.Column<int>(type: "int", nullable: false),
                    PatientLocationBedNumber = table.Column<string>(type: "longtext", nullable: false),
                    PatientLocationWardName = table.Column<string>(type: "longtext", nullable: false),
                    EventTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EventData = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EventGuid = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PatientGuid = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PatientCurrentLocationGuid = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EventSeverity = table.Column<int>(type: "int", nullable: false),
                    EventClass = table.Column<int>(type: "int", nullable: false),
                    EventCallerGuid = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListOfStandaloneEvents", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListOfStandaloneEvents");
        }
    }
}
