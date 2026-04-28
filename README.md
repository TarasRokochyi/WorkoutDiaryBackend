# Physical Activity Analysis — Backend

This repository contains the backend for a physical activity analysis system based on workout data.

The system is built using a **three-layer architecture**:

- Controllers Layer
- Business Logic Layer (BLL)
- Data Access Layer (DAL)

**PostgreSQL** is used as the database, with **Entity Framework Core** as the ORM.

## Controllers

Controllers manage:

- Workout data
- Exercise data
- Workout templates
- User authentication and authorization
- Data for charts and statistics

## Authentication

User management is implemented using **ASP.NET Core Identity**.

Authentication and authorization are based on:

- JWT Access Tokens
- Refresh Tokens

Automatic token refresh is supported for maintaining user sessions.

## Design Patterns

- **Generic Repository Pattern** — used in the DAL for CRUD operations
- **Unit of Work Pattern** — used for communication between BLL and DAL

## Infrastructure

- PostgreSQL database running in Docker
- Dockerfiles for backend services
- CI pipeline that builds and pushes Docker images to GitHub Container Registry

## Technologies

- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- ASP.NET Core Identity
- JWT Authentication
- Docker
- GitHub CI/CD

Link to WorkoutDiaryFrontend - https://github.com/TarasRokochyi/WorkoutDiaryFrontend
