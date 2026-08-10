# CRN Product API

## Project Overview

CRN Product API is a RESTful backend API developed using .NET 8 and ASP.NET Core Web API.

The application provides product CRUD operations along with JWT-based authentication and refresh token functionality.

The project follows a layered architecture to improve maintainability, separation of concerns, and scalability.


## Key Features

- User registration and login
- JWT-based authentication
- Refresh token mechanism
- Product CRUD operations
- Pagination for product collection endpoints
- Entity Framework Core with SQL Server
- Repository and Service layer pattern
- FluentValidation for request validation
- Global exception handling middleware
- Structured logging using Serilog
- Swagger/OpenAPI documentation
- Response compression
- Security headers middleware
- SQL Server indexing
- Docker and Docker Compose support
- Unit testing using xUnit and Moq
- API integration testing using WebApplicationFactory


## Architecture

The application follows a layered architecture with clear separation of responsibilities.

```text
CRNProductAPI
│
├── src
│   │
│   ├── API
│   │   ├── Controllers
│   │   │   ├── AuthController.cs
│   │   │   └── ProductsController.cs
│   │   │
│   │   ├── Middleware
│   │   │   ├── ExceptionMiddleware.cs
│   │   │   └── SecurityHeadersMiddleware.cs
│   │   │
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   ├── Application
│   │   ├── DTOs
│   │   ├── Interfaces
│   │   ├── Mapping
│   │   ├── Services
│   │   └── Validators
│   │
│   ├── Domain
│   │   └── Entities
│   │       ├── Product.cs
│   │       └── User.cs
│   │
│   └── Infrastructure
│       ├── Data
│       │   ├── ApplicationDbContext.cs
│       │   └── Repositories
│       │       ├── ProductRepository.cs
│       │       └── UserRepository.cs
│       │
│       ├── Identity
│       │   ├── JwtTokenGenerator.cs
│       │   └── PasswordHasher.cs
│       │
│       └── Migrations
│
├── tests
│   └── CRNProductAPI.Tests
│       ├── ApiIntegrationTests.cs
│       ├── AuthServiceTests.cs
│       └── ProductServiceTests.cs
│
├── docker-compose.yml
├── CRNProductAPI.sln
└── README.md

## Tech Stack

- **Framework:** .NET 8 / ASP.NET Core Web API
- **Language:** C#
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **Authentication:** JWT with Refresh Tokens
- **Validation:** FluentValidation
- **Mapping:** AutoMapper
- **Logging:** Serilog
- **API Documentation:** Swagger / OpenAPI
- **Containerization:** Docker / Docker Compose
- **Testing:** xUnit, Moq, and WebApplicationFactory


## API Endpoints

### Authentication

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/Auth/register` | Register a new user |
| POST | `/api/Auth/login` | Authenticate user and generate tokens |
| POST | `/api/Auth/refresh` | Generate new access and refresh tokens |

### Products

All product endpoints require JWT authentication.

| Method | Endpoint | Authentication | Description |
|---|---|---|---|
| GET | `/api/Products?page=1&pageSize=10` | JWT | Get paginated products |
| GET | `/api/Products/{id}` | JWT | Get product by ID |
| POST | `/api/Products` | JWT | Create a new product |
| PUT | `/api/Products` | JWT | Update an existing product |
| DELETE | `/api/Products/{id}` | JWT | Delete a product |


#### Pagination Parameters

| Parameter | Default | Allowed Range | Description |
|---|---:|---:|---|
| `page` | 1 | Greater than 0 | Page number |
| `pageSize` | 10 | 1 - 100 | Number of products per page |


## Swagger / OpenAPI

The API provides interactive Swagger/OpenAPI documentation for all available endpoints.

After starting the application with Docker, open:

http://localhost:8080/swagger/index.html

Swagger can be used to:

- View all available API endpoints
- View request and response models
- Test API endpoints
- Authenticate using the JWT access token


## Authentication Flow

The application uses JWT-based authentication with a refresh token strategy.

### Registration Flow

```text
Client
  ↓
POST /api/Auth/register
  ↓
Validate registration data
  ↓
Check if email already exists
  ↓
Hash password
  ↓
Save user to SQL Server
  ↓
Generate access token
  ↓
Generate refresh token
  ↓
Return tokens to client
```

### Login Flow

```text
Client
  ↓
POST /api/Auth/login
  ↓
Validate email and password
  ↓
Verify user credentials
  ↓
Generate access token
  ↓
Generate refresh token
  ↓
Return authentication response
```



