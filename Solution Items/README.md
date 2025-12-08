📚 BookHub API

BookHub is a scalable ASP.NET Core 9 Web API for managing books, categories, user favorites, and reviews. It includes authentication, authorization, admin features, and RESTful endpoints built with EF Core, SQL Server, Identity, and JWT Authentication. This project is designed as a portfolio piece to showcase practical backend skills.

🚀 Features
👤 Authentication & Authorization

Register and login using JWT

ASP.NET Core Identity integration

Pre-seeded roles: Admin & User

Role-based authorization on endpoints

📘 Core Modules

Books (Admin: Add, Edit, Delete)

Categories (Admin: Add, Edit, Delete)

Favorite Books (Users: Add, Edit, Delete)

Reviews (Users: Add, Edit)

Global exception handling

Validation using DataAnnotations

Repository & Unit-of-Work pattern

🔍 Query Features

Pagination

Sorting

Filtering

🗂 Tech Stack

ASP.NET Core 9 / C#

Entity Framework Core

SQL Server

Identity & JWT Authentication

Repository & Unit-of-Work Pattern

Postman Collection for testing

🗄️ Database Setup
Apply EF Core Migrations
dotnet ef database update

Data Seeding (Automatic)

Books & Categories → seeded inside OnModelCreating

Roles (Admin, User) → seeded via IdentitySeed.Seed() at startup

Default Admin User

Email: admin@bookhub.com

Password: Admin@123!

All data is seeded automatically. No SQL scripts required.

👤 Creating a Normal User
POST /api/account/register


Then log in:

POST /api/account/login


The new user automatically receives the User role.

📮 Postman Collection

Available here:

Solution Items/BookHub API.postman_collection.json


Includes:

Authentication requests

Admin endpoints

User endpoints

Pagination & sorting examples

Import into Postman to test all endpoints.

🤝 Contributing

Pull requests are welcome.

Open an issue for suggestions or improvements.