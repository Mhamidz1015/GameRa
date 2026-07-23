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

- User Authentication
- JWT Authorization
- ASP.NET Identity
- Game Management
- Digital Store
- Shopping Cart
- Orders
- Reviews
- User Library
- Redis Caching
- Event-Driven Communication
- Health Checks
- API Documentation
- Docker Support

---

# 🏛 Architecture

GameRa follows several enterprise architecture patterns:

- Modular Monolith
- Domain Driven Design (DDD)
- Clean Architecture
- CQRS
- Vertical Slice Architecture
- Event-Driven Architecture

                HTTP Request
                     │
                     ▼
             ASP.NET API Layer
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

# 🧩 Project Structure

GameRa

├── API
│
├── Modules
│     ├── Users
│     ├── Games
│     ├── Store
│     ├── Reviews
│     └── ...
│
├── Common
│
├── BuildingBlocks
│
├── Docker
│
└── Shared
Every module is designed to be isolated and communicates through Integration Events.

---

# 🏗 Design Patterns

The project applies several enterprise design patterns.

## Architectural Patterns

- Modular Monolith
- Domain Driven Design
- CQRS
- Vertical Slice Architecture
- Clean Architecture

## Design Patterns

- Repository Pattern
- Unit of Work
- Dependency Injection
- Mediator Pattern
- Domain Events
- Integration Events
- Outbox Pattern
- Options Pattern

---

# 📦 Technologies

## Backend

- ASP.NET Core
- .NET 9
- C#

## Database

- PostgreSQL
- Entity Framework Core
- Dapper

## Authentication

- ASP.NET Identity
- JWT Authentication

## Messaging

- RabbitMQ
- MassTransit

## Cache

- Redis

## Validation

- FluentValidation

## Logging

- Serilog

## API

- Swagger
- REST API

## Containerization

- Docker
- Docker Compose

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

# 📡 Event Driven Flow

Business Action

↓

Domain Event

↓

Outbox

↓

RabbitMQ

↓

Consumer

↓

Integration Event

↓

Another Module
---

# 🔐 Authentication

Authentication is implemented using:

- ASP.NET Identity
- JWT Bearer Authentication

The architecture separates authentication from business modules while keeping the system modular.

---

# ⚡ Caching

Redis is used to improve application performance and reduce unnecessary database queries.

---

# 📚 API Documentation
Swagger is integrated for interactive API documentation.

https://localhost:5001/swagger
---

# 🐳 Running with Docker

docker compose up -d
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

# 🛠 Engineering Principles

- SOLID
- DRY
- KISS
- Separation of Concerns
- Dependency Injection
- High Cohesion
- Low Coupling

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

# 📚 Learning Objectives

This project was built to practice and demonstrate:

- Enterprise Backend Development
- Domain Driven Design
- Clean Architecture
- Modular Monolith
- CQRS
- Event-Driven Architecture
- Distributed Systems Concepts
- Production-ready ASP.NET Core Applications

---

# 👨‍💻 Author

Hamidreza Zahedi

Backend Developer (.NET)

GitHub:
https://github.com/Mhamidz1015
