using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PayrollSystem.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class PayrollTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Leaves");

            migrationBuilder.AlterColumn<double>(
                name: "HoursWorked",
                table: "TimeSheets",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "TimeSheets",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "LeaveTypes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<double>(
                name: "LeaveDuration",
                table: "Leaves",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "Leaves",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<double>(
                name: "HourlyRate",
                table: "Employees",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<double>(
                name: "BasicSalary",
                table: "Employees",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.CreateTable(
                name: "PayAdjustments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Recurring = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayAdjustments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payrolls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    AllowOT = table.Column<bool>(type: "boolean", nullable: false),
                    DeductNoPay = table.Column<bool>(type: "boolean", nullable: false),
                    Total = table.Column<decimal>(type: "numeric", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payrolls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Taxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    StartAmount = table.Column<double>(type: "double precision", nullable: false),
                    EndAmount = table.Column<double>(type: "double precision", nullable: false),
                    Percentage = table.Column<double>(type: "double precision", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeePayAdjustments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    PayAdjustmentId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePayAdjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeePayAdjustments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeePayAdjustments_PayAdjustments_PayAdjustmentId",
                        column: x => x.PayAdjustmentId,
                        principalTable: "PayAdjustments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Paysheet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BasicSalary = table.Column<double>(type: "double precision", nullable: false),
                    OverTimePay = table.Column<double>(type: "double precision", nullable: false),
                    TotalAllowances = table.Column<double>(type: "double precision", nullable: false),
                    GrossPay = table.Column<double>(type: "double precision", nullable: false),
                    TotalDeductions = table.Column<double>(type: "double precision", nullable: false),
                    EPF_EmployeePay = table.Column<double>(type: "double precision", nullable: false),
                    EPF_EmployerPay = table.Column<double>(type: "double precision", nullable: false),
                    ETF = table.Column<double>(type: "double precision", nullable: false),
                    NetPay = table.Column<double>(type: "double precision", nullable: false),
                    PayrollId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paysheet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paysheet_Payrolls_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payrolls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaysheetRecord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    PaysheetId = table.Column<int>(type: "integer", nullable: false),
                    EmployeePayAdjustment = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaysheetRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaysheetRecord_Paysheet_PaysheetId",
                        column: x => x.PaysheetId,
                        principalTable: "Paysheet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxRecord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    StartAmount = table.Column<double>(type: "double precision", nullable: false),
                    EndAmount = table.Column<double>(type: "double precision", nullable: false),
                    Percentage = table.Column<double>(type: "double precision", nullable: false),
                    TaxId = table.Column<int>(type: "integer", nullable: false),
                    PaysheetId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxRecord_Paysheet_PaysheetId",
                        column: x => x.PaysheetId,
                        principalTable: "Paysheet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsPaid",
                value: true);

            migrationBuilder.UpdateData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsPaid",
                value: true);

            migrationBuilder.UpdateData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsPaid",
                value: true);

            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "Id", "CreatedDate", "IsActive", "IsPaid", "MaximumAllowedDays", "Type", "UpdatedDate" },
                values: new object[] { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, 365f, "Nopay Leave", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "PayAdjustments",
                columns: new[] { "Id", "CreatedDate", "IsActive", "Name", "Recurring", "Type", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 3, 10, 19, 17, 51, 183, DateTimeKind.Utc).AddTicks(5113), true, "Incentive", true, 0, new DateTime(2025, 3, 10, 19, 17, 51, 183, DateTimeKind.Utc).AddTicks(5114) },
                    { 2, new DateTime(2025, 3, 10, 19, 17, 51, 183, DateTimeKind.Utc).AddTicks(5118), true, "Transport", true, 0, new DateTime(2025, 3, 10, 19, 17, 51, 183, DateTimeKind.Utc).AddTicks(5118) },
                    { 3, new DateTime(2025, 3, 10, 19, 17, 51, 183, DateTimeKind.Utc).AddTicks(5120), true, "Medical Claim", false, 0, new DateTime(2025, 3, 10, 19, 17, 51, 183, DateTimeKind.Utc).AddTicks(5120) }
                });

            migrationBuilder.InsertData(
                table: "Taxes",
                columns: new[] { "Id", "CreatedDate", "Description", "EndAmount", "IsActive", "Percentage", "StartAmount", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 3, 10, 19, 17, 51, 183, DateTimeKind.Utc).AddTicks(3035), "PAYEE", 100000.0, true, 0.0, 0.0, new DateTime(2025, 3, 10, 19, 17, 51, 183, DateTimeKind.Utc).AddTicks(3385) },
                    { 2, new DateTime(2025, 3, 10, 19, 17, 51, 183, DateTimeKind.Utc).AddTicks(3785), "PAYEE", 141667.0, true, 0.059999999999999998, 100000.0, new DateTime(2025, 3, 10, 19, 17, 51, 183, DateTimeKind.Utc).AddTicks(3786) },
                    { 3, new DateTime(2025, 3, 10, 19, 17, 51, 183, DateTimeKind.Utc).AddTicks(3788), "PAYEE", 183333.0, true, 0.12, 141667.0, new DateTime(2025, 3, 10, 19, 17, 51, 183, DateTimeKind.Utc).AddTicks(3788) },
                    { 4, new DateTime(2025, 3, 10, 19, 17, 51, 183, DateTimeKind.Utc).AddTicks(3790), "PAYEE", 225000.0, true, 0.17999999999999999, 183333.0, new DateTime(2025, 3, 10, 19, 17, 51, 183, DateTimeKind.Utc).AddTicks(3790) },
                    { 5, new DateTime(2025, 3, 10, 19, 17, 51, 183, DateTimeKind.Utc).AddTicks(3792), "PAYEE", 266667.0, true, 0.23999999999999999, 225000.0, new DateTime(2025, 3, 10, 19, 17, 51, 183, DateTimeKind.Utc).AddTicks(3792) },
                    { 6, new DateTime(2025, 3, 10, 19, 17, 51, 183, DateTimeKind.Utc).AddTicks(3794), "PAYEE", 308333.0, true, 0.29999999999999999, 266667.0, new DateTime(2025, 3, 10, 19, 17, 51, 183, DateTimeKind.Utc).AddTicks(3794) },
                    { 7, new DateTime(2025, 3, 10, 19, 17, 51, 183, DateTimeKind.Utc).AddTicks(3796), "PAYEE", 3.4028234663852886E+38, true, 0.35999999999999999, 308333.0, new DateTime(2025, 3, 10, 19, 17, 51, 183, DateTimeKind.Utc).AddTicks(3796) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_LeaveTypeId",
                table: "Leaves",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayAdjustments_EmployeeId",
                table: "EmployeePayAdjustments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayAdjustments_PayAdjustmentId",
                table: "EmployeePayAdjustments",
                column: "PayAdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Paysheet_PayrollId",
                table: "Paysheet",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_PaysheetRecord_PaysheetId",
                table: "PaysheetRecord",
                column: "PaysheetId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxRecord_PaysheetId",
                table: "TaxRecord",
                column: "PaysheetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_LeaveTypes_LeaveTypeId",
                table: "Leaves",
                column: "LeaveTypeId",
                principalTable: "LeaveTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_LeaveTypes_LeaveTypeId",
                table: "Leaves");

            migrationBuilder.DropTable(
                name: "EmployeePayAdjustments");

            migrationBuilder.DropTable(
                name: "PaysheetRecord");

            migrationBuilder.DropTable(
                name: "Taxes");

            migrationBuilder.DropTable(
                name: "TaxRecord");

            migrationBuilder.DropTable(
                name: "PayAdjustments");

            migrationBuilder.DropTable(
                name: "Paysheet");

            migrationBuilder.DropTable(
                name: "Payrolls");

            migrationBuilder.DropIndex(
                name: "IX_Leaves_LeaveTypeId",
                table: "Leaves");

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "LeaveTypes");

            migrationBuilder.AlterColumn<decimal>(
                name: "HoursWorked",
                table: "TimeSheets",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "TimeSheets",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<float>(
                name: "LeaveDuration",
                table: "Leaves",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Leaves",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Leaves",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "HourlyRate",
                table: "Employees",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<decimal>(
                name: "BasicSalary",
                table: "Employees",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }
    }
}
