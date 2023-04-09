using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoansAppWebApi.Core.Migrations
{
    /// <inheritdoc />
    public partial class Googletypeadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthType",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthType",
                table: "AspNetUsers");
        }
    }
}
