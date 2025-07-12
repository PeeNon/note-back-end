# Notes API

A RESTful API for managing user notes with JWT authentication, built with ASP.NET Core 9.0.

## Features

- JWT-based authentication and authorization
- CRUD operations for notes
- User registration and login
- Advanced note filtering (search, date range, sorting)
- Pagination support
- User-scoped data access
- Swagger API documentation

## Prerequisites

- .NET SDK 9.0
- Docker (for SQL Server)

## Quick Start Guide

### 1. Clone and Navigate to Project

```bash
cd ~/Documents/hobbie-project/techbodia/note-back-end
```

### 2. Start SQL Server Database

```bash
# Start SQL Server using Docker Compose
docker-compose up -d

# Wait for SQL Server to initialize (about 30 seconds)
# Check if container is running
docker ps
```

### 3. Install .NET Entity Framework Tools (if not already installed)

```bash
dotnet tool install --global dotnet-ef

# Add to PATH if needed (for zsh users)
export PATH="$PATH:/Users/hdtelecom/.dotnet/tools"
```

### 4. Restore Dependencies

```bash
dotnet restore
```

### 5. Apply Database Migrations

```bash
# Create database schema
dotnet ef database update
```

### 6. Start the Application

```bash
dotnet run
```

## Access Points

- **API Base URL**: `http://localhost:5288`
- **Swagger UI**: `http://localhost:5288/swagger/index.html`

## Default Login Credentials

- **Username**: `techbodia`
- **Password**: `techbodia`

## API Testing Steps

1. **Login**: Use `/User/login` endpoint with default credentials
2. **Copy JWT Token** from the response
3. **Authorize**: Click "Authorize" in Swagger UI
4. **Enter Token**: Format as `Bearer <your-jwt-token>`
5. **Test Endpoints**: All `/api/Notes` endpoints are now accessible

## Restart Guide (After Laptop Shutdown)

When you restart your laptop, follow these steps:

```bash
# 1. Navigate to project directory
cd ~/Documents/hobbie-project/techbodia/note-back-end

# 2. Start SQL Server
docker-compose up -d

# 3. Wait 30 seconds for SQL Server to start
sleep 30

# 4. Start the application
dotnet run
```

## One-Command Startup Script

Create a `start.sh` file for easy startup:

```bash
#!/bin/bash
echo "Starting Notes API..."
docker-compose up -d
echo "Waiting for SQL Server to start..."
sleep 30
echo "Starting application..."
dotnet run
```

Make it executable and run:

```bash
chmod +x start.sh
./start.sh
```

## Configuration

### Database Connection

- **SQL Server**: Docker container on port 1433
- **Database**: `notesapplication`
- **Connection String**: Configured in `appsettings.json`

### JWT Configuration

- **Issuer**: `techbodia`
- **Audience**: `techbodia`
- **Token Expiration**: 30 days

## Project Structure

```
├── controllers/          # API controllers
├── dto/                 # Data Transfer Objects
├── models/              # Entity models and DB contexts
├── repositories/        # Data access layer
├── validations/         # Custom validation attributes
├── middlewares/         # Custom middleware
├── Migrations/          # Entity Framework migrations
├── docker-compose.yml   # SQL Server container config
└── appsettings.json     # Application configuration
```

## Key Endpoints

### Authentication

- `POST /User/login` - User login
- `POST /User/register` - User registration

### Notes (Protected)

- `GET /api/Notes` - Get paginated notes with filtering
- `GET /api/Notes/{id}` - Get specific note
- `POST /api/Notes` - Create new note
- `PUT /api/Notes/{id}` - Update note
- `DELETE /api/Notes/{id}` - Delete note
- `DELETE /api/Notes` - Bulk delete notes

### Query Parameters for Notes

- `PageNumber` - Page number (default: 1)
- `PageSize` - Items per page (default: 10)
- `SearchQuery` - Search by title
- `Sort` - Sort by: `title` or `createdAt`
- `Order` - Sort order: `asc` or `desc`
- `Dfrom` - Date range start
- `Dto` - Date range end

## Troubleshooting

### Database Connection Issues

```bash
# Check if Docker is running
docker ps

# Check SQL Server logs
docker logs sqlserver

# Restart SQL Server container
docker-compose restart
```

### Port Conflicts

```bash
# Check what's using port 1433
lsof -i :1433

# Check what's using port 5288
lsof -i :5288
```

### Clean Restart

```bash
# Stop all containers
docker-compose down

# Start fresh
docker-compose up -d
sleep 30
dotnet run
```

## Tech Stack

- **Framework**: ASP.NET Core 9.0
- **Database**: SQL Server (Docker)
- **ORM**: Entity Framework Core (migrations) + Dapper (data access)
- **Authentication**: JWT Bearer tokens
- **Password Hashing**: BCrypt
- **API Documentation**: Swagger/OpenAPI
- **Validation**: Data Annotations + Custom Attributes

## Development

### Adding New Migrations

```bash
# Create new migration
dotnet ef migrations add YourMigrationName

# Apply migration
dotnet ef database update
```

### Useful Docker Commands

```bash
# View running containers
docker ps

# Stop SQL Server
docker-compose down

# View container logs
docker logs sqlserver

# Remove data volume (caution: will lose all data)
docker-compose down -v
```

---

**Note**: The application automatically creates a default user (`techbodia`/`techbodia`) on startup if it doesn't exist.
