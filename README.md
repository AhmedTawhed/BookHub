# 📚 BookHub API

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core%209-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Azure](https://img.shields.io/badge/Azure%20App%20Service-0089D6?style=for-the-badge&logo=microsoftazure&logoColor=white)
![GitHub Actions](https://img.shields.io/badge/GitHub%20Actions-2088FF?style=for-the-badge&logo=githubactions&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Status](https://img.shields.io/badge/Status-Live-success?style=for-the-badge)

**BookHub** is a clean and scalable **ASP.NET Core 9 Web API** for managing books, categories, user favorites, and reviews. It is architected using **Clean Architecture** principles and implements the **Repository & Unit-of-Work** patterns to ensure maintainability and testability.

This project serves as a **portfolio piece** to demonstrate modern backend development skills, focusing on security, database design, and containerization.

---

## ⚡ TL;DR (Why It’s Impressive)
- ✅ **Clean Architecture + Repository/Unit-of-Work** → maintainable & testable  
- ✅ **JWT Auth & Role-Based Access (Admin/User)** → production-grade security  
- ✅ **CI/CD Pipeline with GitHub Actions** → automated build, test & deploy  
- ✅ **Azure Deployment + Docker containerization** → cloud-ready & portable  
- ✅ **Full-featured API** → CRUD, Favorites, Reviews, Pagination, Sorting, Filtering  
- ✅ **xUnit + Moq testing** → business logic validated in isolation  
- ✅ **Demo Ready** → pre-seeded data & instant Swagger access  

---

## 🚀 Live Demo
The API is fully deployed and production-ready:
- **Swagger UI:** [https://bookhub-api.azurewebsites.net/swagger/index.html](https://bookhub-api.azurewebsites.net/swagger/index.html)
- **API Status:** ✅ Operational

---

## 🏗 Architecture & Tech Stack

### 🏗 Architecture
- **Clean Architecture:** Strict separation of concerns across Api, Core, and Infrastructure layers
- **Design Patterns:** Repository Pattern & Unit of Work for a clean and decoupled data access layer

### 🛠 Tech Stack
- **Framework:** ASP.NET Core 9 (Web API)  
- **ORM:** Entity Framework Core  
- **Database:** Microsoft SQL Server  
- **Testing:** xUnit, Moq, FluentAssertions  
- **Cloud & DevOps:** Azure App Service, GitHub Actions, Docker

---

## ✨ Key Features

### 🔒 Security
- **JWT Authentication:** Secure token-based access with ASP.NET Core Identity.
- **Role-Based Access (RBAC):** Strict permissions (Admin vs User).
- **Validation:** Secure password management and robust data validation.

### 📦 Core Modules & Logic
- **Admin Controls:** Full CRUD for Books & Categories.
- **User Engagement:** Personal Favorites & Reviews management.
- **Data Handling:** Efficient Pagination, Sorting, and Filtering.
- **Resilience:** Global exception handling and strict data annotations.

---

## 🧪 Testing Strategy
To ensure code reliability, the project includes a dedicated testing suite:
- **Unit Testing:** Focused on validating business logic within the service layer
- **Featured Tests:** Comprehensive test cases for Book Service, ensuring correct handling of book operations
- **Isolation:** Dependencies are mocked using Moq to ensure services are tested in isolation

---

## ☁️ DevOps & Deployment
This project demonstrates a full modern development lifecycle:
- **CI/CD:** Fully automated via GitHub Actions.
- **Cloud Hosting:** Deployed on Azure App Service (Linux).
- **Containerization:** Docker multi-stage build for optimal performance.

---

## 🗄️ Database & Demo Access

### 🔑 Demo Credentials (Admin):
- **Email:** `admin@bookhub.com`
- **Password:** `Admin@123`

> **Note:** To test as a standard user, simply Register a new account to access Features like Favorites and Reviews.

### ⚙️ Role Logic:
- **Admin Assignment:** The first registered user automatically becomes the Admin. All subsequent registrations are assigned the User role.
- **Pre-seeded Data:** Roles, Categories, and sample Books are seeded automatically during database initialization.

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

## ⚡ Quick Start (Local)
### 1. Clone the repo:
``` bash
git clone https://github.com/AhmedTawhed/BookHub
```
### 2. Apply migrations:
``` bash
dotnet ef database update
```
### 3. Run the API:
``` bash
dotnet run
```

---

## 📮 Postman Collection

- A complete collection is available in: `Solution Items/BookHub API.postman_collection.json`
- Includes authentication, admin/user endpoints, pagination, sorting, and filtering examples.

---

## 🤝 Contributing
- Pull requests are welcome.
- Open an issue for suggestions or improvements.
