using CABESO.Properties;
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
                    FormId = table.Column<int>(nullable: true)
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
                    Id = table.Column<int>(nullable: false).Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Stream = table.Column<string>(nullable: false),
                    Enrolment = table.Column<int>(nullable: false)
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
                    Id = table.Column<int>(nullable: false).Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Sale = table.Column<decimal>(nullable: true),
                    Image = table.Column<string>(nullable: true),
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
                    Id = table.Column<int>(nullable: false).Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false, maxLength: 450),
                    ProductId = table.Column<int>(nullable: false),
                    OrderTime = table.Column<DateTime>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false, defaultValue: 1),
                    PreparationTime = table.Column<DateTime>(nullable: true),
                    CollectionTime = table.Column<DateTime>(nullable: false),
                    CollectionPlace = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.AddForeignKey("FK_OrderUserId", "Orders", "UserId", "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade, onUpdate: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey("FK_OrderProductId", "Orders", "ProductId", "Products", principalColumn: "Id", onDelete: ReferentialAction.Cascade, onUpdate: ReferentialAction.Cascade);

            migrationBuilder.CreateTable(
                name: "OrderHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false, maxLength: 450),
                    ProductId = table.Column<int>(nullable: false),
                    OrderTime = table.Column<DateTime>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false, defaultValue: 1),
                    PreparationTime = table.Column<DateTime>(nullable: true),
                    CollectionTime = table.Column<DateTime>(nullable: false),
                    CollectionPlace = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHistory", x => x.Id);
                });

            migrationBuilder.AddForeignKey("FK_OrderHistoryUserId", "OrderHistory", "UserId", "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade, onUpdate: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey("FK_OrderHistoryProductId", "OrderHistory", "ProductId", "Products", principalColumn: "Id", onDelete: ReferentialAction.Cascade, onUpdate: ReferentialAction.Cascade);

            migrationBuilder.CreateTable(
                name: "Allergens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false).Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergens", x => x.Id);
                });

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

            migrationBuilder.InsertData("Forms", new[] { "Stream", "Enrolment" }, new object[] { "", 2011 });
            migrationBuilder.InsertData("Forms", new[] { "Stream", "Enrolment" }, new object[] { "", 2012 });
            migrationBuilder.InsertData("Forms", new[] { "Stream", "Enrolment" }, new object[] { "", 2013 });

            migrationBuilder.InsertData("Codes", new[] { "Code", "CreationTime", "Role" }, new object[] { "1234567890", Database.SqlNow, Resources.Student });

            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "belegtes Brötchen mit Käse", 1.1, false, false, "1|4|8|12|22" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "belegtes Brötchen mit Salami", 1.1, false, false, "1|4|8|21|22" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "belegtes Brötchen mit Fleischwurst", 1.1, false, false, "1|4|8|22" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "belegtes Brötchen mit Kochschinken", 1.1, false, false, "1|4|8|22" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "belegtes Brötchen mit Pute", 1.1, false, false, "1|4|8|22" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "belegtes Brötchen mit Ei", 1.1, false, false, "1|4|8|22" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "belegtes Körnerbrötchen mit Käse", 1.1, false, false, "1|4|5|8|12|22|23" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "belegtes Körnerbrötchen mit Pute", 1.1, false, false, "1|4|5|8|22|23" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "belegtes Körnerbrötchen mit Kochschinken", 1.1, false, false, "1|4|5|8|22|23" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "belegtes Brötchen mit Käse überbacken", 1.2, false, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "belegtes Laugenbrötchen", 1.2, false, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Nutellabrötchen", 0.6, true, false, "1|4|11|12|14" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Käsebrötchen mit Nutella", 0.9, true, false, "1|11|12|14" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Knolli mit Tomate-Mozzarella", 1.2, true, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Sandwich mit Pute", 1.0, false, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Sandwich mit Gurke", 1.0, true, true, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Sandwich mit Thunfisch", 1.0, false, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "belegte Laugenstange", 1.2, false, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "belegter Wellenreiter", 1.6, false, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Baguette mit Schnitzel", 2.0, false, false, "1|8|22" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Baguette mit Frikadelle", 2.0, false, false, "1|8|12|22" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Baguette mit Hähnchen", 2.0, false, false, "1|8|22" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Brötchen mit Käse überbacken (Käsebrötchen)", 0.6, true, false, "1|12" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Laugenstange", 0.6, true, true, "1" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Bierstange", 0.9, true, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Laugenfalden mit Tomate, Basilikum und Käse", 1.4, true, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Laugenfalden mit Salami und geriebenen Käse", 1.4, false, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "belegetes Fladenbrot ", 0.9, false, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Laugenbrezel", 0.6, true, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "überbackene Laugenstange", 0.9, true, false, "1|12" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Laugenstange mit Frischkäse und Gurken", 1.2, true, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Hefezopf", 0.6, true, true, "1|8|12" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Schokocroissant", 0.8, true, false, "1|11|12" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Donut - bunt", 0.6, true, false, "1|8|11|12" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Donut - Konfetti", 0.6, true, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Donut - Kristalzucker", 0.6, true, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Donut - American Style", 0.6, true, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Donut - mini Black", 0.6, true, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Donut - mini White", 0.6, true, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Donut - mini Pink", 0.6, true, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Muffin", 0.9, true, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Kuchen", 0.9, true, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Berliner", 0.6, true, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Puddingbrezel", 0.9, true, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Apfeltasche", 0.9, true, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Streuselschnecke", 0.9, true, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Johannisbeer Streuseltaler", 0.9, true, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Streuseltaler black/white", 0.9, true, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Amerikaner", 0.9, true, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "normales Brötchen", 0.3, true, true, "1" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Baguette", 0.4, true, true, "1" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Körnerbrötchen", 0.5, true, true, "1|4|5|23" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Hot Dog", 1.1, false, false, "1|12" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Laugenfladen", 1.4, false, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Pizzabrötchen", 0.7, false, false, "" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Pizzastange", 1.1, true, false, "1|4|11|12" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Pizza Margherita", 1.3, true, false, "1|12" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Pizza mit Schinken", 1.4, false, false, "1|12" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Pizza mit Salami", 1.4, false, false, "1|12|21|22" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Pizza mit Thunfisch", 1.5, false, false, "1|12" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Lasagne", 2.4, false, false, "1|8|12" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "vegetarischer Salat", 2.5, true, true });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Salat mit Schinken", 2.5, false, false });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Allergens" }, new object[] { "Salat mit Hähnchen", 3.5, false, false });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan" }, new object[] { "Kinderriegel", 0.4, true, false });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan" }, new object[] { "Duplo", 0.4, true, false });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan" }, new object[] { "Hanuta", 0.4, true, false });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan" }, new object[] { "Corny", 0.4, true, false });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan" }, new object[] { "Milchschnitte", 0.4, true, false });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan" }, new object[] { "Mentos", 0.7, true, true });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan" }, new object[] { "TicTac", 0.7, true, true });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan" }, new object[] { "Mars", 0.7, true, false });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan" }, new object[] { "Snickers", 0.7, true, false });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan" }, new object[] { "Twix", 0.7, true, false });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan" }, new object[] { "Granini", 0.7, true, true });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Size", "Deposit" }, new object[] { "Coca-Cola", 0.9, true, true, "0,33 l", 0.15 });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Size", "Deposit" }, new object[] { "Sprite", 0.9, true, true, "0,33 l", 0.15 });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Size", "Deposit" }, new object[] { "Apfelschorle", 0.9, true, true, "0,33 l", 0.15 });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Size", "Deposit" }, new object[] { "Multivitaminsaft", 0.9, true, true, "0,33 l", 0.15 });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Size", "Deposit" }, new object[] { "Mineralwasser", 0.5, true, true, "0,33 l", 0.15 });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Size" }, new object[] { "Kakao", 0.4, true, false, "0,25 l" });
            migrationBuilder.InsertData("Products", new[] { "Name", "Price", "Vegetarian", "Vegan", "Size" }, new object[] { "Kakao", 0.7, true, false, "0,5 l" });

            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Weizen" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Dinkel" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Kamut" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Roggen" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Gerste" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Hafer" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Krebstiere" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Eier" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Fische" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Erdnüsse" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Sojabohnen" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Milch/Laktose" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Mandeln" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Haselnüsse" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Walnüsse" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Kaschunüsse" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Pecannüsse" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Paranüsse" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Pistazien" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Macadamianüsse" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Sellerie" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Senf" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Sesamsamen" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Schwefeldioxid und Sulphite > 10mg/kg oder > 10mg/l" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Lupinen" });
            migrationBuilder.InsertData("Allergens", new[] { "Description" }, new object[] { "Weichtiere" });

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
