using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingCinema525.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToMovieTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Movies (Name, Description, Price, MainImg, DateTime, Status, CategoryId, CinemaId, ActorId) VALUES('Lawrence of Arabia', 'Epic film featuring Omar Sharif in a breakthrough role.', 120.00, 'lawrence.png', '1962-12-10', 1, 8, 1, 1),('Morgan Ahmed Morgan', 'Comedy starring Adel Emam about a businessman entering university.', 90.00, 'morgan.png', '2007-06-15', 1, 2, 2, 2),('Days of El-Sadat', 'Biographical film starring Ahmed Zaki as President Sadat.', 100.00, 'sadat.png', '2001-07-01', 1, 9, 3, 3),('Bohemian Rhapsody', 'Oscar-winning film starring Rami Malek as Freddie Mercury.', 150.00, 'bohemian.png', '2018-11-02', 1, 7, 4, 4),('Mafia', 'Action film starring Ahmed El Sakka.', 80.00, 'mafia.png', '2002-08-01', 1, 3, 5, 5),('Zaky Chan', 'Comedy starring Ahmed Helmy as a bodyguard.', 70.00, 'zakychan.png', '2005-06-01', 1, 2, 6, 6),('El-Ostoura', 'TV drama starring Mohamed Ramadan.', 60.00, 'ostoura.png', '2016-01-01', 1, 1, 7, 7),('The End', 'Sci-fi drama starring Youssef El Sherif.', 95.00, 'theend.png', '2020-04-01', 1, 5, 8, 8),('Cairo Station', 'Classic Egyptian film starring Hind Rostom.', 85.00, 'cairostation.png', '1958-03-01', 1, 9, 9, 9),('The Nightingale''s Prayer', 'Drama starring Faten Hamama.', 90.00, 'nightingale.png', '1959-01-01', 1, 1, 10, 10),('The Island', 'Action film starring Ahmed El Sakka.', 100.00, 'island.png', '2007-07-01', 1, 3, 11, 5),('Nasser 56', 'Ahmed Zaki portrays President Nasser during the Suez Canal nationalization.', 95.00, 'nasser56.png', '1996-07-01', 1, 9, 1, 3),('Made in Egypt', 'Comedy starring Ahmed Helmy about a body swap between man and toy.', 80.00, 'madeinegypt.png', '2014-06-01', 1, 2, 2, 6),('Number One', 'Musical drama starring Mohamed Ramadan.', 70.00, 'numberone.png', '2018-01-01', 1, 10, 3, 7),('Kafr Delhab', 'Horror-drama starring Youssef El Sherif.', 85.00, 'kafrdelhab.png', '2017-01-01', 1, 6, 4, 8),('The Flirtation of Girls', 'Classic Egyptian comedy starring Hind Rostom.', 75.00, 'flirtation.png', '1949-01-01', 1, 2, 5, 9),('The Thin Thread', 'Drama starring Faten Hamama.', 90.00, 'thinthread.png', '1971-01-01', 1, 1, 6, 10),('Morgan Returns', 'Sequel comedy starring Adel Emam.', 95.00, 'morganreturns.png', '2010-01-01', 1, 2, 7, 2),('Doctor Zhivago', 'International classic featuring Omar Sharif.', 150.00, 'zhivago.png', '1965-12-01', 1, 9, 8, 1),('El Gezira 2', 'Sequel to The Island starring Ahmed El Sakka.', 110.00, 'gezira2.png', '2014-10-01', 1, 3, 9, 5),('Mr. Robot', 'TV series starring Rami Malek as Elliot Alderson.', 120.00, 'mrrobot.png', '2015-06-01', 1, 5, 10, 4),('The Terrorism and the Kebab', 'Political comedy starring Adel Emam.', 100.00, 'terrorism.png', '1992-01-01', 1, 2, 11, 2);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete From Movies");
        }
    }
}
