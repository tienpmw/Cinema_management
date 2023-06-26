using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
	public class CinemaContext : DbContext
	{

		public CinemaContext()
		{
		}

		public CinemaContext(DbContextOptions options) : base(options)
		{
		}

		public virtual DbSet<Role> Role { get; set; } = null!;
		public virtual DbSet<User> User { get; set; } = null!;
		public virtual DbSet<RefreshToken> RefreshToken { get; set; } = null!;
		public virtual DbSet<Booking> Booking { get; set; } = null!;
		public virtual DbSet<Show> Show { get; set; } = null!;
		public virtual DbSet<Room> Room { get; set; } = null!;
		public virtual DbSet<Film> Film { get; set; } = null!;
		public virtual DbSet<Country> Country { get; set; } = null!;
		public virtual DbSet<Genre> Genre { get; set; } = null!;

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//var directory = Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().LastIndexOf("\\")) + "/CinemaWebAPI";
			//var conf = new ConfigurationBuilder()
			//	.SetBasePath(directory)
			//	.AddJsonFile("appsettings.json", true, true)
			//	.Build();
			//if (!optionsBuilder.IsConfigured)
			//{
			optionsBuilder.UseSqlServer("server=localhost; database=CinemaManagement; uid=sa; pwd=sa");
			//optionsBuilder.UseSqlServer(conf.GetConnectionString("DB"));
			//}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Role>().HasData(new Role[]
			{
				new Role{RoleId = 1, RoleName="Admin"},
				new Role{RoleId = 2, RoleName="User"}
			});
			modelBuilder.Entity<Country>().HasData(_country);
		}

		private readonly Country[] _country = new Country[]
		{
			new Country { CountryName = "Afghanistan", CountryCode = "AF" },
			new Country { CountryName = "Åland Islands", CountryCode = "AX" },
			new Country { CountryName = "Albania", CountryCode = "AL" },
  new Country { CountryName = "Algeria", CountryCode = "DZ" },
  new Country { CountryName = "American Samoa", CountryCode = "AS" },
  new Country { CountryName = "AndorrA", CountryCode = "AD" },
  new Country { CountryName = "Angola", CountryCode = "AO" },
  new Country { CountryName = "Anguilla", CountryCode = "AI" },
  new Country { CountryName = "Antarctica", CountryCode = "AQ" },
  new Country { CountryName = "Antigua and Barbuda", CountryCode = "AG" },
  new Country { CountryName = "Argentina", CountryCode = "AR" },
  new Country { CountryName = "Armenia", CountryCode = "AM" },
  new Country { CountryName = "Aruba", CountryCode = "AW" },
  new Country { CountryName = "Australia", CountryCode = "AU" },
  new Country { CountryName = "Austria", CountryCode = "AT" },
  new Country { CountryName = "Azerbaijan", CountryCode = "AZ" },
  new Country { CountryName = "Bahamas", CountryCode = "BS" },
  new Country { CountryName = "Bahrain", CountryCode = "BH" },
  new Country { CountryName = "Bangladesh", CountryCode = "BD" },
  new Country { CountryName = "Barbados", CountryCode = "BB" },
  new Country { CountryName = "Belarus", CountryCode = "BY" },
  new Country { CountryName = "Belgium", CountryCode = "BE" },
  new Country { CountryName = "Belize", CountryCode = "BZ" },
  new Country { CountryName = "Benin", CountryCode = "BJ" },
  new Country { CountryName = "Bermuda", CountryCode = "BM" },
  new Country { CountryName = "Bhutan", CountryCode = "BT" },
  new Country { CountryName = "Bolivia", CountryCode = "BO" },
  new Country { CountryName = "Bosnia and Herzegovina", CountryCode = "BA" },
  new Country { CountryName = "Botswana", CountryCode = "BW" },
  new Country { CountryName = "Bouvet Island", CountryCode = "BV" },
  new Country { CountryName = "Brazil", CountryCode = "BR" },
  new Country { CountryName = "British Indian Ocean Territory", CountryCode = "IO" },
  new Country { CountryName = "Brunei Darussalam", CountryCode = "BN" },
  new Country { CountryName = "Bulgaria", CountryCode = "BG" },
  new Country { CountryName = "Burkina Faso", CountryCode = "BF" },
  new Country { CountryName = "Burundi", CountryCode = "BI" },
  new Country { CountryName = "Cambodia", CountryCode = "KH" },
  new Country { CountryName = "Cameroon", CountryCode = "CM" },
  new Country { CountryName = "Canada", CountryCode = "CA" },
  new Country { CountryName = "Cape Verde", CountryCode = "CV" },
  new Country { CountryName = "Cayman Islands", CountryCode = "KY" },
  new Country { CountryName = "Central African Republic", CountryCode = "CF" },
  new Country { CountryName = "Chad", CountryCode = "TD" },
  new Country { CountryName = "Chile", CountryCode = "CL" },
  new Country { CountryName = "China", CountryCode = "CN" },
  new Country { CountryName = "Christmas Island", CountryCode = "CX" },
  new Country { CountryName = "Cocos (Keeling) Islands", CountryCode = "CC" },
  new Country { CountryName = "Colombia", CountryCode = "CO" },
  new Country { CountryName = "Comoros", CountryCode = "KM" },
  new Country { CountryName = "Congo", CountryCode = "CG" },
  new Country { CountryName = "Congo, The Democratic Republic of the", CountryCode = "CD" },
  new Country { CountryName = "Cook Islands", CountryCode = "CK" },
  new Country { CountryName = "Costa Rica", CountryCode = "CR" },
  new Country { CountryName = "Cote D\"Ivoire", CountryCode = "CI" },
  new Country { CountryName = "Croatia", CountryCode = "HR" },
  new Country { CountryName = "Cuba", CountryCode = "CU" },
  new Country { CountryName = "Cyprus", CountryCode = "CY" },
  new Country { CountryName = "Czech Republic", CountryCode = "CZ" },
  new Country { CountryName = "Denmark", CountryCode = "DK" },
  new Country { CountryName = "Djibouti", CountryCode = "DJ" },
  new Country { CountryName = "Dominica", CountryCode = "DM" },
  new Country { CountryName = "Dominican Republic", CountryCode = "DO" },
  new Country { CountryName = "Ecuador", CountryCode = "EC" },
  new Country { CountryName = "Egypt", CountryCode = "EG" },
  new Country { CountryName = "El Salvador", CountryCode = "SV" },
  new Country { CountryName = "Equatorial Guinea", CountryCode = "GQ" },
  new Country { CountryName = "Eritrea", CountryCode = "ER" },
  new Country { CountryName = "Estonia", CountryCode = "EE" },
  new Country { CountryName = "Ethiopia", CountryCode = "ET" },
  new Country { CountryName = "Falkland Islands (Malvinas)", CountryCode = "FK" },
  new Country { CountryName = "Faroe Islands", CountryCode = "FO" },
  new Country { CountryName = "Fiji", CountryCode = "FJ" },
  new Country { CountryName = "Finland", CountryCode = "FI" },
  new Country { CountryName = "France", CountryCode = "FR" },
  new Country { CountryName = "French Guiana", CountryCode = "GF" },
  new Country { CountryName = "French Polynesia", CountryCode = "PF" },
  new Country { CountryName = "French Southern Territories", CountryCode = "TF" },
  new Country { CountryName = "Gabon", CountryCode = "GA" },
  new Country { CountryName = "Gambia", CountryCode = "GM" },
  new Country { CountryName = "Georgia", CountryCode = "GE" },
  new Country { CountryName = "Germany", CountryCode = "DE" },
  new Country { CountryName = "Ghana", CountryCode = "GH" },
  new Country { CountryName = "Gibraltar", CountryCode = "GI" },
  new Country { CountryName = "Greece", CountryCode = "GR" },
  new Country { CountryName = "Greenland", CountryCode = "GL" },
  new Country { CountryName = "Grenada", CountryCode = "GD" },
  new Country { CountryName = "Guadeloupe", CountryCode = "GP" },
  new Country { CountryName = "Guam", CountryCode = "GU" },
  new Country { CountryName = "Guatemala", CountryCode = "GT" },
  new Country { CountryName = "Guernsey", CountryCode = "GG" },
  new Country { CountryName = "Guinea", CountryCode = "GN" },
  new Country { CountryName = "Guinea-Bissau", CountryCode = "GW" },
  new Country { CountryName = "Guyana", CountryCode = "GY" },
  new Country { CountryName = "Haiti", CountryCode = "HT" },
  new Country { CountryName = "Heard Island and Mcdonald Islands", CountryCode = "HM" },
  new Country { CountryName = "Holy See (Vatican City State)", CountryCode = "VA" },
  new Country { CountryName = "Honduras", CountryCode = "HN" },
  new Country { CountryName = "Hong Kong", CountryCode = "HK" },
  new Country { CountryName = "Hungary", CountryCode = "HU" },
  new Country { CountryName = "Iceland", CountryCode = "IS" },
  new Country { CountryName = "India", CountryCode = "IN" },
  new Country { CountryName = "Indonesia", CountryCode = "ID" },
  new Country { CountryName = "Iran, Islamic Republic Of", CountryCode = "IR" },
  new Country { CountryName = "Iraq", CountryCode = "IQ" },
  new Country { CountryName = "Ireland", CountryCode = "IE" },
  new Country { CountryName = "Isle of Man", CountryCode = "IM" },
  new Country { CountryName = "Israel", CountryCode = "IL" },
  new Country { CountryName = "Italy", CountryCode = "IT" },
  new Country { CountryName = "Jamaica", CountryCode = "JM" },
  new Country { CountryName = "Japan", CountryCode = "JP" },
  new Country { CountryName = "Jersey", CountryCode = "JE" },
  new Country { CountryName = "Jordan", CountryCode = "JO" },
  new Country { CountryName = "Kazakhstan", CountryCode = "KZ" },
  new Country { CountryName = "Kenya", CountryCode = "KE" },
  new Country { CountryName = "Kiribati", CountryCode = "KI" },
  new Country { CountryName = "Korea, Democratic People\"S Republic of", CountryCode = "KP" },
  new Country { CountryName = "Korea, Republic of", CountryCode = "KR" },
  new Country { CountryName = "Kuwait", CountryCode = "KW" },
  new Country { CountryName = "Kyrgyzstan", CountryCode = "KG" },
  new Country { CountryName = "Lao People\"S Democratic Republic", CountryCode = "LA" },
  new Country { CountryName = "Latvia", CountryCode = "LV" },
  new Country { CountryName = "Lebanon", CountryCode = "LB" },
  new Country { CountryName = "Lesotho", CountryCode = "LS" },
  new Country { CountryName = "Liberia", CountryCode = "LR" },
  new Country { CountryName = "Libyan Arab Jamahiriya", CountryCode = "LY" },
  new Country { CountryName = "Liechtenstein", CountryCode = "LI" },
  new Country { CountryName = "Lithuania", CountryCode = "LT" },
  new Country { CountryName = "Luxembourg", CountryCode = "LU" },
  new Country { CountryName = "Macao", CountryCode = "MO" },
  new Country { CountryName = "Macedonia, The Former Yugoslav Republic of", CountryCode = "MK" },
  new Country { CountryName = "Madagascar", CountryCode = "MG" },
  new Country { CountryName = "Malawi", CountryCode = "MW" },
  new Country { CountryName = "Malaysia", CountryCode = "MY" },
  new Country { CountryName = "Maldives", CountryCode = "MV" },
  new Country { CountryName = "Mali", CountryCode = "ML" },
  new Country { CountryName = "Malta", CountryCode = "MT" },
  new Country { CountryName = "Marshall Islands", CountryCode = "MH" },
  new Country { CountryName = "Martinique", CountryCode = "MQ" },
  new Country { CountryName = "Mauritania", CountryCode = "MR" },
  new Country { CountryName = "Mauritius", CountryCode = "MU" },
  new Country { CountryName = "Mayotte", CountryCode = "YT" },
  new Country { CountryName = "Mexico", CountryCode = "MX" },
  new Country { CountryName = "Micronesia, Federated States of", CountryCode = "FM" },
  new Country { CountryName = "Moldova, Republic of", CountryCode = "MD" },
  new Country { CountryName = "Monaco", CountryCode = "MC" },
  new Country { CountryName = "Mongolia", CountryCode = "MN" },
  new Country { CountryName = "Montserrat", CountryCode = "MS" },
  new Country { CountryName = "Morocco", CountryCode = "MA" },
  new Country { CountryName = "Mozambique", CountryCode = "MZ" },
  new Country { CountryName = "Myanmar", CountryCode = "MM" },
  new Country { CountryName = "Namibia", CountryCode = "NA" },
  new Country { CountryName = "Nauru", CountryCode = "NR" },
  new Country { CountryName = "Nepal", CountryCode = "NP" },
  new Country { CountryName = "Netherlands", CountryCode = "NL" },
  new Country { CountryName = "Netherlands Antilles", CountryCode = "AN" },
  new Country { CountryName = "New Caledonia", CountryCode = "NC" },
  new Country { CountryName = "New Zealand", CountryCode = "NZ" },
  new Country { CountryName = "Nicaragua", CountryCode = "NI" },
  new Country { CountryName = "Niger", CountryCode = "NE" },
  new Country { CountryName = "Nigeria", CountryCode = "NG" },
  new Country { CountryName = "Niue", CountryCode = "NU" },
  new Country { CountryName = "Norfolk Island", CountryCode = "NF" },
  new Country { CountryName = "Northern Mariana Islands", CountryCode = "MP" },
  new Country { CountryName = "Norway", CountryCode = "NO" },
  new Country { CountryName = "Oman", CountryCode = "OM" },
  new Country { CountryName = "Pakistan", CountryCode = "PK" },
  new Country { CountryName = "Palau", CountryCode = "PW" },
  new Country { CountryName = "Palestinian Territory, Occupied", CountryCode = "PS" },
  new Country { CountryName = "Panama", CountryCode = "PA" },
  new Country { CountryName = "Papua New Guinea", CountryCode = "PG" },
  new Country { CountryName = "Paraguay", CountryCode = "PY" },
  new Country { CountryName = "Peru", CountryCode = "PE" },
  new Country { CountryName = "Philippines", CountryCode = "PH" },
  new Country { CountryName = "Pitcairn", CountryCode = "PN" },
  new Country { CountryName = "Poland", CountryCode = "PL" },
  new Country { CountryName = "Portugal", CountryCode = "PT" },
  new Country { CountryName = "Puerto Rico", CountryCode = "PR" },
  new Country { CountryName = "Qatar", CountryCode = "QA" },
  new Country { CountryName = "Reunion", CountryCode = "RE" },
  new Country { CountryName = "Romania", CountryCode = "RO" },
  new Country { CountryName = "Russian Federation", CountryCode = "RU" },
  new Country { CountryName = "RWANDA", CountryCode = "RW" },
  new Country { CountryName = "Saint Helena", CountryCode = "SH" },
  new Country { CountryName = "Saint Kitts and Nevis", CountryCode = "KN" },
  new Country { CountryName = "Saint Lucia", CountryCode = "LC" },
  new Country { CountryName = "Saint Pierre and Miquelon", CountryCode = "PM" },
  new Country { CountryName = "Saint Vincent and the Grenadines", CountryCode = "VC" },
  new Country { CountryName = "Samoa", CountryCode = "WS" },
  new Country { CountryName = "San Marino", CountryCode = "SM" },
  new Country { CountryName = "Sao Tome and Principe", CountryCode = "ST" },
  new Country { CountryName = "Saudi Arabia", CountryCode = "SA" },
  new Country { CountryName = "Senegal", CountryCode = "SN" },
  new Country { CountryName = "Serbia and Montenegro", CountryCode = "CS" },
  new Country { CountryName = "Seychelles", CountryCode = "SC" },
  new Country { CountryName = "Sierra Leone", CountryCode = "SL" },
  new Country { CountryName = "Singapore", CountryCode = "SG" },
  new Country { CountryName = "Slovakia", CountryCode = "SK" },
  new Country { CountryName = "Slovenia", CountryCode = "SI" },
  new Country { CountryName = "Solomon Islands", CountryCode = "SB" },
  new Country { CountryName = "Somalia", CountryCode = "SO" },
  new Country { CountryName = "South Africa", CountryCode = "ZA" },
  new Country { CountryName = "South Georgia and the South Sandwich Islands", CountryCode = "GS" },
  new Country { CountryName = "Spain", CountryCode = "ES" },
  new Country { CountryName = "Sri Lanka", CountryCode = "LK" },
  new Country { CountryName = "Sudan", CountryCode = "SD" },
  new Country { CountryName = "SuriCountryName", CountryCode = "SR" },
  new Country { CountryName = "Svalbard and Jan Mayen", CountryCode = "SJ" },
  new Country { CountryName = "Swaziland", CountryCode = "SZ" },
  new Country { CountryName = "Sweden", CountryCode = "SE" },
  new Country { CountryName = "Switzerland", CountryCode = "CH" },
  new Country { CountryName = "Syrian Arab Republic", CountryCode = "SY" },
  new Country { CountryName = "Taiwan, Province of China", CountryCode = "TW" },
  new Country { CountryName = "Tajikistan", CountryCode = "TJ" },
  new Country { CountryName = "Tanzania, United Republic of", CountryCode = "TZ" },
  new Country { CountryName = "Thailand", CountryCode = "TH" },
  new Country { CountryName = "Timor-Leste", CountryCode = "TL" },
  new Country { CountryName = "Togo", CountryCode = "TG" },
  new Country { CountryName = "Tokelau", CountryCode = "TK" },
  new Country { CountryName = "Tonga", CountryCode = "TO" },
  new Country { CountryName = "Trinidad and Tobago", CountryCode = "TT" },
  new Country { CountryName = "Tunisia", CountryCode = "TN" },
  new Country { CountryName = "Turkey", CountryCode = "TR" },
  new Country { CountryName = "Turkmenistan", CountryCode = "TM" },
  new Country { CountryName = "Turks and Caicos Islands", CountryCode = "TC" },
  new Country { CountryName = "Tuvalu", CountryCode = "TV" },
  new Country { CountryName = "Uganda", CountryCode = "UG" },
  new Country { CountryName = "Ukraine", CountryCode = "UA" },
  new Country { CountryName = "United Arab Emirates", CountryCode = "AE" },
  new Country { CountryName = "United Kingdom", CountryCode = "GB" },
  new Country { CountryName = "United States", CountryCode = "US" },
  new Country { CountryName = "United States Minor Outlying Islands", CountryCode = "UM" },
  new Country { CountryName = "Uruguay", CountryCode = "UY" },
  new Country { CountryName = "Uzbekistan", CountryCode = "UZ" },
  new Country { CountryName = "Vanuatu", CountryCode = "VU" },
  new Country { CountryName = "Venezuela", CountryCode = "VE" },
  new Country { CountryName = "Viet Nam", CountryCode = "VN" },
  new Country { CountryName = "Virgin Islands, British", CountryCode = "VG" },
  new Country { CountryName = "Virgin Islands, U.S.", CountryCode = "VI" },
  new Country { CountryName = "Wallis and Futuna", CountryCode = "WF" },
  new Country { CountryName = "Western Sahara", CountryCode = "EH" },
  new Country { CountryName = "Yemen", CountryCode = "YE" },
  new Country { CountryName = "Zambia", CountryCode = "ZM" },
  new Country { CountryName = "Zimbabwe", CountryCode = "ZW" }
		};
	}
}
