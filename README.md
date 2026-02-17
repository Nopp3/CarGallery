# CarGallery

A full-stack hobby application for managing and browsing car listings.

## Table of Contents

- [Description](#description)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [Architecture](#architecture)
- [Overview](#overview)
- [Features](#features)
- [Security Notes](#security-notes)
- [Roadmap](#roadmap)

## Description

CarGallery is a full-stack app where users can register, log in, add cars (with images), and browse cars posted by others. Admin users can manage reference data (brands / body types) and users.

## Tech Stack

Frontend

- Angular 16
- Bootstrap 5
- TypeScript
- RxJS 7
- Font Awesome

Backend

- ASP.NET Core Web API (.NET 6)
- Entity Framework Core (SQL Server provider)
- JWT auth (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- Password hashing with BCrypt

Database

- SQL Server (Azure SQL Edge in Docker)

Infra

- Docker Compose
- Caddy (reverse proxy, the only public entrypoint)
- Nginx (serves the Angular build with SPA fallback)

## Getting Started

### Docker (recommended)

Prerequisites: Docker Desktop.

1. Clone:

```bash
git clone https://github.com/Nopp3/CarGallery
cd CarGallery
```

2. Create `.env`:

```bash
cp .env.example .env
```

3. Set secrets in `.env`:

- `MSSQL_SA_PASSWORD`: SQL Server `sa` password for the DB container
- `JWT_SIGNING_KEY`: JWT signing key for the API (must be at least 32 characters)
- `SEED_ADMIN_PASSWORD`: password for the seeded admin user (created on first database creation)

Optional:

- `SEED_ADMIN_USERNAME`: username for the seeded admin user (default: `admin`)
- `SEED_ADMIN_EMAIL`: email for the seeded admin user (default: `support@cargallery.com`)

4. Build and start:

```bash
docker compose up -d --build
```

5. Open:

- App: `http://127.0.0.1:8080`
- Health: `http://127.0.0.1:8080/health` (expected response: `Healthy`)

Reset the database (destructive):

```bash
docker compose down -v
```

### Local Development (optional)

Backend (see `Backend/CarGalleryAPI/CarGalleryAPI/Properties/launchSettings.json`):

```bash
cd Backend/CarGalleryAPI/CarGalleryAPI
dotnet run
```

Frontend (proxies `/api` to the backend):

```bash
cd Frontend/CarGallery
npm ci
ng serve --proxy-config proxy.conf.json
```

Open `http://localhost:4200`.

## Architecture

```
Browser
  -> Caddy (exposed on host)
       /            -> Frontend container (Nginx serves Angular build)
       /api/*       -> Backend container (ASP.NET Core)
       /health      -> Backend container
Backend -> Azure SQL Edge (SQL Server) container
```

## Overview

There are 4 main pages:

- **Log in / Sign up**: authentication and registration (guest-only routes).
- **Home**: shows the authenticated user's own cars, with add/edit/delete.
- **All**: shows cars uploaded by all users, with a brand filter.
- **Panel (Administrators Only)**: admin panel to manage brands, body types, and users.

## Features

### Containerized stack

- Full stack runs via Docker Compose.
- Caddy is the only container exposed to the outside; frontend/backend/DB are internal.

### Authentication and authorization

- Login issues a JWT and stores it in an HttpOnly cookie (`cg_access_token`).
- The backend enforces:
  - admin endpoints: roles `HeadAdmin`, `Admin`
  - car write operations: ownership checks (owner or admin can update/delete)

### API robustness

- `GET /health` endpoint for health checks.
- Unauthorized and forbidden API responses use `application/problem+json` (ProblemDetails) with a `traceId`.

### Database initialization (current)

- On first run, the API creates `CarGalleryDB` and seeds reference data from:
  - `Backend/CarGalleryAPI/CarGalleryAPI/Initial.sql`
  - `Backend/CarGalleryAPI/CarGalleryAPI/CarGalleryDBData.xlsx`

### Car upload and management (current)

- Upload an image and car metadata.
- Images are currently written to the API container disk (`wwwroot/images`) and served as static files.

## Security Notes

- The frontend does not rely on `sessionStorage` for roles; it uses `GET /api/auth/me` as the source of truth for UI decisions.
- The API also supports `Authorization: Bearer <token>` for tooling, but browser usage is cookie-based.

## Roadmap

Planned improvements:

- Add minimal integration tests and GitHub Actions CI
- Upgrade backend to .NET 10 (LTS) and align packages
- Introduce EF Core migrations as the schema source of truth and implement idempotent seeding
- Move the Excel import to development-only or a manual admin action
- Replace disk image storage with Cloudinary signed uploads (stateless API)
- Migrate from SQL Server to PostgreSQL
