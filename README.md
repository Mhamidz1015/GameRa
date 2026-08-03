<div align="center">

# 🎮 GameRa

### Modern Steam-inspired Digital Distribution Platform

Enterprise Backend built with ASP.NET Core, Domain-Driven Design, Modular Monolith, CQRS, Event-Driven Architecture and Clean Architecture

<p>

![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-336791?style=for-the-badge&logo=postgresql)
![Redis](https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis)
![MassTransit](https://img.shields.io/badge/MassTransit-FF6600?style=for-the-badge)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker)

</p>

Enterprise-grade backend demonstrating modern software architecture and distributed system concepts.

</div>

---

# 📖 Overview

GameRa is a Steam-inspired digital game distribution platform built with modern .NET technologies.

The project demonstrates how to build a scalable, maintainable and production-ready backend using Domain-Driven Design (DDD), CQRS and Modular Monolith Architecture.

Rather than focusing only on business features, this project emphasizes clean architecture, modularity, event-driven communication and enterprise development practices.

---

# ✨ Features

- User Authentication & Authorization
- External Identity Provider Integration
- JWT Authentication
- OAuth2 & OpenID Connect
- Game Management
- Digital Store
- Shopping Cart
- Orders Management
- User Library
- Redis Distributed Caching
- Background Jobs
- Quartz.NET Scheduler
- Event-Driven Communication
- Inbox / Outbox Pattern
- Integration Events
- Domain Events
- Health Checks
- Unit Testing
- Integration Testing
- Docker Support

---

# 📦 Modules

| Module  | Responsibility                                      |
|---------|-----------------------------------------------------|
| Users   | User registration, authentication, profile management |
| Games   | Game catalog, categories, game management           |
| Store   | Shopping cart, orders, payments, customer management |
| Library | User's purchased games, archiving                   |

Each module follows **Clean Architecture** with four layers:

```
Domain → Application → Infrastructure → Presentation
```

Modules communicate exclusively through **Integration Events** — never via direct references.

---

# 🏛 Architecture

GameRa is designed as an Enterprise Backend application following modern software architecture principles.

### Architectural Patterns

- Modular Monolith
- Clean Architecture
- Domain-Driven Design (DDD)
- CQRS
- Vertical Slice Architecture
- Event-Driven Architecture
- Inbox / Outbox Pattern

### Communication Patterns

- Domain Events (in-memory, intra-module)
- Integration Events (cross-module via MassTransit)
- Asynchronous Messaging

---

```
                HTTP Request
                     │
                     ▼
              ASP.NET Core API
                     │
                     ▼
                  MediatR
            Command / Query
                     │
                     ▼
             Application Layer
                     │
                     ▼
               Domain Layer
                     │
                     ▼
          Infrastructure Layer
                     │
                     ▼
               PostgreSQL
```

---

# 🏗 Design Patterns

The project implements several Enterprise Design Patterns.

## Architectural Patterns

- Modular Monolith
- Clean Architecture
- Domain-Driven Design (DDD)
- CQRS
- Vertical Slice Architecture
- Event-Driven Architecture

## Design Patterns

- Repository Pattern
- Unit of Work
- Dependency Injection
- Mediator Pattern
- Domain Events
- Integration Events
- Inbox Pattern
- Outbox Pattern
- Options Pattern

---

# ⚙ Tech Stack

| Category | Technologies |
|----------|--------------|
| Runtime | .NET 10, ASP.NET Core |
| Language | C# |
| Architecture | Modular Monolith, Clean Architecture, DDD, CQRS, Vertical Slice |
| ORM | Entity Framework Core, Dapper |
| Database | PostgreSQL |
| Cache | Redis |
| Messaging | MassTransit (InMemory) |
| Background Processing | Quartz.NET |
| Authentication | Keycloak, JWT, OAuth2, OpenID Connect |
| Testing | xUnit, FluentAssertions, Bogus, Testcontainers for .NET |
| Logging | Serilog |
| Containerization | Docker, Docker Compose |

---

# 🔄 CQRS Flow

```
Client
  ↓
API Endpoint
  ↓
MediatR
  ↓
Command / Query
  ↓
Handler
  ↓
Repository
  ↓
Database
```

---

# 🔄 Event Flow

```
Business Action
  ↓
Domain Event raised in memory
  ↓
SaveChanges → Outbox Table (same transaction)
  ↓
Quartz.NET Background Job
  ↓
Domain Event Handler
  ↓
MassTransit (InMemory transport)
  ↓
Inbox Table (target module)
  ↓
Quartz.NET Background Job
  ↓
Integration Event Handler
  ↓
Target Module Command
```

This guarantees **at-least-once delivery** and **exactly-once processing** via idempotency checks.

---

# 🔐 Authentication & Identity

Authentication and Authorization are implemented using an external Identity Provider.

Technologies:

- Keycloak
- JWT Bearer Authentication
- OAuth2
- OpenID Connect

This approach separates identity management from business modules while improving scalability, maintainability and security.

---

# 📡 Messaging

GameRa uses asynchronous communication between modules via **MassTransit** with InMemory transport.

- Integration Events carry data across module boundaries
- Each module has its own Inbox to receive events
- Each module has its own Outbox to publish domain events
- Idempotency is enforced — duplicate messages are safely ignored

---

# ⏰ Background Jobs

GameRa uses Quartz.NET to execute scheduled background jobs.

Each module runs two jobs:

| Job | Responsibility |
|-----|---------------|
| ProcessOutboxJob | Reads domain events from outbox and dispatches handlers |
| ProcessInboxJob | Reads integration events from inbox and dispatches handlers |

---

# 🚀 Running the Project

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

### With Docker Compose

```bash
git clone https://github.com/Mhamidz1015/GameRa.git
cd GameRa
docker-compose up --build
```

This starts all required services automatically:
- PostgreSQL
- Redis
- Keycloak (with pre-configured realm)
- GameRa API

### Running Locally

```bash
# 1. Start infrastructure services
docker-compose up postgres redis keycloak

# 2. Restore packages
dotnet restore

# 3. Run migrations
dotnet ef database update

# 4. Run application
dotnet run --project src/API/GameRa
```

> ⚠️ Keycloak must be running and configured before starting the API.
> The realm configuration is imported automatically via Docker.

---

# 🧪 Testing

GameRa follows different testing strategies to ensure code quality.

### Unit Testing

- xUnit
- Domain aggregate tests
- Business rule validation

### Integration Testing (per module)

- Testcontainers for .NET (PostgreSQL)
- Real database, no mocks
- Command and Query handler tests

### System Integration Testing

- Full end-to-end cross-module tests
- Testcontainers (PostgreSQL + Redis + Keycloak)
- Verifies event propagation between modules (Outbox → MassTransit → Inbox)

```bash
# Run all tests
dotnet test

# Run only integration tests
dotnet test test/GameRa.IntegrationTests
```

---

# 📈 Future Improvements

- Payment Gateway Integration
- Notification Service
- Wishlist
- Discounts & Coupons
- Recommendation Engine
- Search Service
- Admin Dashboard

---

# 💾 Infrastructure

- PostgreSQL — persistent storage per module (separate schemas)
- Redis — distributed caching
- Docker & Docker Compose — containerized local environment

---

# 🛠 Engineering Principles

- SOLID
- Separation of Concerns
- Dependency Injection
- High Cohesion
- Low Coupling
- Fail Fast

---

# 📚 Enterprise Concepts Demonstrated

This project demonstrates the implementation of several enterprise backend concepts, including:

- Domain-Driven Design (DDD)
- Clean Architecture
- Modular Monolith
- CQRS
- Vertical Slice Architecture
- Event-Driven Architecture
- Domain Events
- Integration Events
- Inbox / Outbox Pattern
- Asynchronous Messaging
- Distributed Caching
- Identity Provider Integration
- Background Job Processing
- Production-ready API Design
- Unit & Integration Testing

---

# 👨‍💻 Author

Hamidreza Zahedi

Backend Developer (.NET)

GitHub: https://github.com/Mhamidz1015
