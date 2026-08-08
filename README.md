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
- Entity Framework Core with SQL Server
- Repository and Service layer pattern
- FluentValidation for request validation
- Global exception handling middleware
- Structured logging using Serilog
- Swagger/OpenAPI documentation
- Docker and Docker Compose support


## Architecture

The application follows a layered architecture with clear separation of responsibilities.

```text
CRN Product API
│
├── API
│   ├── Controllers
│   ├── Middleware
│   ├── Filters
│   └── Program.cs
│
├── Application
│   ├── DTOs
│   ├── Interfaces
│   ├── Services
│   ├── Mapping
│   └── Validators
│
├── Domain
│   └── Entities
│
└── Infrastructure
    ├── Data
    ├── Repositories
    └── Identity

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
- **Testing:** xUnit and Moq


## API Endpoints

### Authentication

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/Auth/register` | Register a new user |
| POST | `/api/Auth/login` | Authenticate user and generate tokens |
| POST | `/api/Auth/refresh` | Generate new access and refresh tokens |

### Products

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/Products` | Get all products |
| GET | `/api/Products/{id}` | Get product by ID |
| POST | `/api/Products` | Create a new product |
| PUT | `/api/Products` | Update an existing product |
| DELETE | `/api/Products/{id}` | Delete a product |



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
Generate access token
  ↓
Generate refresh token
  ↓
Return tokens to client
```



### Accessing Protected APIs

```text
Client
  ↓
Authorization: Bearer <access_token>
  ↓
JWT Authentication
  ↓
Validate token
  ↓
Authorize request
  ↓
Access protected Product APIs
```


### Refresh Token Flow

```text
Access token expires
  ↓
Client sends refresh token
  ↓
Validate refresh token
  ↓
Check refresh token expiry
  ↓
Generate new access token
  ↓
Generate new refresh token
  ↓
Rotate refresh token
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

### Run the Application

The recommended way to run the application is using Docker Compose:

```bash
docker compose up --build
```

```text
http://localhost:8080
```

```text
http://localhost:8080/swagger/index.html
```



## Docker Setup

The application is containerized using Docker and Docker Compose.

The Docker environment consists of two containers:

- **CRN Product API** — ASP.NET Core Web API
- **SQL Server** — Microsoft SQL Server 2022




### Start the Application

From the project root directory, run:

```bash
docker compose up --build
```

### Start Without Rebuilding

If no source code or Docker configuration has changed:

```bash
docker compose up
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
git clone <repository-url>
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



## Testing

The project includes automated unit tests using xUnit and Moq.

The tests cover:

- Product service operations
- Authentication service operations
- Product creation and retrieval scenarios
- Product update and delete scenarios
- User registration and login scenarios
- Refresh token scenarios

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