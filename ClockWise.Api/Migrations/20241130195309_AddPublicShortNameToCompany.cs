using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClockWise.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPublicShortNameToCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicShortName",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicShortName",
                table: "Companies");
        }
    }
}
