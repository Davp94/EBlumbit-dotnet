# EBlumbit API

Backend REST de EBlumbit (compras, ventas, inventario, clientes, proveedores y
usuarios) construido con **ASP.NET Core 10**, **Entity Framework Core**,
**PostgreSQL**, autenticación **JWT**, **Serilog** y **Swagger/OpenAPI**.

---

## Requisitos

| Herramienta | Versión | Necesario para |
| --- | --- | --- |
| [.NET SDK](https://dotnet.microsoft.com/download/dotnet/10.0) | 10.x | Desarrollo, publicación y `dotnet ef` |
| [PostgreSQL](https://www.postgresql.org/download/) | 16+ (15+ debería servir) | Base de datos |
| Docker Engine + Docker Compose | v2+ | Ejecución con contenedores |
| `dotnet-ef` (herramienta global) | 10.x | Crear/aplicar migraciones |

Verifica la instalación:

```bash
dotnet --version        # debe empezar con 10.
docker compose version
```

Instala la herramienta de migraciones:

```bash
dotnet tool install --global dotnet-ef --version 10.*
# Si ya está instalada:
dotnet tool update --global dotnet-ef --version 10.*
```

> Si `dotnet ef` no se reconoce, agrega `~/.dotnet/tools` al `PATH`:
> `export PATH="$PATH:$HOME/.dotnet/tools"` (Linux/macOS).

---

## Configuración

La cadena de conexión por defecto en `appsettings.json` es:

```
Host=localhost;Port=5432;Database=eblumbit_db;Username=postgres;Password=123456
```

Puedes sobrescribirla sin tocar el repositorio con variables de entorno
(usa `__` para separar secciones):

```bash
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=eblumbit_db;Username=postgres;Password=123456"
```

O en `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=eblumbit_db;Username=postgres;Password=123456"
  },
  "Storage": {
    "FileDirectory": "Files"
  }
}
```

Variables relevantes:

| Variable / clave | Descripción |
| --- | --- |
| `ConnectionStrings__DefaultConnection` | Conexión a PostgreSQL |
| `Jwt__Key` | Clave secreta para firmar los JWT (**cámbiala en producción**) |
| `Jwt__Issuer` / `Jwt__Audience` | Emisor y audiencia del token |
| `Jwt__DurationInMinutes` | Duración del access token (15 por defecto) |
| `Storage__FileDirectory` | Carpeta donde se guardan archivos subidos (`Files` por defecto en Docker) |
| `Cors__AllowedOrigins__0`, `...__1`, ... | Orígenes permitidos para el frontend |

> `appsettings.json` incluye una ruta absoluta de `Storage:FileDirectory`
> pensada para una máquina concreta. En desarrollo usa `"Files"` (relativa al
> directorio del proyecto) o una ruta propia.

---

## Desarrollo

### 1. Instalar y arrancar PostgreSQL

**Ubuntu / Debian**

```bash
sudo apt update
sudo apt install -y postgresql postgresql-contrib
sudo systemctl enable --now postgresql
```

**macOS (Homebrew)**

```bash
brew install postgresql@16
brew services start postgresql@16
```

**Windows**

```powershell
winget install PostgreSQL.PostgreSQL.16
# o descarga el instalador desde https://www.postgresql.org/download/windows/
```

### 2. Crear la base de datos

```bash
sudo -u postgres psql
```

```sql
CREATE DATABASE eblumbit_db;
ALTER USER postgres WITH PASSWORD '123456';
\q
```

En macOS/Windows normalmente el superusuario es tu usuario del sistema y basta:

```bash
createdb eblumbit_db
```

### 3. Instalar dependencias y aplicar migraciones

Desde la raíz del proyecto:

```bash
dotnet restore
dotnet ef database update
```

`dotnet ef database update` crea el esquema. **No se ejecuta automáticamente**:
sin este paso la app falla al arrancar porque los *seeders* intentan escribir
en tablas que aún no existen.

### 4. Ejecutar

```bash
dotnet run --launch-profile http
```

La API queda disponible en:

- API: <http://localhost:5071>
- Swagger UI: <http://localhost:5071/swagger>
- OpenAPI JSON (solo Development): <http://localhost:5071/openapi/v1.json>

Con el perfil `https`: <https://localhost:7084> (y HTTP en 5071).

### Datos iniciales (seed)

Al arrancar, la aplicación ejecuta `DataSeeder` e inserta (si no existen):

- Permisos, roles `ADMIN`, `VENDEDOR`, `RRHH` y sus permisos.
- Categorías, sucursales, almacenes, productos, inventario, clientes y proveedores.
- **10 usuarios aleatorios** (Bogus) con la contraseña `Password123*`.

Los usuarios sembrados **no tienen rol asignado**. Para consultarlos y asignarles
roles:

```bash
psql -U postgres -d eblumbit_db -c 'SELECT id, email, name FROM users ORDER BY id LIMIT 10;'
psql -U postgres -d eblumbit_db -f assign_roles_permissions.sql
```

`assign_roles_permissions.sql` asigna ADMIN a los usuarios 1–4, VENDEDOR a 5–8
y RRHH a 9–10 (y todos los permisos al rol ADMIN).

### Login

```bash
curl -X POST http://localhost:5071/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"usuario@ejemplo.com","password":"Password123*"}'
```

También se acepta `username` en lugar de `email`.

---

## Producción (sin Docker)

### 1. Publicar

```bash
dotnet publish EBlumbit.csproj -c Release -o ./publish
```

### 2. Aplicar migraciones contra la base de producción

```bash
export ConnectionStrings__DefaultConnection="Host=DB_HOST;Port=5432;Database=eblumbit_db;Username=postgres;Password=TU_PASSWORD"
dotnet ef database update
```

### 3. Configurar variables y arrancar

```bash
export ASPNETCORE_ENVIRONMENT=Production
export ASPNETCORE_URLS="http://0.0.0.0:5071"
export ConnectionStrings__DefaultConnection="Host=DB_HOST;Port=5432;Database=eblumbit_db;Username=postgres;Password=TU_PASSWORD"
export Jwt__Key="UNA_CLAVE_LARGA_Y_SECRETA"
export Storage__FileDirectory="/var/lib/eblumbit/files"
export Cors__AllowedOrigins__0="https://tu-frontend.com"

dotnet ./publish/EBlumbit.dll
```

Notas para producción:

- Cambia `Jwt:Key` (la del repositorio es solo para desarrollo).
- La app llama a `UseHttpsRedirection()`. Si no configuras HTTPS, ejecútala
  detrás de un reverse proxy (Nginx, Caddy, Traefik) que termine TLS, o define
  `ASPNETCORE_HTTPS_PORTS`/certificado. Con solo HTTP el redirect se omite.
- Los logs se escriben en `logs/app.log` (relativo al directorio de contenido)
  y también a consola (Serilog).
- La carpeta de `Storage:FileDirectory` debe existir o ser escribible.
- Las migraciones se aplican manualmente; no las ejecuta el arranque.

---

## Docker / Docker Compose

El proyecto incluye `Dockerfile` (multi-stage, .NET 10) y `docker-compose.yml`
con PostgreSQL 16, la API y un servicio opcional para migraciones.

### Arranque recomendado

```bash
# 1. Levantar primero la base de datos
docker compose up -d db

# 2. Aplicar migraciones (servicio "migrate", perfil tools)
docker compose --profile tools run --rm migrate

# 3. Construir y levantar la API
docker compose up -d --build
```

La API queda en <http://localhost:5071/swagger>.

### Comandos útiles

```bash
docker compose logs -f app        # logs de la API
docker compose ps                 # estado de los servicios
docker compose down               # detener (conserva datos)
docker compose down -v            # detener y borrar volúmenes (base de datos)
docker compose up -d --build      # reconstruir tras cambios de código
```

### Variables de entorno (`.env`)

Crea un archivo `.env` en la raíz para personalizar los valores; Docker Compose
lo lee automáticamente:

```dotenv
POSTGRES_DB=eblumbit_db
POSTGRES_USER=postgres
POSTGRES_PASSWORD=cambia_esta_password
POSTGRES_PORT=5432
APP_PORT=5071
ASPNETCORE_ENVIRONMENT=Production
JWT_KEY=una_clave_larga_y_secreta
JWT_ISSUER=EblumbitIssuer
JWT_AUDIENCE=EblumbitAudience
```

Volúmenes creados:

| Volumen | Contenido |
| --- | --- |
| `postgres-data` | Datos de PostgreSQL |
| `app-files` | Archivos subidos (`/app/files`) |
| `app-logs` | Logs de Serilog (`/app/logs`) |

### Solo Docker (sin Compose)

```bash
docker build -t eblumbit-api .
docker network create eblumbit-net

docker run -d --name eblumbit-db --network eblumbit-net \
  -e POSTGRES_DB=eblumbit_db -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=123456 postgres:16-alpine

# Aplicar migraciones desde el host apuntando a localhost:5432
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=eblumbit_db;Username=postgres;Password=123456"
dotnet ef database update

docker run -d --name eblumbit-api --network eblumbit-net \
  -p 5071:5071 \
  -e ConnectionStrings__DefaultConnection="Host=eblumbit-db;Port=5432;Database=eblumbit_db;Username=postgres;Password=123456" \
  -e Jwt__Key="una_clave_larga_y_secreta" \
  -e Storage__FileDirectory=/app/files \
  -v eblumbit-files:/app/files \
  eblumbit-api
```

---

## Estructura del proyecto

```
EBlumbit/
├── Authorization/     # Permisos y policy handler
├── Builders/          # Construcción de respuestas/DTOs
├── Config/            # Configuración adicional
├── Controllers/       # Endpoints REST
├── Data/              # AppDbContext (EF Core)
├── Dto/               # Contratos de entrada/salida
├── exceptions/        # Excepciones y manejador global
├── Migrations/        # Migraciones de EF Core
├── Models/            # Entidades
├── Repository/        # Acceso a datos (EF Core + Dapper)
├── Seeders/           # Datos iniciales
├── Services/          # Lógica de negocio
├── Files/             # Archivos subidos (por defecto)
├── Program.cs         # Punto de entrada y configuración
├── Dockerfile
└── docker-compose.yml
```

---

## Solución de problemas

| Síntoma | Causa / solución |
| --- | --- |
| `relation "..." does not exist` al arrancar | Faltan migraciones: `dotnet ef database update` |
| `dotnet ef` no encontrado | `dotnet tool install --global dotnet-ef --version 10.*` y agrega `~/.dotnet/tools` al `PATH` |
| `Connection refused` a PostgreSQL | El servicio no está activo o el host/puerto no coincide |
| `password authentication failed` | Ajusta `POSTGRES_PASSWORD` / cadena de conexión |
| Error al subir archivos | `Storage:FileDirectory` no existe o no es escribible |
| La API no responde tras `docker compose up` | Revisa `docker compose logs app`; probablemente falten migraciones |
| CORS bloqueado | Agrega el origen del frontend en `Cors:AllowedOrigins` |
