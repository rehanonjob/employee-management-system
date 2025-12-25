using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixAttendanceFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_attendances_Employees_EmployeeId",
                table: "attendances");

            migrationBuilder.DropIndex(
                name: "IX_attendances_EmployeeId",
                table: "attendances");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "attendances");

            migrationBuilder.CreateIndex(
                name: "IX_attendances_EmpId",
                table: "attendances",
                column: "EmpId");

            migrationBuilder.AddForeignKey(
                name: "FK_attendances_Employees_EmpId",
                table: "attendances",
                column: "EmpId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_attendances_Employees_EmpId",
                table: "attendances");

            migrationBuilder.DropIndex(
                name: "IX_attendances_EmpId",
                table: "attendances");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "attendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_attendances_EmployeeId",
                table: "attendances",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_attendances_Employees_EmployeeId",
                table: "attendances",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
