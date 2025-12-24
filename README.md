# 📚 BookHub API

BookHub is a clean and scalable **ASP.NET Core 9 Web API** for managing books, categories, user favorites, and reviews. It is architected using Clean Architecture principles and implements the Repository & Unit-of-Work patterns to ensure maintainability and testability.

This project serves as a **portfolio piece** to demonstrate modern backend development skills, focusing on security, database design, and containerization.

---

## ✨ Key Features

### 🔒 Authentication & Authorization
- **Identity Integration:** Built using ASP.NET Core Identity for secure user management.
- **JWT Support:** Secure token-based authentication for all RESTful endpoints.
- **Role-Based Access (RBAC):** Distinct permissions for Admin and User roles.

### 📦 Core Modules & Logic
- **Books & Categories:** Full CRUD operations (Restricted to Admin).
- **User Engagement:** Users can manage their Favorite Books and write Reviews.
- **Clean Engineering:** Implementation of Repository & Unit-of-Work patterns.
- **Robustness:** Global Exception Handling and strict Data Annotations validation.

### 🔍 Query Features
- **Pagination:** Efficient data fetching for large book collections.
- **Sorting & Filtering:** Dynamic query capabilities for better user experience.

---

## 🗂 Tech Stack
### 🛠 Core Technologies
- **Framework:** ASP.NET Core 9 (Web API)
- **ORM:** Entity Framework Core
- **Database:** Microsoft SQL Server
- **Containerization:** Docker (Multi-stage builds)

### 🏗 Architecture & Patterns
- **Clean Architecture:** Separation of Concerns (Api, Core, Infrastructure)
- **Design Patterns:** Repository Pattern & Unit of Work
- **Security:** ASP.NET Core Identity + JWT Authentication

### 🧪 Tools & Testing
- **API Documentation:** Swagger / OpenAPI
- **Manual Testing:** Postman (Collection included in Solution Items/)

---

## 🐳 Run with Docker
This project is containerized using a multi-stage build for optimal performance.
### 1. Build the image:
``` bash
docker build -t bookhub-api .
```
### 2. Run the container:
``` bash
docker run -d -p 8080:8080 --name bookhub-container bookhub-api
```
(Note: Ensure your connection string in appsettings.json is updated to reach your SQL Server instance from the container).

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

### To simplify testing for recruiters and developers, the system handles roles dynamically:

- **Pre-seeded Data:** Roles, Categories and sample Books are seeded automatically via OnModelCreating.
- **Smart Admin Assignment:** The First User who registers via /api/account/register is automatically granted the Admin role.
	- All subsequent registrations are assigned the User role.
- **Goal:** This logic allows you to test Admin-only features immediately after your first registration without manual DB edits.

---

## 📮 Postman Collection

- A complete collection covering all endpoints: Solution Items/BookHub API.postman_collection.json
- Includes authentication, admin/user endpoints, pagination, sorting, and filtering examples
  
Import into Postman to test all endpoints.

---

## 🤝 Contributing
- Pull requests are welcome.
- Open an issue for suggestions or improvements.