### Accessing Protected APIs

```text
Client
  ↓
Authorization: Bearer <access_token>
  ↓
JWT Authentication Middleware
  ↓
Validate token
  ↓
Authorize request
  ↓
ProductsController
  ↓
Product Service
  ↓
Product Repository
  ↓
SQL Server
```


### Refresh Token Flow

```text
Access token expires
  ↓
Client sends refresh token
  ↓
POST /api/Auth/refresh
  ↓
Validate refresh token
  ↓
Check refresh token expiry
  ↓
Generate new access token
  ↓
Generate new refresh token
  ↓
Return new authentication response
```

## Environment Setup

### Prerequisites

- .NET 8 SDK
- Docker Desktop
- SQL Server
- Visual Studio 2022 or Visual Studio Code

### Configuration

The application uses `appsettings.json` for application configuration.

The main configuration includes:

- SQL Server connection string
- JWT secret key
- JWT issuer
- JWT audience
- JWT token expiry

For local development, update the connection string and JWT settings according to your environment.

Do not commit production secrets, passwords, or sensitive credentials to source control.

### Run the Application

The application can be started using Docker Compose.

From the project root directory, run:

```bash
docker compose up --build
```

After the containers start, the API will be available at:

```text
http://localhost:8080
```

Swagger UI will be available at:

```text
http://localhost:8080/swagger/index.html
```

To verify that the containers are running:

```bash
docker compose ps
```

## Docker Setup

The application is containerized using Docker and Docker Compose.

The Docker environment consists of two containers:

- **CRN Product API** — ASP.NET Core Web API container
- **SQL Server** — Microsoft SQL Server 2022 database container


### Start the Application

From the project root directory, run:

```bash
docker compose up --build
```
From the project root directory, build the Docker images and start the application using:
```markdown
From the project root directory, run:
```


### Start Without Rebuilding

If no source code or Docker configuration has changed:

```bash
docker compose up
```

If the Docker image is already built and no Docker configuration has changed, start the containers using:

```markdown
If no source code or Docker configuration has changed:
```

### Stop the Application

Press:

```text
Ctrl + C
```


Or run:

```bash
docker compose down
```


## Deployment Procedure

The application can be deployed using Docker Compose.

### Deployment Steps

1. Clone the repository:

```bash
git clone https://github.com/Prathameshmahajan03/CRNProductAPI.git
```

2. Navigate to the project directory:

```bash
cd CRNProductAPI
```

3. Build the Docker image and start the containers:

```bash
docker compose up --build
```

4. Verify that the containers are running:

```bash
docker ps
```

5. Open Swagger UI:

```text
http://localhost:8080/swagger/index.html
```

6. To stop the application:

```bash
docker compose down
```


### Testing

The project includes automated tests using xUnit, Moq, and WebApplicationFactory.

The tests cover:

- Product service operations
- Authentication service operations
- Product creation and retrieval scenarios
- Product update and delete scenarios
- User registration and login scenarios
- Refresh token scenarios
- API integration testing


### Running Tests

Tests can be executed using Visual Studio Test Explorer or the .NET CLI:

```bash
dotnet test
```

### Current Test Status

- **Total tests:** 7
- **Passed:** 7
- **Failed:** 0

## Error Handling

The application uses a global exception handling middleware to handle unexpected exceptions consistently.

The `ExceptionMiddleware`:

- Catches unhandled exceptions
- Logs the exception details using Serilog
- Returns an appropriate HTTP status code
- Provides a consistent error response to the client

This prevents internal exception details from being exposed directly through the API response.


## Project Structure

```text
CRNProductAPI
│
├── src
│   ├── API
│   │   ├── Controllers
│   │   ├── Middleware
│   │   ├── Filters
│   │   ├── Logs
│   │   ├── appsettings.json
│   │   ├── Dockerfile
│   │   └── Program.cs
│   │
│   ├── Application
│   │   ├── DTOs
│   │   ├── Interfaces
│   │   ├── Mapping
│   │   ├── Services
│   │   └── Validators
│   │
│   ├── Domain
│   │   └── Entities
│   │
│   └── Infrastructure
│       ├── Data
│       ├── Repositories
│       └── Identity
│
├── tests
│   └── CRNProductAPI.Tests
│       ├── AuthServiceTests.cs
│       └── ProductServiceTests.cs
│
├── docker-compose.yml
├── CRNProductAPI.sln
└── README.md
```