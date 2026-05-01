using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingCinema525.Migrations
{
    /// <inheritdoc />
    public partial class fillActorMovieList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO ActorMovieLists (ActorId, MovieID) VALUES(1, 1), (1, 19),  (2, 2), (2, 18), (2, 22),  (3, 3),   (3, 12), (4, 4),  (4, 21), (5, 5), (5, 11),  (5, 20),  (6, 6), (6, 13), (7, 7), (7, 14),  (8, 8),  (8, 15), (9, 9), (9, 16), (10, 10), (10, 17);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete From ActorMovieLists");
        }
    }
}
