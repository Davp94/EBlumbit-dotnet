using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBlumbit.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNombresApellidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_permissions_roles_RoleId",
                table: "permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_refresh_tokens_users_UsersId",
                table: "refresh_tokens");

            migrationBuilder.DropIndex(
                name: "IX_refresh_tokens_UsersId",
                table: "refresh_tokens");

            migrationBuilder.DropIndex(
                name: "IX_permissions_RoleId",
                table: "permissions");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "permissions");

            migrationBuilder.AddColumn<string>(
                name: "apellidos",
                table: "users",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nombres",
                table: "users",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "apellidos",
                table: "users");

            migrationBuilder.DropColumn(
                name: "nombres",
                table: "users");

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "refresh_tokens",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "permissions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_UsersId",
                table: "refresh_tokens",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_permissions_RoleId",
                table: "permissions",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_permissions_roles_RoleId",
                table: "permissions",
                column: "RoleId",
                principalTable: "roles",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_tokens_users_UsersId",
                table: "refresh_tokens",
                column: "UsersId",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
