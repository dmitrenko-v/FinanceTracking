using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeedCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Name", "AuthorId" },
                values: new object[,]
                {
                    { "Entertainment", null },
                    { "Food", null },
                    { "Healthcare", null },
                    { "Shopping", null },
                    { "Travel", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Entertainment");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Food");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Healthcare");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Shopping");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Travel");
        }
    }
}
