using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.Migrations
{
    /// <inheritdoc />
    public partial class AddDesignRequestIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DesignRequests",
                table: "DesignRequests");

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "DesignRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DesignRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DesignRequests",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DesignRequests",
                table: "DesignRequests",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DesignRequests",
                table: "DesignRequests");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DesignRequests");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DesignRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "DesignRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DesignRequests",
                table: "DesignRequests",
                column: "UserId");
        }
    }
}
