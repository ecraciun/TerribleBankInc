using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DefinitelyNotStealing.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Goodies",
                columns: table => new
                {
                    ID = table
                        .Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    ClientIP = table.Column<string>(nullable: false),
                    DataType = table.Column<string>(nullable: false),
                    Data = table.Column<string>(nullable: false),
                    CorrelationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goodies", x => x.ID);
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Goodies");
        }
    }
}
