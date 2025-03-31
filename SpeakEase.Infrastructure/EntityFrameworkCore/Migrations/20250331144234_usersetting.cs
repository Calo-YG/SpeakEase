using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SpeakEase.Infrastructure.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class usersetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "refresh_token",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "UserSetting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    Birthday = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Bio = table.Column<string>(type: "text", nullable: true),
                    IsProfilePublic = table.Column<bool>(type: "boolean", nullable: false),
                    ShowLearningProgress = table.Column<bool>(type: "boolean", nullable: false),
                    AllowMessages = table.Column<bool>(type: "boolean", nullable: false),
                    ReceiveNotifications = table.Column<bool>(type: "boolean", nullable: false),
                    ReceiveEmailUpdates = table.Column<bool>(type: "boolean", nullable: false),
                    ReceivePushNotifications = table.Column<bool>(type: "boolean", nullable: false),
                    AccountActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSetting", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSetting");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "refresh_token",
                type: "text",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
