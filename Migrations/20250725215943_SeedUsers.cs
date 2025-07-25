using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PropertyListingAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName", "IsApproved", "PasswordHash", "PasswordSalt", "PhoneNumber", "Role" },
                values: new object[,]
                {
                    { 1, "Fortunatekgaogelo@gmail.com", "Kgaogelo Tshabalala", true, new byte[] { 237, 23, 189, 46, 142, 43, 190, 133, 62, 120, 191, 13, 184, 55, 206, 212, 92, 20, 179, 37, 242, 13, 122, 9, 92, 21, 245, 95, 198, 175, 20, 150, 221, 182, 242, 148, 225, 189, 196, 113, 242, 119, 90, 185, 7, 42, 74, 23, 131, 173, 204, 45, 166, 34, 225, 82, 206, 93, 64, 198, 193, 82, 243, 41 }, new byte[] { 7, 23, 244, 83, 215, 190, 142, 92, 0, 230, 194, 88, 223, 229, 38, 27, 219, 60, 181, 178, 140, 161, 10, 55, 129, 156, 184, 22, 215, 246, 130, 104, 107, 61, 238, 235, 153, 149, 43, 69, 44, 77, 226, 127, 175, 77, 198, 248, 205, 220, 88, 4, 38, 103, 81, 30, 195, 224, 156, 240, 147, 131, 151, 143 }, "0728945924", 1 },
                    { 2, "agent@example.com", "Agent User", true, new byte[] { 237, 23, 189, 46, 142, 43, 190, 133, 62, 120, 191, 13, 184, 55, 206, 212, 92, 20, 179, 37, 242, 13, 122, 9, 92, 21, 245, 95, 198, 175, 20, 150, 221, 182, 242, 148, 225, 189, 196, 113, 242, 119, 90, 185, 7, 42, 74, 23, 131, 173, 204, 45, 166, 34, 225, 82, 206, 93, 64, 198, 193, 82, 243, 41 }, new byte[] { 7, 23, 244, 83, 215, 190, 142, 92, 0, 230, 194, 88, 223, 229, 38, 27, 219, 60, 181, 178, 140, 161, 10, 55, 129, 156, 184, 22, 215, 246, 130, 104, 107, 61, 238, 235, 153, 149, 43, 69, 44, 77, 226, 127, 175, 77, 198, 248, 205, 220, 88, 4, 38, 103, 81, 30, 195, 224, 156, 240, 147, 131, 151, 143 }, "0728945924", 2 },
                    { 3, "tenant@example.com", "Tenant User", false, new byte[] { 237, 23, 189, 46, 142, 43, 190, 133, 62, 120, 191, 13, 184, 55, 206, 212, 92, 20, 179, 37, 242, 13, 122, 9, 92, 21, 245, 95, 198, 175, 20, 150, 221, 182, 242, 148, 225, 189, 196, 113, 242, 119, 90, 185, 7, 42, 74, 23, 131, 173, 204, 45, 166, 34, 225, 82, 206, 93, 64, 198, 193, 82, 243, 41 }, new byte[] { 7, 23, 244, 83, 215, 190, 142, 92, 0, 230, 194, 88, 223, 229, 38, 27, 219, 60, 181, 178, 140, 161, 10, 55, 129, 156, 184, 22, 215, 246, 130, 104, 107, 61, 238, 235, 153, 149, 43, 69, 44, 77, 226, 127, 175, 77, 198, 248, 205, 220, 88, 4, 38, 103, 81, 30, 195, 224, 156, 240, 147, 131, 151, 143 }, "0728945924", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
