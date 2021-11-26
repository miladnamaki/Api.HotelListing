using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelListing.Migrations
{
    public partial class ADDCounfigurationidentitrole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d363a5a3-6fc8-43de-9b37-0b5577e985aa", "a48a9ec5-8efd-4211-95ff-1d56faf5e9b1", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "25e213bb-f9e0-43ba-986a-a5431a6df25a", "0fccbfda-275c-4591-b0ad-a2a45e9dad6f", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "25e213bb-f9e0-43ba-986a-a5431a6df25a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d363a5a3-6fc8-43de-9b37-0b5577e985aa");
        }
    }
}
