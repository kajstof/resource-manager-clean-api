# Resource Manager

## How to setup local environment

### Starting database (postgres) in docker, it can be done with docker-compose tool:

There are three services:
- postgres (`localhost:5432`) - required
- pgadmin4 (`http://localhost:5433`) - for database management
- adminer (`http://localhost:5434`) - for database management

```bash
docker-compose up
```

### Migrations

#### Installing ef tools (prerequisite)

```bash
dotnet tool install -g dotnet-ef
```

#### Creating migration

```bash
dotnet ef migrations add InitialMigration
```

#### Performing migrations

```bash
dotnet ef database update
```

### Creating secrets (instead configuration in `appsettings.json`)

```bash
cd ResourceManager/src/ResourceManager.Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:ResourceDbContext" "Host=localhost;Database=postgres;Username=postgres;Password=postgres" 
```

To list secrets:

```bash
dotnet user-secrets list
```

### Generating tokens for users

For example:

```bash
cd ResourceManager/src/ResourceManager.Api
dotnet user-jwts create --name anna --role admin --role user
dotnet user-jwts create --name elza --role user
dotnet user-jwts create --name olaf --role user
```

To list tokens:

```bash
dotnet user-jwts list --json
```
