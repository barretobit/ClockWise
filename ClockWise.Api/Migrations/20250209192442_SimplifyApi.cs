using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClockWise.Api.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyApi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Companies_CompanyId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkSessions_Employees_EmployeeId",
                table: "WorkSessions");

            migrationBuilder.DropIndex(
                name: "IX_WorkSessions_EmployeeId",
                table: "WorkSessions");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "Companies",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "Companies");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSessions_EmployeeId",
                table: "WorkSessions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Companies_CompanyId",
                table: "Employees",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSessions_Employees_EmployeeId",
                table: "WorkSessions",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
