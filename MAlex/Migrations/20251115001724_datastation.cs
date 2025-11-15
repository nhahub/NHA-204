using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MAlex.Migrations
{
    /// <inheritdoc />
    public partial class datastation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Stations",
                columns: new[] { "StationID", "ClosingTime", "Location", "OpeningTime", "StationName", "Status" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 23, 0, 0, 0), "Raml Square", new TimeSpan(0, 6, 0, 0, 0), "Raml", "Active" },
                    { 2, new TimeSpan(0, 23, 0, 0, 0), "El Raml District", new TimeSpan(0, 6, 0, 0, 0), "Mahattet El Raml", "Active" },
                    { 3, new TimeSpan(0, 22, 30, 0, 0), "Cleopatra District", new TimeSpan(0, 6, 0, 0, 0), "Cleopatra", "Active" },
                    { 4, new TimeSpan(0, 22, 30, 0, 0), "Sidi Gaber Area", new TimeSpan(0, 6, 0, 0, 0), "Sidi Gaber", "Active" },
                    { 5, new TimeSpan(0, 22, 30, 0, 0), "Sporting District", new TimeSpan(0, 6, 0, 0, 0), "Sporting", "Active" },
                    { 6, new TimeSpan(0, 22, 0, 0, 0), "Ibrahimia Area", new TimeSpan(0, 6, 0, 0, 0), "Ibrahimia", "Active" },
                    { 7, new TimeSpan(0, 22, 0, 0, 0), "Azarita District", new TimeSpan(0, 6, 0, 0, 0), "Azarita", "Active" },
                    { 8, new TimeSpan(0, 22, 0, 0, 0), "Shatby Main Road", new TimeSpan(0, 6, 0, 0, 0), "Shatby", "Active" },
                    { 9, new TimeSpan(0, 23, 0, 0, 0), "Smouha Square", new TimeSpan(0, 6, 0, 0, 0), "Smouha", "Active" },
                    { 10, new TimeSpan(0, 23, 0, 0, 0), "Kafr Abdou Street", new TimeSpan(0, 6, 0, 0, 0), "Kafr Abdou", "Active" },
                    { 11, new TimeSpan(0, 22, 30, 0, 0), "Victoria District", new TimeSpan(0, 6, 0, 0, 0), "Victoria", "Active" },
                    { 12, new TimeSpan(0, 22, 30, 0, 0), "Mandara Area", new TimeSpan(0, 6, 0, 0, 0), "Mandara", "Active" },
                    { 13, new TimeSpan(0, 22, 30, 0, 0), "Asafra Main Street", new TimeSpan(0, 6, 0, 0, 0), "Asafra", "Active" },
                    { 14, new TimeSpan(0, 22, 0, 0, 0), "Miami Corniche", new TimeSpan(0, 6, 0, 0, 0), "Miami", "Active" },
                    { 15, new TimeSpan(0, 22, 0, 0, 0), "Louran District", new TimeSpan(0, 6, 0, 0, 0), "Louran", "Active" },
                    { 16, new TimeSpan(0, 23, 0, 0, 0), "Sidi Bishr Area", new TimeSpan(0, 6, 0, 0, 0), "Sidi Bishr", "Active" },
                    { 17, new TimeSpan(0, 23, 0, 0, 0), "Asafra Beach Road", new TimeSpan(0, 6, 0, 0, 0), "Al Asafra Beach", "Active" },
                    { 18, new TimeSpan(0, 23, 0, 0, 0), "Montaza Gardens", new TimeSpan(0, 6, 0, 0, 0), "Montaza", "Active" },
                    { 19, new TimeSpan(0, 22, 0, 0, 0), "Stanley Bridge", new TimeSpan(0, 6, 0, 0, 0), "Stanley", "Active" },
                    { 20, new TimeSpan(0, 22, 0, 0, 0), "Gleem Area", new TimeSpan(0, 6, 0, 0, 0), "Gleem", "Active" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationID",
                keyValue: 20);
        }
    }
}
