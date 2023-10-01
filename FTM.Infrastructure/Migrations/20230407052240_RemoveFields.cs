using System;
using FTM.Domain.Models.IssueModel;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FTM.Infrastructure.Migrations
{
    public partial class RemoveFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "Steps",
                table: "Issues");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "Issues",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Step[]>(
                name: "Steps",
                table: "Issues",
                type: "jsonb",
                nullable: false,
                defaultValue: new Step[0]);
        }
    }
}
