using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CABESO.Data.Migrations
{
    public partial class CreateIdentitySchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Form = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Forms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Codes",
                columns: table => new
                {
                    Code = table.Column<string>(maxLength: 10, fixedLength: true, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Role = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Sale = table.Column<decimal>(nullable: true),
                    Picture = table.Column<string>(nullable: true),
                    Allergens = table.Column<string>(nullable: true),
                    Vegetarian = table.Column<bool>(nullable: false),
                    Vegan = table.Column<bool>(nullable: false),
                    Size = table.Column<string>(nullable: true),
                    Deposit = table.Column<decimal>(nullable: true),
                    Information = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ClientId = table.Column<string>(nullable: false, maxLength: 450),
                    ProductId = table.Column<int>(nullable: false),
                    OrderTime = table.Column<DateTime>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false, defaultValue: 1),
                    CollectionTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.AddForeignKey("FK_OrderClientId", "Orders", "ClientId", "AspNetUsers", principalColumn: "Id");
            migrationBuilder.AddForeignKey("FK_OrderProductId", "Orders", "ProductId", "Products", principalColumn: "Id");

            migrationBuilder.CreateTable(
                name: "OrderHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ClientId = table.Column<string>(nullable: false, maxLength: 450),
                    ProductId = table.Column<int>(nullable: false),
                    OrderTime = table.Column<DateTime>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false, defaultValue: 1),
                    CollectionTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHistory", x => x.Id);
                });

            migrationBuilder.AddForeignKey("FK_OrderHistoryClientId", "OrderHistory", "ClientId", "AspNetUsers", principalColumn: "Id");
            migrationBuilder.AddForeignKey("FK_OrderHistoryProductId", "OrderHistory", "ProductId", "Products", principalColumn: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.InsertData("Forms", new[] { "Id", "Name", "Year" }, new object[] { 1, "Q1", 11 });
            migrationBuilder.InsertData("Codes", new[] { "Code", "CreationTime", "Role" }, new object[] { "1234567890", Database.SqlNow, "Student" });

            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 1, "belegtes Brötchen", 1.1, false, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 2, "belegtes Körnerbrötchen", 1.1, false, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 3, "belegtes überbackenes Käsebrötchen", 1.2, false, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 4, "belegtes Laugenbrötchen", 1.2, false, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 5, "Nutellabrötchen", 0.6, true, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 6, "Käsebrötchen mit Nutella", 0.9, true, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 7, "Knolli mit Tomate-Mozzarella", 1.2, true, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 8, "Sandwich mit Pute", 1.0, false, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 12, "Sandwich mit Gurke", 1.0, true, true });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 13, "Sandwich mit Thunfisch", 1.0, false, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 9, "belegte Laugenstange", 1.2, false, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 10, "belegter Wellenreiter", 1.6, false, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 11, "Baguette mit Schnitzel", 2.0, false, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 14, "Baguette mit Frikadelle", 2.0, false, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 15, "Baguette mit Hähnchen", 2.0, false, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 16, "überbackenes Käsebrötchen", 0.6, true, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 17, "Laugenstange", 0.6, true, true });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 18, "überbackene Laugenstange", 0.9, true, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 19, "Hefezopf", 0.6, true, true });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 20, "Schokocroissant", 0.8, true, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 21, "Donut", 0.6, true, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 22, "Kuchen", 0.9, true, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 23, "normales Brötchen", 0.3, true, true });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 24, "Baguette", 0.4, true, true });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 25, "Körnerbrötchen", 0.5, true, true });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 26, "Hot Dog", 1.1, false, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 27, "Laugenfladen", 1.4, false, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 28, "Pizzabrötchen", 0.7, false, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 29, "Pizzastange", 1.1, true, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 30, "Pizza Margherita", 1.3, true, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 31, "Pizza mit Schinken", 1.4, false, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 32, "Pizza mit Salami", 1.4, false, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 33, "Pizza mit Thunfisch", 1.5, false, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 34, "Lasagne", 2.4, false, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 35, "vegetarischer Salat", 2.5, true, true });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 36, "Salat mit Schinken", 2.5, false, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 37, "Salat mit Hähnchen", 3.5, false, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 38, "Kinderriegel", 0.4, true, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 39, "Duplo", 0.4, true, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 40, "Hanuta", 0.4, true, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 41, "Corny", 0.4, true, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 42, "Milchschnitte", 0.4, true, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 43, "Mentos", 0.7, true, true });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 44, "TicTac", 0.7, true, true });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 45, "Mars", 0.7, true, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 46, "Snickers", 0.7, true, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 47, "Twix", 0.7, true, false });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan" }, new object[] { 48, "Granini", 0.7, true, true });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan", "Size", "Deposit" }, new object[] { 49, "Coca-Cola", 0.9, true, true, "0,33 l", 0.15 });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan", "Size", "Deposit" }, new object[] { 50, "Sprite", 0.9, true, true, "0,33 l", 0.15 });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan", "Size", "Deposit" }, new object[] { 51, "Apfelschorle", 0.9, true, true, "0,33 l", 0.15 });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan", "Size", "Deposit" }, new object[] { 52, "Multivitaminsaft", 0.9, true, true, "0,33 l", 0.15 });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan", "Size", "Deposit" }, new object[] { 53, "Mineralwasser", 0.5, true, true, "0,33 l", 0.15 });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan", "Size" }, new object[] { 54, "Kakao", 0.4, true, false, "0,25 l" });
            migrationBuilder.InsertData("Products", new[] { "Id", "Name", "Price", "Vegetarian", "Vegan", "Size" }, new object[] { 55, "Kakao", 0.7, true, false, "0,5 l" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Forms");

            migrationBuilder.DropTable(
                name: "Codes");
        }
    }
}
