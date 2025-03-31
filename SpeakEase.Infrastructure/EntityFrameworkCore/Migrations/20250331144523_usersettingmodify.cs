using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpeakEase.Infrastructure.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class usersettingmodify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSetting",
                table: "UserSetting");

            migrationBuilder.RenameTable(
                name: "UserSetting",
                newName: "user_setting");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_setting",
                table: "user_setting",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_user_setting",
                table: "user_setting");

            migrationBuilder.RenameTable(
                name: "user_setting",
                newName: "UserSetting");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSetting",
                table: "UserSetting",
                column: "Id");
        }
    }
}
