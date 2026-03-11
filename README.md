# 📚 BookHub API

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core%209-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Azure](https://img.shields.io/badge/Azure%20App%20Service-0089D6?style=for-the-badge&logo=microsoftazure&logoColor=white)
![GitHub Actions](https://img.shields.io/badge/GitHub%20Actions-2088FF?style=for-the-badge&logo=githubactions&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Status](https://img.shields.io/badge/Status-Live-success?style=for-the-badge)
![CI/CD Status](https://github.com/AhmedTawhed/BookHub/actions/workflows/deploy.yml/badge.svg)


**BookHub** is a clean and scalable **ASP.NET Core 9 Web API** for managing books, categories, user favorites, and reviews. Built with **Clean Architecture** and **Cloud-Agnostic** principles, ensuring seamless deployment across **Azure**, **AWS**, or **Docker-based** environments.

This project serves as a **portfolio piece** to demonstrate modern backend development skills, focusing on security, database design, and automated CI/CD pipelines.

---

## ⚡ TL;DR (Why It's Impressive)
- ✅ **Clean Architecture + Repository/Unit-of-Work** → maintainable & testable  
- ✅ **JWT Auth & Role-Based Access (Admin/User)** → production-grade security  
- ✅ **CI/CD Pipeline with GitHub Actions** → automated build, test & deploy  
- ✅ **Azure Deployment + Docker containerization** → Architected for Azure App Service & Container Registry  
- ✅ **Full-featured API** → CRUD, Favorites, Reviews, Pagination, Sorting, Filtering  
- ✅ **In-Memory Caching** → cache-aside pattern for books and categories  
- ✅ **Rate Limiting** → fixed window limiting on auth and general endpoints  
- ✅ **FluentValidation** → request validation on all DTOs  
- ✅ **Health Checks** → database connectivity monitoring at `/health`  
- ✅ **46 Unit Tests** → full service layer coverage with xUnit & Moq  
- ✅ **Structured Logging & Observability** → Serilog for production-grade monitoring  
- ✅ **Demo Ready** → pre-seeded data & instant Swagger access
- ✅ **Full-Stack Project** → [Angular 21 frontend](https://github.com/AhmedTawhed/BookHub-UI) with [Live Demo](https://cosmic-daffodil-7b3d0b.netlify.app) (signals, standalone components, lazy loading)

---

## 🚀 Live Demo
The API is containerized and currently hosted on **Render** with a fully functional Angular 21 frontend:

| | Link |
|---|---|
| 🌐 **Frontend** | [cosmic-daffodil-7b3d0b.netlify.app](https://cosmic-daffodil-7b3d0b.netlify.app) |
| 📖 **Backend Swagger UI** | [bookhub-9x8b.onrender.com/swagger](https://bookhub-9x8b.onrender.com/swagger/index.html) |
| 💻 **Frontend Repo** | [github.com/AhmedTawhed/BookHub-UI](https://github.com/AhmedTawhed/BookHub-UI) |

> ⚠️ Render free tier spins down after inactivity — first request may take 30–60s.

---

## 🏗 Architecture

```
BookHub/
├── BookHub.Api/             # Controllers, Middleware, Program.cs
├── BookHub.Core/            # Entities, DTOs, Interfaces, Validators
├── BookHub.Infrastructure/  # EF Core, Repositories, Unit of Work
└── BookHub.Tests/           # xUnit unit tests
```

- **Clean Architecture:** Strict separation of concerns across Api, Core, and Infrastructure layers.
- **Design Patterns:** Repository Pattern & Unit of Work for a clean and decoupled data access layer.

---

## 🛠 Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 9 (Web API) |
| ORM | Entity Framework Core |
| Database | Microsoft SQL Server (MonsterASP) |
| Validation | FluentValidation |
| Caching | IMemoryCache (cache-aside pattern) |
| Testing | xUnit, Moq, FluentAssertions |
| Cloud & DevOps | Azure App Service, Render, GitHub Actions, Docker |
| Logging | Serilog (Console + Rolling File sinks) |

---

## ✨ Key Features

### 🔒 Security
- **JWT Authentication:** Secure token-based access with ASP.NET Core Identity.
- **Role-Based Access (RBAC):** Strict permissions (Admin vs User).
- **Rate Limiting:** Fixed window rate limiting — 5 requests/min on auth endpoints, 60 requests/min on general endpoints.
- **FluentValidation:** Server-side validation on all incoming DTOs.

### 📦 Core Modules & Logic
- **Admin Controls:** Full CRUD for Books & Categories (Admin only).
- **User Engagement:** Personal Favorites & Reviews management.
- **User Profile:** Get and update profile — username, email, and password.
- **Data Handling:** Efficient Pagination, Sorting, and Filtering.
- **Caching:** Cache-aside pattern with IMemoryCache — books and categories cached for 10 minutes, invalidated on write.
- **Resilience:** Global exception handling and strict data validation.

### 🛡️ Resilience & Monitoring
- **Global Exception Handling:** Centralized middleware that catches all unhandled exceptions, ensuring a consistent ApiResponse<T> format and preventing sensitive data leaks.
- **Health Checks:** Database connectivity exposed at `/health`.
- **Structured Logging with Serilog:** High-performance logging to both Console and Rolling Files, providing full visibility into the system's behavior.
- **Request Monitoring:** Automatic logging of HTTP request details (Path, Method, Response Time, Status Code) for performance auditing.
- **Security Auditing:** Detailed logging for critical actions (Login successes/failures, Administrative changes) to ensure traceability.

---

## 📮 API Endpoints

### 🔑 Auth — `/api/account`
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| POST | `/register` | Public | Register a new user |
| POST | `/login` | Public | Login and receive JWT token |
| GET | `/profile` | Authorized | Get current user profile |
| PUT | `/profile` | Authorized | Update username, email, or password |

### 📚 Books — `/api/book`
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| GET | `/all` | Public | Get all books with ratings |
| GET | `/paged` | Public | Get books with pagination, sorting, filtering |
| GET | `/{id}` | Public | Get book by ID |
| POST | `/` | Admin | Add a new book |
| PUT | `/{id}` | Admin | Update a book |
| DELETE | `/{id}` | Admin | Delete a book |

### 🗂 Categories — `/api/category`
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| GET | `/all` | Public | Get all categories |
| GET | `/paged` | Public | Get categories with pagination |
| GET | `/{id}` | Public | Get category by ID |
| POST | `/` | Admin | Add a new category |
| PUT | `/{id}` | Admin | Update a category |
| DELETE | `/{id}` | Admin | Delete a category |

### ⭐ Reviews — `/api/review`
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| GET | `/book/{bookId}` | Public | Get all reviews for a book |
| GET | `/{id}` | Public | Get review by ID |
| GET | `/user` | User | Get current user's reviews |
| POST | `/` | User | Add a review |
| PUT | `/{reviewId}` | User | Update own review |
| DELETE | `/{reviewId}` | User | Delete own review |

### ❤️ Favorites — `/api/favoritebook`
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| GET | `/` | User | Get current user's favorite books |
| POST | `/{bookId}` | User | Add book to favorites |
| DELETE | `/{bookId}` | User | Remove book from favorites |

---

## 🧪 Testing Strategy
- **46 Unit Tests** covering the full service layer.
- **Services covered:** BookService, CategoryService, ReviewService, FavoriteBookService.
- **Isolation:** All dependencies mocked with Moq.
- **Assertions:** FluentAssertions for readable test output.

---

## ☁️ DevOps & Deployment
This project demonstrates a full modern development lifecycle:
- **CI/CD:** Fully automated via GitHub Actions. On every push to `master`, the pipeline runs tests and triggers deployment.
- **Cloud Strategy:** Architected for **Azure App Service** & **Azure Container Registry**; currently deployed on **Render** for live demo hosting.
- **Containerization:** **Docker** multi-stage build. This ensures the app is portable and can be deployed to **Azure Web Apps for Containers**, **Azure Kubernetes Service (AKS)** or any container host with zero code changes.

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