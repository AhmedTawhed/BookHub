using BookHub.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookHub.Infrastructure.Data.Seeding
{
        public static class DataSeed
        {
            public static void Seed(ModelBuilder builder)
            {
            var fixedDate = new DateTime(2025, 12, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Fiction" },
                new Category { Id = 2, Name = "Science" },
                new Category { Id = 3, Name = "Fantasy" },
                new Category { Id = 4, Name = "Thriller" },
                new Category { Id = 5, Name = "Romance" },
                new Category { Id = 6, Name = "History" },
                new Category { Id = 7, Name = "Technology" },
                new Category { Id = 8, Name = "Self-Help" }
            );

            builder.Entity<Book>().HasData(
                 new Book { Id = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", CategoryId = 1, CreatedAt = fixedDate, Description = "A classic novel of the Jazz Age exploring themes of wealth and society." },
                new Book { Id = 2, Title = "1984", Author = "George Orwell", CategoryId = 1, CreatedAt = fixedDate, Description = "Dystopian novel about surveillance, totalitarianism, and loss of freedom." },
                new Book { Id = 3, Title = "Brief Answers to the Big Questions", Author = "Stephen Hawking", CategoryId = 2, CreatedAt = fixedDate, Description = "Stephen Hawking's thoughts on the universe, science, and humanity's future." },
                new Book { Id = 4, Title = "A Short History of Nearly Everything", Author = "Bill Bryson", CategoryId = 2, CreatedAt = fixedDate, Description = "A journey through science and history explaining how we understand the world." },
                new Book { Id = 5, Title = "Harry Potter and the Sorcerer's Stone", Author = "J.K. Rowling", CategoryId = 3, CreatedAt = fixedDate, Description = "The first adventure of Harry Potter in the magical world of Hogwarts." },
                new Book { Id = 6, Title = "The Hobbit", Author = "J.R.R. Tolkien", CategoryId = 3, CreatedAt = fixedDate, Description = "Bilbo Baggins embarks on an epic quest with dwarves to reclaim their homeland." },
                new Book { Id = 7, Title = "The Girl with the Dragon Tattoo", Author = "Stieg Larsson", CategoryId = 4, CreatedAt = fixedDate, Description = "A thriller involving mystery, corporate corruption, and a hacker detective." },
                new Book { Id = 8, Title = "Gone Girl", Author = "Gillian Flynn", CategoryId = 4, CreatedAt = fixedDate, Description = "A suspenseful story of a marriage gone terribly wrong with shocking twists." },
                new Book { Id = 9, Title = "Clean Code", Author = "Robert C. Martin", CategoryId = 7, CreatedAt = fixedDate, Description = "Guidelines and principles for writing maintainable and readable software code." },
                new Book { Id = 10, Title = "Atomic Habits", Author = "James Clear", CategoryId = 8, CreatedAt = fixedDate, Description = "A practical guide on building good habits and breaking bad ones." }
            );
        }
    }
}