using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketplace.Modules.Listings.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRentalAndVacancies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ListingRentals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ListingId = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Rooms = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Area = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Floor = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Features = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingRentals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingRentals_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListingVacancies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ListingId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    EmploymentType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    SalaryText = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    LocationText = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingVacancies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingVacancies_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListingRentalRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RentalId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Price = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Area = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Guests = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Beds = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Amenities = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrls = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingRentalRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingRentalRooms_ListingRentals_RentalId",
                        column: x => x.RentalId,
                        principalTable: "ListingRentals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListingRentalRooms_RentalId",
                table: "ListingRentalRooms",
                column: "RentalId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingRentals_ListingId",
                table: "ListingRentals",
                column: "ListingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ListingVacancies_ListingId",
                table: "ListingVacancies",
                column: "ListingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListingRentalRooms");

            migrationBuilder.DropTable(
                name: "ListingVacancies");

            migrationBuilder.DropTable(
                name: "ListingRentals");
        }
    }
}
