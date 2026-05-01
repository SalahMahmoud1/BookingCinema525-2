using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingCinema525.Migrations
{
    /// <inheritdoc />
    public partial class AddActor2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Actors (Name, Mobile, Address, Email, Img) VALUES('Omar Sharif', '+20-123-456-7890', 'Cairo, Egypt', 'omar.sharif@example.com', '1.png'),('Adel Emam', '+20-987-654-3210', 'Giza, Egypt', 'adel.emam@example.com', '2.png'),('Ahmed Zaki', '+20-555-111-2222', 'Heliopolis, Cairo, Egypt', 'ahmed.zaki@example.com', '3.png'),('Rami Malek', '+1-555-123-4567', 'Los Angeles, USA', 'rami.malek@example.com', '4.png'),('Ahmed El Sakka', '+20-444-333-2222', 'Dokki, Cairo, Egypt', 'ahmed.elsakka@example.com', '5.png'),('Ahmed Helmy', '+20-222-333-4444', 'Nasr City, Cairo, Egypt', 'ahmed.helmy@example.com', '6.png'),('Mohamed Ramadan', '+20-777-888-9999', 'Cairo, Egypt', 'mohamed.ramadan@example.com', '7.png'),('Youssef El Sherif', '+20-666-555-4444', 'Alexandria, Egypt', 'youssef.elsherif@example.com', '8.png'),('Hind Rostom', '+20-111-222-3333', 'Cairo, Egypt', 'hind.rostom@example.com', '9.png'),('Faten Hamama', '+20-999-888-7777', 'Cairo, Egypt', 'faten.hamama@example.com', '10.png');");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete From Actors");
        }
    }
}
