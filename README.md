# Myfirstapp (ASP.NET Core MVC)

This is an ASP.NET Core MVC app with multilingual UI (HU/EN/CZ) and topic-based educational content.

## Local setup

### 1) Configure database connection (do NOT commit secrets)

The app expects a SQL Server / Azure SQL connection string under:

- `ConnectionStrings:Default`

Recommended options:

**Option A — Environment variable (Windows PowerShell):**

```powershell
$env:ConnectionStrings__Default = "Server=tcp:<server>.database.windows.net,1433;Initial Catalog=<db>;Persist Security Info=False;User ID=<user>;Password=<password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

**Option B — User Secrets (development):**

```powershell
dotnet user-secrets init

dotnet user-secrets set "ConnectionStrings:Default" "Server=tcp:<server>.database.windows.net,1433;Initial Catalog=<db>;Persist Security Info=False;User ID=<user>;Password=<password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

The tracked `appsettings.json` files contain only placeholders.

### 2) Run

```powershell
cd WebApplication1

dotnet run
```

## Database tables used

## API endpoints

### GET /api/getexercises

Returns exercise metadata for a selected language. Intended for mobile clients (for example React Native).

Query parameters:

- `language` (required): 2-letter language code. Supported values in practice: `hu`, `en`, `cs` (mapped to `cz` in database queries).

Success response (`200 OK`): array of objects with fields:

- `exercisetitle`
- `exerciseid`
- `topic`
- `grade`

Error response (`400 Bad Request`):

```json
{
	"error": "language is required"
}
```

Example request:

```http
GET /api/getexercises?language=en
```

Example success response:

```json
[
	{
		"exercisetitle": "Recycling quiz",
		"exerciseid": "12345",
		"topic": "environment",
		"grade": 4
	}
]
```

### videolist

Expected columns:

- `id` (INT IDENTITY, PK)
- `videotitle` (NVARCHAR)
- `youtubeid` (NVARCHAR)
- `topic` (NVARCHAR)
- `language` (CHAR(2), one of `hu`, `en`, `cz`)
- `status` (BIT, 1=enabled)

### exerciselist

Expected columns:

- `id` (INT IDENTITY, PK)
- `exercisetitle` (NVARCHAR)
- `exerciseid` (NVARCHAR)
- `topic` (NVARCHAR)
- `language` (CHAR(2), one of `hu`, `en`, `cz`)
- `status` (BIT, 1=enabled)

## Notes

- The app’s UI culture uses `cs` for Czech, but the database uses `cz`; the code maps `cs` → `cz` when querying.
- Exercise thumbnails are generated from `ExerciseId` (numeric package IDs use OkosDoboz thumbnails).
