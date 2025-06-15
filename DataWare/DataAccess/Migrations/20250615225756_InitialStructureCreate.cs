using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialStructureCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "booking_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_booking_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "country",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_country", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ticketing_provider",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ticketing_provider", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "airline",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    iata_code = table.Column<string>(type: "text", nullable: false),
                    icao_code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    country_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_airline", x => x.id);
                    table.ForeignKey(
                        name: "fk_airline_country_country_id",
                        column: x => x.country_id,
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "city",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    iata_code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    country_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_city", x => x.id);
                    table.ForeignKey(
                        name: "fk_city_country_country_id",
                        column: x => x.country_id,
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "booking",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    client_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    ticketing_provider_id = table.Column<int>(type: "integer", nullable: false),
                    external_booking_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_booking", x => x.id);
                    table.ForeignKey(
                        name: "fk_booking_booking_status_status_id",
                        column: x => x.status_id,
                        principalTable: "booking_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_booking_ticketing_provider_ticketing_provider_id",
                        column: x => x.ticketing_provider_id,
                        principalTable: "ticketing_provider",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "airport",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    iata_code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    city_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_airport", x => x.id);
                    table.ForeignKey(
                        name: "fk_airport_city_city_id",
                        column: x => x.city_id,
                        principalTable: "city",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "passenger",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    firstname = table.Column<string>(type: "text", nullable: false),
                    lastname = table.Column<string>(type: "text", nullable: false),
                    middlename = table.Column<string>(type: "text", nullable: true),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    gender = table.Column<int>(type: "integer", nullable: false),
                    passport_number = table.Column<string>(type: "text", nullable: false),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    booking_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_passenger", x => x.id);
                    table.ForeignKey(
                        name: "fk_passenger_booking_booking_id",
                        column: x => x.booking_id,
                        principalTable: "booking",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_passenger_country_country_id",
                        column: x => x.country_id,
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "flight",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    booking_id = table.Column<Guid>(type: "uuid", nullable: false),
                    from_airport_id = table.Column<long>(type: "bigint", nullable: false),
                    to_airport_id = table.Column<long>(type: "bigint", nullable: false),
                    departure_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    arrival_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ticketing_provider_id = table.Column<int>(type: "integer", nullable: false),
                    total_price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_flight", x => x.id);
                    table.ForeignKey(
                        name: "fk_flight_airport_from_airport_id",
                        column: x => x.from_airport_id,
                        principalTable: "airport",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_flight_airport_to_airport_id",
                        column: x => x.to_airport_id,
                        principalTable: "airport",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_flight_booking_booking_id",
                        column: x => x.booking_id,
                        principalTable: "booking",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_flight_ticketing_provider_ticketing_provider_id",
                        column: x => x.ticketing_provider_id,
                        principalTable: "ticketing_provider",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "search_request",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    aggregation_started = table.Column<bool>(type: "boolean", nullable: false),
                    client_id = table.Column<string>(type: "text", nullable: false),
                    from_airport_id = table.Column<long>(type: "bigint", nullable: false),
                    to_airport_id = table.Column<long>(type: "bigint", nullable: false),
                    departure_date = table.Column<DateOnly>(type: "date", nullable: false),
                    passenger_count = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    search_result_key = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_search_request", x => x.id);
                    table.ForeignKey(
                        name: "fk_search_request_airport_from_airport_id",
                        column: x => x.from_airport_id,
                        principalTable: "airport",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_search_request_airport_to_airport_id",
                        column: x => x.to_airport_id,
                        principalTable: "airport",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "flight_segment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    flight_id = table.Column<Guid>(type: "uuid", nullable: false),
                    flight_number = table.Column<string>(type: "text", nullable: false),
                    airline_id = table.Column<int>(type: "integer", nullable: false),
                    from_airport_id = table.Column<long>(type: "bigint", nullable: false),
                    to_airport_id = table.Column<long>(type: "bigint", nullable: false),
                    departure_date_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    arrival_date_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_flight_segment", x => x.id);
                    table.ForeignKey(
                        name: "fk_flight_segment_airline_airline_id",
                        column: x => x.airline_id,
                        principalTable: "airline",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_flight_segment_airport_from_airport_id",
                        column: x => x.from_airport_id,
                        principalTable: "airport",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_flight_segment_airport_to_airport_id",
                        column: x => x.to_airport_id,
                        principalTable: "airport",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_flight_segment_flight_flight_id",
                        column: x => x.flight_id,
                        principalTable: "flight",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "booking_status",
                columns: new[] { "id", "code", "name" },
                values: new object[,]
                {
                    { 1, "CREATED", "Создано" },
                    { 2, "PENDING", "В обработке" },
                    { 3, "BOOKED", "Забронировано" },
                    { 4, "FAILED", "Ошибка" }
                });

            migrationBuilder.InsertData(
                table: "country",
                columns: new[] { "id", "code", "name" },
                values: new object[,]
                {
                    { 1, "FR", "Франция" },
                    { 2, "USA", "США" },
                    { 3, "UK", "Великобритания" },
                    { 4, "GEO", "Грузия" },
                    { 5, "UAE", "ОАЭ" }
                });

            migrationBuilder.InsertData(
                table: "ticketing_provider",
                columns: new[] { "id", "code", "name" },
                values: new object[] { 1, "AIRTICKETS", "AirTickets.Fly" });

            migrationBuilder.InsertData(
                table: "airline",
                columns: new[] { "id", "country_id", "iata_code", "icao_code", "name" },
                values: new object[,]
                {
                    { 1, 1, "AF", "AFR", "AirFrance" },
                    { 2, 3, "BA", "BAW", "British Airways" },
                    { 3, 5, "EK", "UAE", "Emirates" }
                });

            migrationBuilder.InsertData(
                table: "city",
                columns: new[] { "id", "country_id", "iata_code", "name" },
                values: new object[,]
                {
                    { 1L, 3, "LCY", "Лондон" },
                    { 2L, 1, "CDG", "Париж" },
                    { 3L, 4, "TBS", "Тбилиси" },
                    { 4L, 2, "LAX", "Лос Анджелес" },
                    { 5L, 5, "DBX", "Дубай" }
                });

            migrationBuilder.InsertData(
                table: "airport",
                columns: new[] { "id", "city_id", "iata_code", "name" },
                values: new object[,]
                {
                    { 1L, 2L, "CDG", "Аэропорт Шарль-Де-Голь" },
                    { 2L, 4L, "LAX", "Международный аэропорт Лос-Анджелес" },
                    { 3L, 5L, "DBX", "Международный аэропорт Дубая" },
                    { 4L, 1L, "LHR", "Лондонский аэропорт Хитроу" },
                    { 5L, 3L, "TBS", "Международный аэропорт Тбилиси имени Шота Руставели" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_airline_country_id",
                table: "airline",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_airport_city_id",
                table: "airport",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_booking_status_id",
                table: "booking",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "ix_booking_ticketing_provider_id",
                table: "booking",
                column: "ticketing_provider_id");

            migrationBuilder.CreateIndex(
                name: "ix_city_country_id",
                table: "city",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_flight_booking_id",
                table: "flight",
                column: "booking_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_flight_from_airport_id",
                table: "flight",
                column: "from_airport_id");

            migrationBuilder.CreateIndex(
                name: "ix_flight_ticketing_provider_id",
                table: "flight",
                column: "ticketing_provider_id");

            migrationBuilder.CreateIndex(
                name: "ix_flight_to_airport_id",
                table: "flight",
                column: "to_airport_id");

            migrationBuilder.CreateIndex(
                name: "ix_flight_segment_airline_id",
                table: "flight_segment",
                column: "airline_id");

            migrationBuilder.CreateIndex(
                name: "ix_flight_segment_flight_id",
                table: "flight_segment",
                column: "flight_id");

            migrationBuilder.CreateIndex(
                name: "ix_flight_segment_from_airport_id",
                table: "flight_segment",
                column: "from_airport_id");

            migrationBuilder.CreateIndex(
                name: "ix_flight_segment_to_airport_id",
                table: "flight_segment",
                column: "to_airport_id");

            migrationBuilder.CreateIndex(
                name: "ix_passenger_booking_id",
                table: "passenger",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "ix_passenger_country_id",
                table: "passenger",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_search_request_from_airport_id",
                table: "search_request",
                column: "from_airport_id");

            migrationBuilder.CreateIndex(
                name: "ix_search_request_to_airport_id",
                table: "search_request",
                column: "to_airport_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "flight_segment");

            migrationBuilder.DropTable(
                name: "passenger");

            migrationBuilder.DropTable(
                name: "search_request");

            migrationBuilder.DropTable(
                name: "airline");

            migrationBuilder.DropTable(
                name: "flight");

            migrationBuilder.DropTable(
                name: "airport");

            migrationBuilder.DropTable(
                name: "booking");

            migrationBuilder.DropTable(
                name: "city");

            migrationBuilder.DropTable(
                name: "booking_status");

            migrationBuilder.DropTable(
                name: "ticketing_provider");

            migrationBuilder.DropTable(
                name: "country");
        }
    }
}
