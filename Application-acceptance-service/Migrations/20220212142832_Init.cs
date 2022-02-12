using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Application_acceptance_service.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applicants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    DateBirth = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CityBirth = table.Column<string>(type: "text", nullable: true),
                    AddressBirth = table.Column<string>(type: "text", nullable: true),
                    AddressCurrent = table.Column<string>(type: "text", nullable: true),
                    INN = table.Column<string>(type: "text", nullable: true),
                    SNILS = table.Column<string>(type: "text", nullable: true),
                    PassportNum = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestedCredits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreditType = table.Column<int>(type: "integer", nullable: false),
                    RequestedAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    RequestedCurrency = table.Column<string>(type: "text", nullable: true),
                    AnnualSalary = table.Column<decimal>(type: "numeric", nullable: false),
                    MonthlySalary = table.Column<decimal>(type: "numeric", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestedCredits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationNum = table.Column<string>(type: "text", nullable: true),
                    ApplicationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    BranchBank = table.Column<string>(type: "text", nullable: true),
                    BranchBankAddress = table.Column<string>(type: "text", nullable: true),
                    CreditManagerId = table.Column<long>(type: "bigint", nullable: false),
                    ScoringStatus = table.Column<bool>(type: "boolean", nullable: false),
                    ScoringDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ApplicantId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestedCreditId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_Applicants_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Applicants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Applications_RequestedCredits_RequestedCreditId",
                        column: x => x.RequestedCreditId,
                        principalTable: "RequestedCredits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApplicantId",
                table: "Applications",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_RequestedCreditId",
                table: "Applications",
                column: "RequestedCreditId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Applicants");

            migrationBuilder.DropTable(
                name: "RequestedCredits");
        }
    }
}
