# 📚 BookHub API

![CI/CD Status](https://github.com/AhmedTawhed/BookHub/actions/workflows/deploy.yml/badge.svg)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core%209-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Azure](https://img.shields.io/badge/Azure%20App%20Service-0089D6?style=for-the-badge&logo=microsoftazure&logoColor=white)
![GitHub Actions](https://img.shields.io/badge/GitHub%20Actions-2088FF?style=for-the-badge&logo=githubactions&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Status](https://img.shields.io/badge/Status-Live-success?style=for-the-badge)

**BookHub** is a clean and scalable **ASP.NET Core 9 Web API** for managing books, categories, user favorites, and reviews. Built with **Clean Architecture** and **Cloud-Agnostic** principles, ensuring seamless deployment across **Azure**, **AWS**, or **Docker-based** environments.

This project serves as a **portfolio piece** to demonstrate modern backend development skills, focusing on security, database design, and automated CI/CD pipelines.

---

## ⚡ TL;DR (Why It’s Impressive)
- ✅ **Clean Architecture + Repository/Unit-of-Work** → maintainable & testable  
- ✅ **JWT Auth & Role-Based Access (Admin/User)** → production-grade security  
- ✅ **CI/CD Pipeline with GitHub Actions** → automated build, test & deploy  
- ✅ **Azure Deployment + Docker containerization** → Architected for Azure App Service & Container Registry.
- ✅ **Full-featured API** → CRUD, Favorites, Reviews, Pagination, Sorting, Filtering  
- ✅ **xUnit + Moq testing** → business logic validated in isolation  
- ✅ **Structured Logging & Observability** → Integrated Serilog for production-grade monitoring and file-based auditing.
- ✅ **Demo Ready** → pre-seeded data & instant Swagger access  

---

## 🚀 Live Demo
The API is containerized and currently hosted on **Render** for the live demonstration:
- **Swagger UI:** [https://bookhub-9x8b.onrender.com/swagger/index.html](https://bookhub-9x8b.onrender.com/swagger/index.html)
- **API Status:** ✅ Operational

---

## 🏗 Architecture & Tech Stack

### 🏗 Architecture
- **Clean Architecture:** Strict separation of concerns across Api, Core, and Infrastructure layers.
- **Design Patterns:** Repository Pattern & Unit of Work for a clean and decoupled data access layer.

### 🛠 Tech Stack
- **Framework:** ASP.NET Core 9 (Web API)  
- **ORM:** Entity Framework Core  
- **Database:** Microsoft SQL Server (Hosted on MonsterASP)  
- **Testing:** xUnit, Moq, FluentAssertions
- **Cloud & DevOps:** Azure App Service (Target), Render (Live Demo), GitHub Actions, Docker
- **Logging: Serilog** (Console, File sinks) with Structured Logging

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

### 🛡️ Resilience & Monitoring
- **Global Exception Handling:** Centralized middleware that catches all unhandled exceptions, ensuring a consistent `ApiResponse<T>` format and preventing sensitive data leaks.
- **Structured Logging with Serilog:** High-performance logging to both **Console** and **Rolling Files**, providing full visibility into the system's behavior.
- **Request Monitoring:** Automatic logging of HTTP request details (Path, Method, Response Time, Status Code) for performance auditing.
- **Security Auditing:** Detailed logging for critical actions (Login successes/failures, Administrative changes) to ensure traceability.

---

## 🧪 Testing Strategy
To ensure code reliability, the project includes a dedicated testing suite:
- **Unit Testing:** Focused on validating business logic within the service layer.
- **Featured Tests:** Comprehensive test cases for Book Service, ensuring correct handling of book operations.
- **Isolation:** Dependencies are mocked using Moq to ensure services are tested in isolation.

---

## ☁️ DevOps & Deployment
This project demonstrates a full modern development lifecycle:
- **CI/CD:** Fully automated via GitHub Actions. On every push to `master`, the pipeline runs tests and triggers deployment.
- **Cloud Strategy:** Originally designed for **Microsoft Azure**; currently utilizing **Render** for live demo hosting to showcase manual environment configuration and external DB connectivity.
- **Containerization:** **Docker** multi-stage build. This ensures the app is portable and can be deployed to **Azure Web Apps for Containers** or **Azure Kubernetes Service (AKS)** with zero code changes.

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