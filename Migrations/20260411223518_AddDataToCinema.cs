using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingCinema525.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToCinema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Cinemas (Name, Description, Img, Status) VALUES('Cinema Radio', 'Historic cinema in downtown Cairo, famous for classic Egyptian films.', 'Cinema1.png', 1),('Odeon Cinema', 'Popular Cairo cinema known for screening both Egyptian and international movies.', 'Cinema2.png', 0),('Renaissance Cinema', 'Modern multiplex chain in Egypt offering the latest releases.', 'Cinema3.png', 1),('Galaxy Cinema', 'Large cinema complex in Cairo with multiple halls and IMAX screens.', 'Cinema4.png', 1),('Cairo Opera House Cinema', 'Cultural venue occasionally used for film screenings and festivals.', 'Cinema5.png', 0),('Zawya Cinema', 'Independent cinema in Cairo focusing on art-house and alternative films.', 'Cinema6.png', 1),('Miami Cinema', 'One of Cairo’s oldest cinemas, located on Talaat Harb Street.', 'Cinema7.png', 0),('Metro Cinema', 'Historic cinema in downtown Cairo, part of Egypt’s golden film era.', 'Cinema8.png', 0),('Rivoli Cinema', 'Classic cinema hall in Cairo, now used for special screenings.', 'Cinema9.png', 1),('Cineplex City Stars', 'Modern multiplex inside City Stars Mall, showing international blockbusters.', 'Cinema10.png', 1);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete From Cinemas");
        }
    }
}
