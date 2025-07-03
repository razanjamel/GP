using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.Migrations
{
    /// <inheritdoc />
    public partial class AddDesignStatusToDesignRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "DesignRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "DesignRequests");
        }
    }
}
