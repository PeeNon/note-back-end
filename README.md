# Prerequisite
* .NET SDK 9.0
* SQL Server
## Installation Guide

Configure SQL Server `ConnectionStrings` or use existing

```
appsettings.json
```

Install dotnet-ef if not installed:

```
dotnet tool install --global dotnet-ef
```
Restore dependencies:
```
dotnet restore
```

Migrate schema if not migrated:

```
dotnet ef database update
```

Start the application:

```
dotnet run
```

------------

Once up, you may access the swagger ui at [http://localhost:5288/swagger/index.html](http://localhost:5288/swagger/index.html) 