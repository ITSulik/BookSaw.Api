using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookSaw.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOldPriceToBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OldPrice",
                table: "Books",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldPrice",
                table: "Books");
        }
    }
}
