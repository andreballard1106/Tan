# Tandem Backend Coding Exercise

A RESTful API service for managing user entities, built with .NET 8 using Clean Architecture principles and Domain-Driven Design.

## Architecture

The solution follows Clean Architecture with clear separation of concerns:

- **Tandem.Domain**: Domain entities and repository interfaces
- **Tandem.Application**: Business logic, MediatR handlers, DTOs, and validation
- **Tandem.Infrastructure**: Data access layer with Entity Framework Core
- **Tandem.Api**: REST API endpoints and configuration
- **Tandem.IntegrationTests**: Integration tests with actual HTTP calls

## Features

- RESTful endpoints for user management (POST, GET, PUT)
- Domain-Driven Design with rich domain models
- CQRS pattern using MediatR
- FluentValidation for request validation
- Swagger/OpenAPI documentation
- Integration tests with comprehensive coverage
- Docker support with Dockerfile and docker-compose
- In-memory database (can be easily switched to SQL Server/PostgreSQL)

## API Endpoints

### POST /api/users
Creates a new user.

**Request Body:**
```json
{
  "firstName": "Matthew",
  "middleName": "Decker",
  "lastName": "Lund",
  "phoneNumber": "555-555-5555",
  "emailAddress": "matt@awesomedomain.com"
}
```

**Response:** 201 Created with user details including generated userId

### GET /api/users?emailAddress={email}
Retrieves a user by email address. Returns a combined name field.

**Response:**
```json
{
  "userId": "54c4e684-0a6a-449d-b445-61ddd12Ad3d",
  "name": "Matthew Decker Lund",
  "phoneNumber": "555-555-5555",
  "emailAddress": "matt@awesomedomain.com"
}
```

### PUT /api/users/{emailAddress}
Updates an existing user by email address.

**Request Body:**
```json
{
  "firstName": "Updated",
  "middleName": "New",
  "lastName": "Name",
  "phoneNumber": "555-999-9999"
}
```

## HTTP Status Codes

- `200 OK`: Successful GET or PUT operation
- `201 Created`: Successful user creation
- `400 Bad Request`: Validation errors or invalid request data
- `404 Not Found`: User not found
- `409 Conflict`: Duplicate email address on creation

## Getting Started

### Prerequisites

- .NET 8 SDK
- Docker (optional, for containerized deployment)

### Running the Application

1. Restore dependencies:
```bash
dotnet restore
```

2. Build the solution:
```bash
dotnet build
```

3. Run the API:
```bash
cd src/Tandem.Api
dotnet run
```

4. Access Swagger UI:
   - Navigate to `https://localhost:5001/swagger` or `http://localhost:5000/swagger`

### Running with Docker

1. Build and run with docker-compose:
```bash
docker-compose up --build
```

2. Access the API at `http://localhost:5000`

### Running Tests

```bash
dotnet test
```

## Technology Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core (InMemory provider)
- MediatR (CQRS/Mediator pattern)
- FluentValidation
- Swashbuckle (Swagger)
- xUnit and FluentAssertions (Testing)

## Project Structure

```
Tandem/
├── src/
│   ├── Tandem.Api/              # Web API layer
│   ├── Tandem.Application/      # Application logic
│   ├── Tandem.Domain/           # Domain entities
│   └── Tandem.Infrastructure/   # Data access
├── tests/
│   └── Tandem.IntegrationTests/ # Integration tests
├── Dockerfile
├── docker-compose.yml
└── README.md
```

