using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable


namespace BookHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedBooksAndCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAtUtc",
                table: "Books",
                newName: "CreatedAt");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Fiction" },
                    { 2, "Science" },
                    { 3, "Fantasy" },
                    { 4, "Thriller" },
                    { 5, "Romance" },
                    { 6, "History" },
                    { 7, "Technology" },
                    { 8, "Self-Help" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CategoryId", "CreatedAt", "Description", "Title" },
                values: new object[,]
                {
                    { 1, "F. Scott Fitzgerald", 1, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A classic novel of the Jazz Age exploring themes of wealth and society.", "The Great Gatsby" },
                    { 2, "George Orwell", 1, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dystopian novel about surveillance, totalitarianism, and loss of freedom.", "1984" },
                    { 3, "Stephen Hawking", 2, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Stephen Hawking's thoughts on the universe, science, and humanity's future.", "Brief Answers to the Big Questions" },
                    { 4, "Bill Bryson", 2, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A journey through science and history explaining how we understand the world.", "A Short History of Nearly Everything" },
                    { 5, "J.K. Rowling", 3, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The first adventure of Harry Potter in the magical world of Hogwarts.", "Harry Potter and the Sorcerer's Stone" },
                    { 6, "J.R.R. Tolkien", 3, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bilbo Baggins embarks on an epic quest with dwarves to reclaim their homeland.", "The Hobbit" },
                    { 7, "Stieg Larsson", 4, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A thriller involving mystery, corporate corruption, and a hacker detective.", "The Girl with the Dragon Tattoo" },
                    { 8, "Gillian Flynn", 4, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A suspenseful story of a marriage gone terribly wrong with shocking twists.", "Gone Girl" },
                    { 9, "Robert C. Martin", 7, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Guidelines and principles for writing maintainable and readable software code.", "Clean Code" },
                    { 10, "James Clear", 8, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A practical guide on building good habits and breaking bad ones.", "Atomic Habits" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Books",
                newName: "CreatedAtUtc");
        }
    }
}
