📚 BookHub API

BookHub is a clean and scalable **ASP.NET Core 9 Web API** for managing books, categories, user favorites, and reviews.  
It includes authentication, authorization, admin features, and a set of RESTful endpoints built using **EF Core**, **SQL Server**, **Identity**, and **JWT Authentication**.  

This project is designed as a **portfolio piece** to demonstrate practical backend skills.

---

## ✨ Key Features

### 🔒 Authentication & Authorization
- Register and login with **JWT**
- **ASP.NET Core Identity** integration
- Pre-seeded roles: **Admin & User**
- Role-based authorization on endpoints

### 📦 Core Modules
- **Books** (Admin: Add, Edit, Delete)
- **Categories** (Admin: Add, Edit, Delete)
- **Favorite Books** (Users: Add, Edit, Delete)
- **Reviews** (Users: Add, Edit)
- Global exception handling
- Validation using **DataAnnotations**
- **Clean Architecture Principles** (implementing Repository & Unit-of-Work pattern)

### 🔍 Query Features
- Pagination
- Sorting
- Filtering

---

## 🗂 Tech Stack
- **Backend:** ASP.NET Core 9, C#
- **Database:** SQL Server, EF Core
- **Authentication:** Identity + JWT
- **Testing:** Postman Collection included

---

## ⚡ Quick Start
### 1. Clone the repo:
``` bash
git clone https://github.com/AhmedTawhed/BookHub
cd BookHub
```
### 2. Apply EF Core migrations:
``` bash
dotnet ef database update
```
### 3. Run the API:
``` bash
dotnet run
```
### 4. Test endpoints via Postman:
Solution Items/BookHub API.postman_collection.json

---
## 🗄️ Database & Users

### Automatic Data Seeding:
- Books & Categories → seeded inside OnModelCreating
- Roles (Admin, User) → seeded via IdentitySeed.Seed() at startup

### Default Admin
- Email: admin@bookhub.com
- Password: Admin@123!
  <br>
All seeding is handled automatically. No SQL scripts needed.
### Normal User:
- Register: POST /api/account/register
- Login: POST /api/account/login → auto-assigned User role

---

## 📮 Postman Collection

- A complete collection covering all endpoints: Solution Items/BookHub API.postman_collection.json
- Includes authentication, admin/user endpoints, pagination, sorting, and filtering examples
  
Import into Postman to test all endpoints.

---

## 🤝 Contributing
- Pull requests are welcome.
- Open an issue for suggestions or improvements.
