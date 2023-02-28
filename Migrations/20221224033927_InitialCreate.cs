using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalBackend.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RaceResult",
                columns: table => new
                {
                    resultID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    racePosition = table.Column<string>(type: "TEXT", nullable: true),
                    driver = table.Column<string>(type: "TEXT", nullable: true),
                    teamName = table.Column<string>(type: "TEXT", nullable: true),
                    race = table.Column<string>(type: "TEXT", nullable: true),
                    raceDate = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceResult", x => x.resultID);
                });

            migrationBuilder.CreateTable(
                name: "Restaurant",
                columns: table => new
                {
                    restaurantID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    restaurantName = table.Column<string>(type: "TEXT", nullable: true),
                    restaurantStreetAddress = table.Column<string>(type: "TEXT", nullable: true),
                    restaurantSuburb = table.Column<string>(type: "TEXT", nullable: true),
                    restaurantCuisine = table.Column<string>(type: "TEXT", nullable: true),
                    priceTier = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant", x => x.restaurantID);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantUser",
                columns: table => new
                {
                    userId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    userName = table.Column<string>(type: "TEXT", nullable: true),
                    password = table.Column<string>(type: "TEXT", nullable: true),
                    dateJoined = table.Column<string>(type: "TEXT", nullable: true),
                    emailAddress = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantUser", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "Sponsors",
                columns: table => new
                {
                    sponsorID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    sponsorName = table.Column<string>(type: "TEXT", nullable: true),
                    sponsorLink = table.Column<string>(type: "TEXT", nullable: true),
                    sponsorStockName = table.Column<string>(type: "TEXT", nullable: true),
                    sponsorYear = table.Column<int>(type: "INTEGER", nullable: true),
                    teamName = table.Column<string>(type: "TEXT", nullable: true),
                    isCrypto = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sponsors", x => x.sponsorID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Message = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    reviewId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    timeWaited = table.Column<int>(type: "INTEGER", nullable: false),
                    requiredBooking = table.Column<string>(type: "TEXT", nullable: true),
                    weekDay = table.Column<string>(type: "TEXT", nullable: false),
                    reviewDate = table.Column<string>(type: "TEXT", nullable: false),
                    reviewTime = table.Column<string>(type: "TEXT", nullable: false),
                    RestaurantId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.reviewId);
                    table.ForeignKey(
                        name: "FK_Review_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "restaurantID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Review_RestaurantUser_UserId",
                        column: x => x.UserId,
                        principalTable: "RestaurantUser",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Review_RestaurantId",
                table: "Review",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_UserId",
                table: "Review",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RaceResult");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "Sponsors");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Restaurant");

            migrationBuilder.DropTable(
                name: "RestaurantUser");
        }
    }
}
