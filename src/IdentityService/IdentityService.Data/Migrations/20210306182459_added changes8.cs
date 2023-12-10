using Microsoft.EntityFrameworkCore.Migrations;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class addedchanges8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                "Message",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "Message",
                "AccountJournalEntry");
        }
    }
}