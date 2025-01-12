using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserManagementService.Migrations
{
    /// <inheritdoc />
    public partial class AddedExamplaryUsersToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccountVerificationToken", "Email", "IsActive", "IsVerified", "Name", "PasswordHash", "PasswordResetToken", "Role" },
                values: new object[,]
                {
                    { 1, null, "admin@admin.com", true, true, "Admin", "$2a$11$kzHkzDTWXpjO5w7m1NyNnOStn6JVrVIRC6pJy1GVH6j0M3ibmeESS", null, 1 },
                    { 2, null, "user@user.com", true, true, "User", "$2a$11$cMaEsjBmPxovlaf552I/F.KCoBgsbECyHXDyV73RRMaLANBEWGoZ6", null, 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
