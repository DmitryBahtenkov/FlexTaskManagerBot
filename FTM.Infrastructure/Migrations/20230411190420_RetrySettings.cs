using FTM.Domain.Models.IssueModel;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FTM.Infrastructure.Migrations
{
    public partial class RetrySettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RetryDaysCount",
                table: "Issues");

            migrationBuilder.AddColumn<RetrySettings>(
                name: "RetrySettings",
                table: "Issues",
                type: "jsonb",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RetrySettings",
                table: "Issues");

            migrationBuilder.AddColumn<int>(
                name: "RetryDaysCount",
                table: "Issues",
                type: "integer",
                nullable: true);
        }
    }
}
