using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingCinema525.Migrations
{
    /// <inheritdoc />
    public partial class AddDatToCategryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Categories (Name, Description, Status) VALUES('Drama', 'Films focusing on emotional storytelling and character development.', 1),('Comedy', 'Lighthearted films designed to entertain and amuse audiences.', 1),('Action', 'High-energy films with stunts, fights, and fast-paced sequences.', 1),('Romance', 'Films centered on love stories and emotional relationships.', 1),('Thriller', 'Suspenseful films that keep audiences on edge.', 1),('Horror', 'Films designed to evoke fear and tension.', 1),('Documentary', 'Non-fiction films exploring real events, people, or issues.', 1),('Historical', 'Films depicting past events and cultural heritage.', 1),('Musical', 'Films combining storytelling with songs and dance.', 0),('Animation', 'Films created using animated techniques, appealing to all ages.', 1);");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete From Categories");
        }
    }
}
