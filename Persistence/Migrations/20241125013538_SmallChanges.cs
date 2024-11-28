using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SmallChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AccountTypes_AccountTypeName",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Cards_CardNumber",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_Cards_CardNumber",
                table: "Incomes");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Incomes_CardNumber",
                table: "Incomes");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_CardNumber",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Expenses");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Categories",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountTypeName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AccountTypes",
                column: "Type",
                values: new object[]
                {
                    "Base",
                    "Premium"
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", null, "User", "USER" },
                    { "2", null, "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AccountTypes_AccountTypeName",
                table: "AspNetUsers",
                column: "AccountTypeName",
                principalTable: "AccountTypes",
                principalColumn: "Type",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AccountTypes_AccountTypeName",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AccountTypes",
                keyColumn: "Type",
                keyValue: "Base");

            migrationBuilder.DeleteData(
                table: "AccountTypes",
                keyColumn: "Type",
                keyValue: "Premium");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "Incomes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "Expenses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "AccountTypeName",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Number = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Number);
                    table.ForeignKey(
                        name: "FK_Cards_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_CardNumber",
                table: "Incomes",
                column: "CardNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CardNumber",
                table: "Expenses",
                column: "CardNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_UserId",
                table: "Cards",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AccountTypes_AccountTypeName",
                table: "AspNetUsers",
                column: "AccountTypeName",
                principalTable: "AccountTypes",
                principalColumn: "Type");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Cards_CardNumber",
                table: "Expenses",
                column: "CardNumber",
                principalTable: "Cards",
                principalColumn: "Number",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_Cards_CardNumber",
                table: "Incomes",
                column: "CardNumber",
                principalTable: "Cards",
                principalColumn: "Number",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
