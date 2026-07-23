<div align="center">

# 🎮 GameRa

### Modern Steam-inspired Digital Distribution Platform

Enterprise Backend built with ASP.NET Core, Domain-Driven Design, Modular Monolith, CQRS, Event-Driven Architecture and Clean Architecture

<p>

![.NET](https://img.shields.io/badge/.NET-9-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-336791?style=for-the-badge&logo=postgresql)
![Redis](https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?style=for-the-badge&logo=rabbitmq)
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
- Reviews
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

- Domain Events
- Integration Events
- Asynchronous Messaging

---
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
| Messaging | RabbitMQ, MassTransit |
| Scheduling | Quartz.NET |
| Authentication | Keycloak, ASP.NET Identity, JWT, OAuth2, OpenID Connect |
| Testing | xUnit, Integration Tests |
| Logging | Serilog |
| Containerization | Docker, Docker Compose |

---

# 🔄 CQRS Flow

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
---

# 🔄 Event Flow

```text
Business Action

↓

Domain Event

↓

Outbox

↓

Background Processor

↓

RabbitMQ

↓

Inbox

↓

Consumer

↓

Integration Event

↓

Target Module
```
---
# 🔐 Authentication & Identity

Authentication and Authorization are implemented using an external Identity Provider.

Technologies:

- Keycloak
- ASP.NET Identity
- JWT Bearer Authentication
- OAuth2
- OpenID Connect

This approach separates identity management from business modules while improving scalability, maintainability and security.

---
# 📡 Messaging

GameRa uses asynchronous communication between modules.

Technologies:

- RabbitMQ
- MassTransit
- Integration Events
- Domain Events
- Inbox Pattern
- Outbox Pattern

---
# ⏰ Background Jobs

GameRa uses Quartz.NET to execute scheduled background jobs.

Typical scenarios include:

- Processing Outbox Messages
- Cleaning Expired Data
- Scheduled Synchronization
- Periodic Maintenance Jobs
---

# 🚀 Running Project

Clone repository

git clone https://github.com/Mhamidz1015/GameRa.git
Restore packages

dotnet restore
Run migrations

dotnet ef database update
Run application

dotnet run
---
# 🧪 Testing

GameRa follows different testing strategies to ensure code quality.

### Unit Testing

- xUnit
- Domain Tests
- Application Tests

### Integration Testing

- ASP.NET Core Integration Tests
- End-to-End API Tests
- Database Integration Tests

The testing approach helps verify business logic, API endpoints and module interactions.

---

# 📈 Future Improvements

- Payment Gateway
- Notification Service
- Wishlist
- Discounts
- Coupons
- Recommendation Engine
- Search Service
- Admin Dashboard
- CDN Integration
- File Storage

---
# 💾 Infrastructure

- PostgreSQL
- Redis
- Docker
- Docker Compose
---
# 🛠 Engineering Principles

- SOLID
- DRY
- KISS
- YAGNI
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

GitHub:
https://github.com/Mhamidz1015
