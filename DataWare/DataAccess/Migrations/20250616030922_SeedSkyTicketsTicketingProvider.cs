using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedSkyTicketsTicketingProvider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ticketing_provider",
                columns: new[] { "id", "code", "name" },
                values: new object[] { 2, "SKYTICKETS", "SKY Tickets" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ticketing_provider",
                keyColumn: "id",
                keyValue: 2);
        }
    }
}
