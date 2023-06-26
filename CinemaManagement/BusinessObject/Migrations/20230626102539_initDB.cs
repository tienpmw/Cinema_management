using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    public partial class initDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    CountryCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryCode);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    GenreId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenreName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.GenreId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    RoomId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberRow = table.Column<int>(type: "int", nullable: false),
                    NumberColumn = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.RoomId);
                });

            migrationBuilder.CreateTable(
                name: "Film",
                columns: table => new
                {
                    FilmId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenreId = table.Column<long>(type: "bigint", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(5)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilmDuration = table.Column<long>(type: "bigint", nullable: false),
                    DateRelease = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Film", x => x.FilmId);
                    table.ForeignKey(
                        name: "FK_Film_Country_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Country",
                        principalColumn: "CountryCode");
                    table.ForeignKey(
                        name: "FK_Film_Genre_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genre",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsConfirmEmail = table.Column<bool>(type: "bit", nullable: false),
                    IsLoginGoogle = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Show",
                columns: table => new
                {
                    ShowId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<long>(type: "bigint", nullable: false),
                    FilmId = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SeatStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Show", x => x.ShowId);
                    table.ForeignKey(
                        name: "FK_Show_Film_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Film",
                        principalColumn: "FilmId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Show_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    BookingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShowId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    SeatBooking = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DateBooking = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContentBill = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPay = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Booking_Show_ShowId",
                        column: x => x.ShowId,
                        principalTable: "Show",
                        principalColumn: "ShowId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Booking_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "CountryCode", "CountryName" },
                values: new object[,]
                {
                    { "AD", "AndorrA" },
                    { "AE", "United Arab Emirates" },
                    { "AF", "Afghanistan" },
                    { "AG", "Antigua and Barbuda" },
                    { "AI", "Anguilla" },
                    { "AL", "Albania" },
                    { "AM", "Armenia" },
                    { "AN", "Netherlands Antilles" },
                    { "AO", "Angola" },
                    { "AQ", "Antarctica" },
                    { "AR", "Argentina" },
                    { "AS", "American Samoa" },
                    { "AT", "Austria" },
                    { "AU", "Australia" },
                    { "AW", "Aruba" },
                    { "AX", "Åland Islands" },
                    { "AZ", "Azerbaijan" },
                    { "BA", "Bosnia and Herzegovina" },
                    { "BB", "Barbados" },
                    { "BD", "Bangladesh" },
                    { "BE", "Belgium" },
                    { "BF", "Burkina Faso" },
                    { "BG", "Bulgaria" },
                    { "BH", "Bahrain" },
                    { "BI", "Burundi" },
                    { "BJ", "Benin" },
                    { "BM", "Bermuda" },
                    { "BN", "Brunei Darussalam" },
                    { "BO", "Bolivia" },
                    { "BR", "Brazil" },
                    { "BS", "Bahamas" },
                    { "BT", "Bhutan" },
                    { "BV", "Bouvet Island" },
                    { "BW", "Botswana" },
                    { "BY", "Belarus" },
                    { "BZ", "Belize" },
                    { "CA", "Canada" },
                    { "CC", "Cocos (Keeling) Islands" },
                    { "CD", "Congo, The Democratic Republic of the" },
                    { "CF", "Central African Republic" },
                    { "CG", "Congo" },
                    { "CH", "Switzerland" }
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "CountryCode", "CountryName" },
                values: new object[,]
                {
                    { "CI", "Cote D\"Ivoire" },
                    { "CK", "Cook Islands" },
                    { "CL", "Chile" },
                    { "CM", "Cameroon" },
                    { "CN", "China" },
                    { "CO", "Colombia" },
                    { "CR", "Costa Rica" },
                    { "CS", "Serbia and Montenegro" },
                    { "CU", "Cuba" },
                    { "CV", "Cape Verde" },
                    { "CX", "Christmas Island" },
                    { "CY", "Cyprus" },
                    { "CZ", "Czech Republic" },
                    { "DE", "Germany" },
                    { "DJ", "Djibouti" },
                    { "DK", "Denmark" },
                    { "DM", "Dominica" },
                    { "DO", "Dominican Republic" },
                    { "DZ", "Algeria" },
                    { "EC", "Ecuador" },
                    { "EE", "Estonia" },
                    { "EG", "Egypt" },
                    { "EH", "Western Sahara" },
                    { "ER", "Eritrea" },
                    { "ES", "Spain" },
                    { "ET", "Ethiopia" },
                    { "FI", "Finland" },
                    { "FJ", "Fiji" },
                    { "FK", "Falkland Islands (Malvinas)" },
                    { "FM", "Micronesia, Federated States of" },
                    { "FO", "Faroe Islands" },
                    { "FR", "France" },
                    { "GA", "Gabon" },
                    { "GB", "United Kingdom" },
                    { "GD", "Grenada" },
                    { "GE", "Georgia" },
                    { "GF", "French Guiana" },
                    { "GG", "Guernsey" },
                    { "GH", "Ghana" },
                    { "GI", "Gibraltar" },
                    { "GL", "Greenland" },
                    { "GM", "Gambia" }
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "CountryCode", "CountryName" },
                values: new object[,]
                {
                    { "GN", "Guinea" },
                    { "GP", "Guadeloupe" },
                    { "GQ", "Equatorial Guinea" },
                    { "GR", "Greece" },
                    { "GS", "South Georgia and the South Sandwich Islands" },
                    { "GT", "Guatemala" },
                    { "GU", "Guam" },
                    { "GW", "Guinea-Bissau" },
                    { "GY", "Guyana" },
                    { "HK", "Hong Kong" },
                    { "HM", "Heard Island and Mcdonald Islands" },
                    { "HN", "Honduras" },
                    { "HR", "Croatia" },
                    { "HT", "Haiti" },
                    { "HU", "Hungary" },
                    { "ID", "Indonesia" },
                    { "IE", "Ireland" },
                    { "IL", "Israel" },
                    { "IM", "Isle of Man" },
                    { "IN", "India" },
                    { "IO", "British Indian Ocean Territory" },
                    { "IQ", "Iraq" },
                    { "IR", "Iran, Islamic Republic Of" },
                    { "IS", "Iceland" },
                    { "IT", "Italy" },
                    { "JE", "Jersey" },
                    { "JM", "Jamaica" },
                    { "JO", "Jordan" },
                    { "JP", "Japan" },
                    { "KE", "Kenya" },
                    { "KG", "Kyrgyzstan" },
                    { "KH", "Cambodia" },
                    { "KI", "Kiribati" },
                    { "KM", "Comoros" },
                    { "KN", "Saint Kitts and Nevis" },
                    { "KP", "Korea, Democratic People\"S Republic of" },
                    { "KR", "Korea, Republic of" },
                    { "KW", "Kuwait" },
                    { "KY", "Cayman Islands" },
                    { "KZ", "Kazakhstan" },
                    { "LA", "Lao People\"S Democratic Republic" },
                    { "LB", "Lebanon" }
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "CountryCode", "CountryName" },
                values: new object[,]
                {
                    { "LC", "Saint Lucia" },
                    { "LI", "Liechtenstein" },
                    { "LK", "Sri Lanka" },
                    { "LR", "Liberia" },
                    { "LS", "Lesotho" },
                    { "LT", "Lithuania" },
                    { "LU", "Luxembourg" },
                    { "LV", "Latvia" },
                    { "LY", "Libyan Arab Jamahiriya" },
                    { "MA", "Morocco" },
                    { "MC", "Monaco" },
                    { "MD", "Moldova, Republic of" },
                    { "MG", "Madagascar" },
                    { "MH", "Marshall Islands" },
                    { "MK", "Macedonia, The Former Yugoslav Republic of" },
                    { "ML", "Mali" },
                    { "MM", "Myanmar" },
                    { "MN", "Mongolia" },
                    { "MO", "Macao" },
                    { "MP", "Northern Mariana Islands" },
                    { "MQ", "Martinique" },
                    { "MR", "Mauritania" },
                    { "MS", "Montserrat" },
                    { "MT", "Malta" },
                    { "MU", "Mauritius" },
                    { "MV", "Maldives" },
                    { "MW", "Malawi" },
                    { "MX", "Mexico" },
                    { "MY", "Malaysia" },
                    { "MZ", "Mozambique" },
                    { "NA", "Namibia" },
                    { "NC", "New Caledonia" },
                    { "NE", "Niger" },
                    { "NF", "Norfolk Island" },
                    { "NG", "Nigeria" },
                    { "NI", "Nicaragua" },
                    { "NL", "Netherlands" },
                    { "NO", "Norway" },
                    { "NP", "Nepal" },
                    { "NR", "Nauru" },
                    { "NU", "Niue" },
                    { "NZ", "New Zealand" }
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "CountryCode", "CountryName" },
                values: new object[,]
                {
                    { "OM", "Oman" },
                    { "PA", "Panama" },
                    { "PE", "Peru" },
                    { "PF", "French Polynesia" },
                    { "PG", "Papua New Guinea" },
                    { "PH", "Philippines" },
                    { "PK", "Pakistan" },
                    { "PL", "Poland" },
                    { "PM", "Saint Pierre and Miquelon" },
                    { "PN", "Pitcairn" },
                    { "PR", "Puerto Rico" },
                    { "PS", "Palestinian Territory, Occupied" },
                    { "PT", "Portugal" },
                    { "PW", "Palau" },
                    { "PY", "Paraguay" },
                    { "QA", "Qatar" },
                    { "RE", "Reunion" },
                    { "RO", "Romania" },
                    { "RU", "Russian Federation" },
                    { "RW", "RWANDA" },
                    { "SA", "Saudi Arabia" },
                    { "SB", "Solomon Islands" },
                    { "SC", "Seychelles" },
                    { "SD", "Sudan" },
                    { "SE", "Sweden" },
                    { "SG", "Singapore" },
                    { "SH", "Saint Helena" },
                    { "SI", "Slovenia" },
                    { "SJ", "Svalbard and Jan Mayen" },
                    { "SK", "Slovakia" },
                    { "SL", "Sierra Leone" },
                    { "SM", "San Marino" },
                    { "SN", "Senegal" },
                    { "SO", "Somalia" },
                    { "SR", "SuriCountryName" },
                    { "ST", "Sao Tome and Principe" },
                    { "SV", "El Salvador" },
                    { "SY", "Syrian Arab Republic" },
                    { "SZ", "Swaziland" },
                    { "TC", "Turks and Caicos Islands" },
                    { "TD", "Chad" },
                    { "TF", "French Southern Territories" }
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "CountryCode", "CountryName" },
                values: new object[,]
                {
                    { "TG", "Togo" },
                    { "TH", "Thailand" },
                    { "TJ", "Tajikistan" },
                    { "TK", "Tokelau" },
                    { "TL", "Timor-Leste" },
                    { "TM", "Turkmenistan" },
                    { "TN", "Tunisia" },
                    { "TO", "Tonga" },
                    { "TR", "Turkey" },
                    { "TT", "Trinidad and Tobago" },
                    { "TV", "Tuvalu" },
                    { "TW", "Taiwan, Province of China" },
                    { "TZ", "Tanzania, United Republic of" },
                    { "UA", "Ukraine" },
                    { "UG", "Uganda" },
                    { "UM", "United States Minor Outlying Islands" },
                    { "US", "United States" },
                    { "UY", "Uruguay" },
                    { "UZ", "Uzbekistan" },
                    { "VA", "Holy See (Vatican City State)" },
                    { "VC", "Saint Vincent and the Grenadines" },
                    { "VE", "Venezuela" },
                    { "VG", "Virgin Islands, British" },
                    { "VI", "Virgin Islands, U.S." },
                    { "VN", "Viet Nam" },
                    { "VU", "Vanuatu" },
                    { "WF", "Wallis and Futuna" },
                    { "WS", "Samoa" },
                    { "YE", "Yemen" },
                    { "YT", "Mayotte" },
                    { "ZA", "South Africa" },
                    { "ZM", "Zambia" },
                    { "ZW", "Zimbabwe" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { 1L, "Admin" },
                    { 2L, "User" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ShowId",
                table: "Booking",
                column: "ShowId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_UserId",
                table: "Booking",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Film_CountryCode",
                table: "Film",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Film_GenreId",
                table: "Film",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Show_FilmId",
                table: "Show",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_Show_RoomId",
                table: "Show",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Show");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Film");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Genre");
        }
    }
}
