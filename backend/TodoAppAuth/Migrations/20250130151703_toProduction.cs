using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TodoAppAuth.Migrations
{
    /// <inheritdoc />
    public partial class toProduction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "IsActive", "RoleName", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateOnly(2025, 1, 30), true, "Admin", new DateOnly(2025, 1, 30) },
                    { 2, new DateOnly(2025, 1, 30), true, "User", new DateOnly(2025, 1, 30) }
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "CreatedAt", "IsActive", "StatusName", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateOnly(2025, 1, 30), true, "Pendiente", new DateOnly(2025, 1, 30) },
                    { 2, new DateOnly(2025, 1, 30), true, "En proceso", new DateOnly(2025, 1, 30) },
                    { 3, new DateOnly(2025, 1, 30), true, "Completada", new DateOnly(2025, 1, 30) },
                    { 4, new DateOnly(2025, 1, 30), true, "Cancelada", new DateOnly(2025, 1, 30) }
                });
        }
    }
}
