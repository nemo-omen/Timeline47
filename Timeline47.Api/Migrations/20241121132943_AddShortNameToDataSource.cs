using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Timeline47.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddShortNameToDataSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "DataSources",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "DataSources");
        }
    }
}
