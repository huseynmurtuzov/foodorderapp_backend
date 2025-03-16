using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YemekSepeti.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryPersonnel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VeichleType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPersonnel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkingHours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CategoryRestaurant",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    RestaurantsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryRestaurant", x => new { x.CategoriesId, x.RestaurantsId });
                    table.ForeignKey(
                        name: "FK_CategoryRestaurant_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CategoryRestaurant_Restaurants_RestaurantsId",
                        column: x => x.RestaurantsId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CustomerRestaurant",
                columns: table => new
                {
                    CustomersId = table.Column<int>(type: "int", nullable: false),
                    RestaurantsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRestaurant", x => new { x.CustomersId, x.RestaurantsId });
                    table.ForeignKey(
                        name: "FK_CustomerRestaurant_Customers_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CustomerRestaurant_Restaurants_RestaurantsId",
                        column: x => x.RestaurantsId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Meals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meals_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    DeliveryPersonelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Orders_DeliveryPersonnel_DeliveryPersonelId",
                        column: x => x.DeliveryPersonelId,
                        principalTable: "DeliveryPersonnel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Orders_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestaurantReviews_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RestaurantReviews_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CategoryMeal",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    MealsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryMeal", x => new { x.CategoriesId, x.MealsId });
                    table.ForeignKey(
                        name: "FK_CategoryMeal_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CategoryMeal_Meals_MealsId",
                        column: x => x.MealsId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MealOrder",
                columns: table => new
                {
                    MealsId = table.Column<int>(type: "int", nullable: false),
                    OrdersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealOrder", x => new { x.MealsId, x.OrdersId });
                    table.ForeignKey(
                        name: "FK_MealOrder_Meals_MealsId",
                        column: x => x.MealsId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MealOrder_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[] { -1, "Fast Food" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "Email", "Name", "Password", "PhoneNumber" },
                values: new object[] { -9, "Yakutiye,Erzurum", "fatimemurtuzova@gmail.com", "Fatime Murtuzova", "Fatime1234!", "05053661826" });

            migrationBuilder.InsertData(
                table: "DeliveryPersonnel",
                columns: new[] { "Id", "Email", "Name", "Password", "PhoneNumber", "VeichleType" },
                values: new object[] { -10, "hakanyildiz@gmail.com", "Hakan Yıldız", "Hakan1234!", "0538 613 36 08", "Araba" });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "Address", "Email", "Image", "Name", "Password", "PhoneNumber", "Rating", "WorkingHours" },
                values: new object[,]
                {
                    { -8, "Bu restoranın adres bilgileri MirtitOrder'da bulunmaktadır", "ustadonerci@gmail.com", "https://images.deliveryhero.io/image/fd-tr/tr-logos/cj7mp-logo.jpg", "Usta Dönerci", "Ustadonerci1234!", "(0442) 343 99 99", 3.2000000000000002, "11:00 - 22:00" },
                    { -7, "Taşmağazalar Cd. No: 16", "lapidispide@gmail.com", "https://images.deliveryhero.io/image/fd-tr/LH/gp7a-listing.jpg", "Lapidis Pide", "Lapidispide1234!", "(0442) 215 48 25", 4.4000000000000004, "11:00 - 22:00" },
                    { -6, "Bu restoranın adres bilgileri MirtitOrder'da bulunmaktadır", "oncudoner@gmail.com", "https://images.deliveryhero.io/image/fd-tr/tr-logos/co9zi-logo.jpg", "Öncü Döner", "Oncudoner1234!", "(0442) 233 05 05", 3.7999999999999998, "10:30 - 02:00" },
                    { -5, "Yukarı Mumcu Mah. Cumhuriyet Caddesi No: 22/3 MGZ No: 100 Palerium AVM Yakutiye", "popeyes@gmail.com", "https://images.deliveryhero.io/image/fd-tr/tr-logos/cr5ff-logo.jpg", "Popeyes", "Poeyes1234!", "444 7 677", 3.2999999999999998, "11:00 - 22:00" },
                    { -4, "GEZ Mah. Spor Yolu Sokak Alin Sitesi NO:52 Düzgün Market ilerisi YAKUTİYE ERZURUM", "cheffpizza@gmail.com", "https://images.deliveryhero.io/image/fd-tr/LH/jbue-listing.jpg", "Cheff Pizza", "Chefpizza1234!", "0553 697 79 42", 4.4000000000000004, "11:00 - 02:00" },
                    { -3, "Palerium AVM,No:22/3 Mağaza:100", "burgerking@gmail.com", "https://images.deliveryhero.io/image/fd-tr/tr-logos/cl3by-logo.jpg", "Burger King", "Burgerking1234!", "444 5 464", 3.7999999999999998, "11:00 - 22:00" },
                    { -2, "Ömer Nasuhi Bilmen Mahallesi Kombina Caddesi No:9 Yakutiye Erzurum", "musqaburger@gmail.com", "https://images.deliveryhero.io/image/fd-tr/tr-logos/ch8or-logo.jpg", "Musqa Burger", "Musqaburger1234!", "0545 442 85 25", 3.8999999999999999, "11:00 - 01:00" },
                    { -1, "MERSIS No: 0773072861500001", "maydanozdoner@gmail.com", "https://images.deliveryhero.io/image/fd-tr/tr-logos/cp9bu-logo.jpg", "Maydonoz Döner", "Maydanozdoner1234!", "(0442) 238 23 22", 3.2999999999999998, "11:00 - 23:59" }
                });

            migrationBuilder.InsertData(
                table: "Meals",
                columns: new[] { "Id", "Description", "Image", "IsAvailable", "Name", "Price", "Quantity", "RestaurantId" },
                values: new object[,]
                {
                    { -64, "3 adet Poppy Sandviç + Patates Kızartması (Büyük) + Nuggets (12’li) + Tenders (3'lü) + 1L. İçecek", "https://images.deliveryhero.io/image/fd-tr/Products/65679780.jpg??width=500", true, "Beşiktaş Taraftar Menüsü", 430m, 0, -5 },
                    { -63, "2 Adet Tavukburger® + Patates Kızartması (Orta) + 1L. İçecek", "https://images.deliveryhero.io/image/fd-tr/Products/28499447.jpg??width=500", true, "Ekonomix Menü", 220m, 0, -5 },
                    { -62, "Popchicken + Büyük Boy Patates + 4’lü Soğan Halkası + 4’lü Nuggets + Magnum Mini Badem + Kutu İçecek", "https://images.deliveryhero.io/image/fd-tr/Products/68486480.jpg??width=500", true, "Algida Menüsü (Algida Keyif)", 270m, 0, -5 },
                    { -61, "Tavuk, mozzarella peyniri, pizza sosu", "https://images.deliveryhero.io/image/fd-tr/Products/6492527.jpg??width=500", true, "Cheff Chicken Pizza (Büyük Boy)", 275m, 0, -4 },
                    { -60, "250 gr. patates kızartması, 4 adet nugget, 4 adet soğan halkası, 4 adet mozzarella sticks", "https://images.deliveryhero.io/image/fd-tr/Products/35603055.jpg??width=500", true, "Cheff Box", 199m, 0, -4 },
                    { -59, "4 adet", "https://images.deliveryhero.io/image/fd-tr/Products/6492540.jpg??width=500", true, "Mozzarella Sticks (4 Adet)", 65m, 0, -4 },
                    { -58, "3 Adet Seçeceğiniz Sandviç + Patates Kızartması (Büyük) + 1 L. İçecek", "https://images.deliveryhero.io/image/fd-tr/Products/27388702.jpg??width=500", true, "Benim Üçlüm", 295m, 0, -3 },
                    { -57, "Double Cheese Burger + Patates Kızartması (Orta) + Seçeceğin Algida Max + Kutu İçecek", "https://images.deliveryhero.io/image/fd-tr/Products/28459724.jpg??width=500", true, "Tek Kişilik Algida Menü", 235m, 0, -3 },
                    { -56, "Big King® + Whopper® Jr. + Chicken Royale® + Tavuklu Barbekü Deluxe Burger + Patates Kızartması (Büyük) + Soğan Halkası (8'li) + Algida Frigola (570 ml.) yada Algida Maraş Usulü Sade Dondurma (500 ml.) + 1 L. İçecek", "https://images.deliveryhero.io/image/fd-tr/Products/42100301.jpg??width=500", true, "Algida Menü (Algida’lı Dev Menü)", 600m, 0, -3 },
                    { -55, "Ev yapımı hamburger ekmeği, ev yapımı 3 x 70 gr. hamburger köftesi, Musqa sos, közlenmiş biber, 3 adet cheddar peyniri, double dana jambon, karamelize soğan. Patates kızartması ile", "https://images.deliveryhero.io/image/fd-tr/Products/26726741.jpg??width=500", true, "Chef - X Burger", 377.90m, 0, -2 },
                    { -54, "Ev yapımı hamburger ekmeği, ev yapımı hamburger köftesi, sarımsaklı mayonez, ince dilim bonfile, domates, soğan, cheddar peyniri. Patates kızartması ile", "https://images.deliveryhero.io/image/fd-tr/Products/26726101.jpg??width=500", true, "Musqover Burger", 318.90m, 0, -2 },
                    { -53, "Ev yapımı hamburger ekmeği, 2 x 70 ev yapım hamburger köftesi, kaşar peyniri (2 adet), cheddar peyniri (2 adet), ortası et soslu lokum bonfile, Musqa sos. Patates kızartması ile", "https://images.deliveryhero.io/image/fd-tr/Products/26725832.jpg??width=500", true, "Big Bang Burger", 333.90m, 0, -2 },
                    { -52, "4 Adet Maytako Tavuk Döner + 2'li Külah Patates Kızartması + İçecek (1 L.)", "https://images.deliveryhero.io/image/fd-tr/Products/10775565.jpg??width=500", true, "Maytako Parti (Tavuk Dönerden)", 520m, 0, -1 },
                    { -51, "2 Adet Maytako (50 gr. et döner, tortilla lavaş, chedar peyniri, salatalık turşusu, patates kızartması, soğan, acılı sarımsaklı mayonez sos. Cin biberi ile)", "https://images.deliveryhero.io/image/fd-tr/Products/10775562.jpg??width=500", true, "Maytako (Et Dönerden) 2 Adet", 280m, 0, -1 },
                    { -50, "2 Adet Maytako (50 gr. tavuk döner, tortilla lavaş, chedar peyniri, salatalık turşusu, patates kızartması, soğan, acılı sarımsaklı mayonez sos. Cin biberi ile)", "https://images.deliveryhero.io/image/fd-tr/Products/10775561.jpg??width=500", true, "Maytako (Tavuk Dönerden) 2 Adet", 190m, 0, -1 },
                    { -49, "3 Adet Dürüm Tavuk Döner + Patates Kızartması (Büyük) + Soğan Halkası (6'lı) + Coca-Cola (1 L.)", "https://images.deliveryhero.io/image/fd-tr/Products/37095605.jpg??width=500", true, "Coca-Cola Fırsatı (3'lü Dürüm Menü)", 440m, 1, -8 },
                    { -48, "3 Adet Baget Tavuk Döner + Patates Kızartması (Büyük) + Soğan Halkası (6’lı) + Coca-Cola (1 L.)", "https://images.deliveryhero.io/image/fd-tr/Products/37095608.jpg??width=500", true, "Coca-Cola Fırsatı (Seçmeli Baget Menü)", 260m, 1, -8 },
                    { -47, "Dürüm Tavuk Döner + Patates Kızartması (Orta) + Ayran (20 cl.)", "https://images.deliveryhero.io/image/fd-tr/Products/37095642.jpg??width=500", true, "Dürüm Tavuk Döner Menü", 190m, 1, -8 },
                    { -46, "Dürüm Et Döner + Patates Kızartması (Orta) + Ayran (20 cl.)", "https://images.deliveryhero.io/image/fd-tr/Products/37095643.jpg??width=500", true, "Dürüm Et Döner Menü", 240m, 1, -8 },
                    { -45, "2 Adet Baget Tavuk Döner + Patates Kızartması (Orta) + Ayran (20 cl.)", "https://images.deliveryhero.io/image/fd-tr/Products/12742732.jpg??width=500", true, "2'li Baget Tavuk Döner Menü", 185m, 1, -8 },
                    { -44, "2 Adet Tombik Et Döner + Patates Kızartması (Orta) + (İçecek 1 L.)", "https://images.deliveryhero.io/image/fd-tr/Products/37095624.jpg??width=500", true, "2’li Tombik Et Döner Menü", 340m, 1, -8 },
                    { -43, "İskender sosu, İskender tereyağı, sivri biber, domates, yoğurt", "https://images.deliveryhero.io/image/fd-tr/Products/37095662.jpg??width=500", true, "UD® Et İskender", 255m, 1, -8 },
                    { -42, "3 Adet Baget Et Döner + Patates Kızartması (Büyük) + Soğan Halkası (6’lı) + İçecek (1 L.)", "https://images.deliveryhero.io/image/fd-tr/Products/37095618.jpg??width=500", true, "3'lü Baget Et Döner Menü", 360m, 1, -8 },
                    { -41, "Tavuk döner, kaşar peyniri", "https://images.deliveryhero.io/image/fd-tr/Products/66786044.jpg??width=500", true, "Döner Pide", 170m, 1, -7 },
                    { -40, "Dana sucuk, taze kaşar peyniri", "https://images.deliveryhero.io/image/fd-tr/Products/61276438.jpg??width=500", true, "Sucuklu Kaşarlı Pide", 240m, 1, -7 },
                    { -39, "Günlük, taze, sinirsiz et", "https://images.deliveryhero.io/image/fd-tr/Products/61276436.jpg??width=500", true, "Kuşbaşılı Pide", 260m, 1, -7 },
                    { -38, "Kıyma, kavurma, kaşar peyniri, salam, sucuk, sosis", "https://images.deliveryhero.io/image/fd-tr/Products/65580785.jpg??width=500", true, "Açık Lapidis Özel Karışık", 270m, 1, -7 },
                    { -37, "Özel kavurmalı", "https://images.deliveryhero.io/image/fd-tr/Products/61276439.jpg??width=500", true, "Kavurmalı Pide (Kapalı)", 290m, 1, -7 },
                    { -36, "Kıyma, soğan", "https://images.deliveryhero.io/image/fd-tr/Products/61276434.jpg??width=500", true, "Kapalı Kıymalı Pide", 210m, 1, -7 },
                    { -35, "Salata, sumak, meze ile", "https://images.deliveryhero.io/image/fd-tr/Products/9802942.jpg??width=500", true, "Lahmacun", 80m, 1, -7 },
                    { -34, "Kuşbaşı et, kaşar peyniri", "https://images.deliveryhero.io/image/fd-tr/Products/61276437.jpg??width=500", true, "Kuşbaşılı Kaşarlı Pide", 260m, 1, -7 },
                    { -33, "Öncü Zurna Tavuk Döner Dürüm (120 gr.) + Patates Kızartması + İçecek (33 cl.)", "https://images.deliveryhero.io/image/fd-tr/Products/61471097.jpg??width=500", true, "Combo Öncü Zurna Tavuk Döner Dürüm Menü", 212.42m, 1, -6 },
                    { -32, "Tavuk Döner Dürüm (80 gr.) + Patates Kızartması + İçecek (33 cl.)", "https://images.deliveryhero.io/image/fd-tr/Products/61470321.jpg??width=500", true, "Combo Tavuk Döner Dürüm Menü", 178.42m, 1, -6 },
                    { -31, "2 Adet Tavuk Döner Dürüm (80 gr.) + Patates Kızartması + içecek (1 L.)", "https://images.deliveryhero.io/image/fd-tr/Products/61482869.jpg??width=500", true, "İkili Menü", 305.91m, 1, -6 },
                    { -30, "100 gr.", "https://images.deliveryhero.io/image/fd-tr/Products/61470328.jpg??width=500", true, "Patates Kızartması (100 gr.)", 42.41m, 1, -6 },
                    { -29, "Tavuk Döner Dürüm (80 gr.) + Patates Kızartması + Sütaş Ayran (27.5 cl.)", "https://images.deliveryhero.io/image/fd-tr/Products/61470317.jpg??width=500", true, "Avantajlı Tavuk Döner Dürüm Menü", 174.24m, 1, -6 },
                    { -28, "120 gr. tavuk döner, patates kızartması, turşu, özel Öncü sos, sarımsaklı mayonez", "https://images.deliveryhero.io/image/fd-tr/Products/61470323.jpg??width=500", true, "Öncü Zurna Tavuk Döner Dürüm (120 gr.)", 144.42m, 1, -6 },
                    { -27, "2 Adet Tavuk Döner Dürüm (80 gr.) + 2 Adet Patates Kızartması + İçecek (1 L.)", "https://images.deliveryhero.io/image/fd-tr/Products/30279841.jpg??width=500", true, "Kazandıran Menü", 229.49m, 1, -6 },
                    { -26, "80 gr. tavuk döner, patates kızartması, turşu, özel Öncü sos, sarımsaklı mayonez", "https://images.deliveryhero.io/image/fd-tr/Products/61470322.jpg??width=500", true, "Tavuk Döner Dürüm (80 gr.)", 110.42m, 1, -6 },
                    { -25, "3 Adet Tavukburger® + Nuggets (12'li) + Patates Kızartması (Büyük) + Seçeceğiniz 2 Adet Sos + 1L. İçecek", "https://images.deliveryhero.io/image/fd-tr/Products/28499435.jpg??width=500", true, "Coca-Cola Fırsatı (3 Kişilik Doyuran Kova)", 390m, 1, -5 },
                    { -24, "2 Adet Poppy Sandviç + Nuggets (4'lü) + Soğan Halkası (4'lü) + Tenders (2'li) + Patates Kızartması (Orta) +1L. İçecek", "https://images.deliveryhero.io/image/fd-tr/Products/28499446.jpg??width=500", true, "2 Kişilik Fırsat Menüsü", 310m, 1, -5 },
                    { -23, "Popeyes XL Sandviç + Acılı Kanat (2'li) + Nuggets (4'lü) + Patates Kızartması (Orta) + Kutu İçecek", "https://images.deliveryhero.io/image/fd-tr/Products/28499489.jpg??width=500", true, "Maxi XL Menü", 290m, 1, -5 },
                    { -22, "2 Adet Tavukburger® + Patates Kızartması (Orta) + Seçeceğiniz 2 Adet Sos + İçecek (1 L.)", "https://images.deliveryhero.io/image/fd-tr/Products/62936401.jpg??width=500", true, "Yemeksepeti Özel Ekonomix Menü", 210m, 1, -5 },
                    { -21, "2 Adet Popchicken Sandviç + Patates Kızartması (Orta) + Nuggets (4'lü) + Soğan Halkası (4'lü) + 1L. İçecek\r\n\r\n", "https://images.deliveryhero.io/image/fd-tr/Products/45539560.jpg??width=500", true, "Yemeksepeti Özel 2 Kişilik Ekonomik Menü", 280m, 1, -5 },
                    { -20, "Pizza sosu, mozzarella peyniri, sucuk, salam, mantar, mısır, zeytin, kapya biberi, jalapeno biberi, çarliston biber\r\n\r\n", "https://images.deliveryhero.io/image/fd-tr/Products/37824098.jpg??width=500", true, "Karışık Pizza (Küçük)", 149m, 1, -4 },
                    { -19, "Karışık Pizza (Orta) + Patates Kızartması + Kutu İçecek", "https://images.deliveryhero.io/image/fd-tr/Products/37824095.jpg??width=500", true, "Orta Boy Menü", 275m, 1, -4 },
                    { -18, "Parmak dilim patatas kızartması", "https://images.deliveryhero.io/image/fd-tr/Products/37824124.jpg??width=500", true, "Patates Kızartması", 65m, 1, -4 },
                    { -17, "Pizza sosu, mozzarella peyniri, sucuk, salam, mantar, mısır, zeytin, kapya biberi, jalapeno biberi, çarliston biber", "https://images.deliveryhero.io/image/fd-tr/Products/37824108.jpg??width=500", true, "Karışık Pizza (Büyük)", 275m, 1, -4 },
                    { -16, "Pizza sosu, mozzarella peyniri, sucuk, salam, mantar, mısır, zeytin, kapya biberi, jalapeno biberi, çarliston biber", "https://images.deliveryhero.io/image/fd-tr/Products/37824103.jpg??width=500", true, "Karışık Pizza (Orta)", 190m, 1, -4 },
                    { -15, "Big King® + Big King® + Patates Kızartması (Orta) + Coca-Cola (1 L.)", "https://images.deliveryhero.io/image/fd-tr/Products/27388683.jpg??width=500", true, "Coca-Cola Fırsatı (2'li Big King® Fırsatı)", 380m, 1, -3 },
                    { -14, "Big King® + Patates Kızartması (Orta) + Kutu İçecek", "https://images.deliveryhero.io/image/fd-tr/Products/27388722.jpg??width=500", true, "Big King® Menü", 230m, 1, -3 },
                    { -13, "Whopper® + Patates Kızartması (Orta) + Kutu İçecek", "https://images.deliveryhero.io/image/fd-tr/Products/27388717.jpg??width=500", true, "Whopper® Menü", 235m, 1, -3 },
                    { -12, "Big King® + King Chicken® + Patates Kızartması (Orta) + 1 L. İçecek", "https://images.deliveryhero.io/image/fd-tr/Products/27388696.jpg??width=500", true, "Kral İkili", 280m, 1, -3 },
                    { -11, "2 Adet Whopper Jr.® + Patates Kızartması (Orta) + Soğan Halkası (4'lü) + 1 L. İçecek", "https://images.deliveryhero.io/image/fd-tr/Products/44479596.jpg??width=500", true, "Yemeksepeti Özel 2’li Whopper Jr.® Kampanyası", 220m, 1, -3 },
                    { -10, "7 adet soğan halkası", "https://images.deliveryhero.io/image/fd-tr/Products/7506896.jpg??width=500", true, "Soğan Halkası (7 Adet)", 45m, 1, -2 },
                    { -9, "Ev yapımı hamburger ekmeği, ev yapımı 2 adet 70 gr. hamburger köftesi, özel sos, double cheddar peyniri, dana jambon, eritilmiş kaşar peyniri. Patates kızartması ile", "https://images.deliveryhero.io/image/fd-tr/Products/29087186.jpg??width=500", true, "Big Boss Burger", 299.90m, 1, -2 },
                    { -8, "Ev yapımı hamburger ekmeği, ev yapımı hamburger köftesi, Musqa sos, cheddar peyniri, soğan, turşu, domates, yeşillik. Patates kızartması ile", "https://images.deliveryhero.io/image/fd-tr/Products/26725995.jpg??width=500", true, "Musqa Rich Burger", 263.90m, 1, -2 },
                    { -7, "Ev yapımı hamburger ekmeği, ev yapımı tavuk burger köftesi, özel sos, mayonez, domates, yeşillik. Patates kızartması ile", "https://images.deliveryhero.io/image/fd-tr/Products/26727171.jpg??width=500", true, "Musqa Chicken Burger", 200.90m, 1, -2 },
                    { -6, "Ev yapımı hamburger ekmeği, ev yapımı hamburger köftesi, Musqa sos, domates, yeşillik, turşu. Patates kızartması ile", "https://images.deliveryhero.io/image/fd-tr/Products/26725904.jpg??width=500", true, "Musqa Burger", 249.90m, 1, -2 },
                    { -5, "Özel fırın lavaşa; 50 gr. tavuk döner, patates kızartması, salatalık turşusu, özel sos, isteğe göre soğan, sarımsaklı mayonez sos. Cin biberi ile", "https://images.deliveryhero.io/image/fd-tr/Products/10775606.jpg??width=500", true, "Tavuk Döner Small Dürüm", 125m, 1, -1 },
                    { -4, "Large Tavuk Döner Dürüm (45 cm.) + Ayran (27 cl.)", "https://images.deliveryhero.io/image/fd-tr/Products/10775572.jpg??width=500", true, "Tavuk Döner Large Dürüm Menü", 210m, 1, -1 },
                    { -3, "Dilimlenmiş patateslerin kızartılarak koni şeklinde kartonda servis edildiği bir atıştırmalıktır.", "https://images.deliveryhero.io/image/fd-tr/Products/10775630.jpg??width=500", true, "Külah Patates Kızartması", 50m, 1, -1 },
                    { -2, "2 adet özel fırın lavaşa; 125 gr. tavuk döner, patates kızartması, salatalık turşusu, özel sos, isteğe göre soğan, sarımsaklı mayonez sos. Cin biberi ile", "https://images.deliveryhero.io/image/fd-tr/Products/10775609.jpg??width=500", true, "Tavuk Döner XLarge Dürüm (45 cm.)", 205m, 1, -1 },
                    { -1, "Özel fırın lavaşa; 75 gr. tavuk döner, salatalık turşusu, patates kızartması, özel sos, isteğe göre soğan, sarımsaklı mayonez sos. Cin biberi ile", "https://images.deliveryhero.io/image/fd-tr/tr-logos/cp9bu-logo.jpg", true, "Tavuk Döner Medium Dürüm", 150m, 1, -1 }
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

            migrationBuilder.CreateIndex(
                name: "IX_CategoryMeal_MealsId",
                table: "CategoryMeal",
                column: "MealsId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryRestaurant_RestaurantsId",
                table: "CategoryRestaurant",
                column: "RestaurantsId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRestaurant_RestaurantsId",
                table: "CustomerRestaurant",
                column: "RestaurantsId");

            migrationBuilder.CreateIndex(
                name: "IX_MealOrder_OrdersId",
                table: "MealOrder",
                column: "OrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_Meals_RestaurantId",
                table: "Meals",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryPersonelId",
                table: "Orders",
                column: "DeliveryPersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RestaurantId",
                table: "Orders",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantReviews_CustomerId",
                table: "RestaurantReviews",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantReviews_RestaurantId",
                table: "RestaurantReviews",
                column: "RestaurantId");
        }

        /// <inheritdoc />
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
                name: "CategoryMeal");

            migrationBuilder.DropTable(
                name: "CategoryRestaurant");

            migrationBuilder.DropTable(
                name: "CustomerRestaurant");

            migrationBuilder.DropTable(
                name: "MealOrder");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "RestaurantReviews");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "DeliveryPersonnel");

            migrationBuilder.DropTable(
                name: "Restaurants");
        }
    }
}
