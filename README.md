# 🏪 dotnet-minimal-api-sample

![.NET](https://img.shields.io/badge/.NET_10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)

API REST para gestión de ventas (POS) con autenticación JWT, BCrypt y SQL Server.

---

## 📑 Tabla de Contenido

- [🛠️ Tecnologías](#tecnologías)
- [📁 Arquitectura](#arquitectura)
- [📋 Endpoints](#endpoints)
- [⚙️ Configuración](#configuración)
- [🧪 Pruebas](#pruebas)
- [🔒 Seguridad](#seguridad)

---

## 🛠️ Tecnologías

| Tecnología | Propósito |
|------------|-----------|
| .NET 10 Minimal API | Framework principal |
| SQL Server + Stored Procedures | Base de datos |
| JWT Bearer | Autenticación |
| BCrypt | Hash de contraseñas |
| Swagger / OpenAPI | Documentación |
| NLog | Logging |

---

## 📁 Arquitectura

```
📂 EndPoint/     →  Rutas y validación de entrada
📂 Services/     →  Lógica de negocio (con interfaces)
📂 Data/         →  Modelos de dominio
📂 DTO/          →  Objetos de transferencia
📂 DataBase/     →  Schema SQL y Stored Procedures
```

---

## 📋 Endpoints

### 🔐 Autenticación
| Método | Ruta | Auth | Descripción |
|--------|------|:----:|-------------|
| `POST` | `/api/login` | ❌ | Login (retorna JWT) |

### 📦 Productos
| Método | Ruta | Auth | Descripción |
|--------|------|:----:|-------------|
| `GET` | `/api/products` | ✅ | Listar productos |
| `GET` | `/api/product/{code}` | ✅ | Producto por código |
| `POST` | `/api/product` | ✅ | Crear producto |
| `PUT` | `/api/product` | ✅ | Actualizar producto |
| `DELETE` | `/api/product/{code}` | ✅ | Eliminar (baja lógica) |

### 👥 Clientes
| Método | Ruta | Auth | Descripción |
|--------|------|:----:|-------------|
| `GET` | `/api/customers` | ✅ | Listar clientes |
| `GET` | `/api/customer/{documentNumber}` | ✅ | Cliente por documento |
| `POST` | `/api/customer` | ✅ | Crear cliente |
| `PUT` | `/api/customer` | ✅ | Actualizar cliente |
| `DELETE` | `/api/customer/{documentNumber}` | ✅ | Eliminar (baja lógica) |

### 🧾 Ventas
| Método | Ruta | Auth | Descripción |
|--------|------|:----:|-------------|
| `POST` | `/api/sale` | ✅ | Crear venta (transaccional) |
| `GET` | `/api/sales` | ✅ | Listar ventas |
| `GET` | `/api/sales/customer/{documentNumber}` | ✅ | Ventas por cliente |
| `GET` | `/api/sales/invoice/{invoiceNumber}` | ✅ | Venta por factura |
| `DELETE` | `/api/sales/cancel/{invoiceNumber}` | ✅ | Cancelar venta |

---

## ⚙️ Configuración

```bash
# 1. Crear la base de datos
Ejecutar DataBase/Schema/Create_database_schema.sql en SQL Server

# 2. Crear los Stored Procedures
Ejecutar todos los scripts en DataBase/Stored procedures/

# 3. Configurar la connection string
Editar appsettings.json → ConnectionStrings:SQL

# 4. Configurar ruta de logs
Editar nlog.config → buscar COLOCAR_RUTA y reemplazar

# 5. Ejecutar
dotnet run
```

---

## 🧪 Pruebas

### Credenciales de prueba

| Usuario | Contraseña |
|---------|------------|
| Cualquier usuario del script | `pass123` |

> Los usuarios de prueba se crean en el script `DataBase/Schema/Create_database_schema.sql`

---

## 🔒 Seguridad

| Medida | Descripción |
|--------|-------------|
| 🔑 | Contraseñas hasheadas con **BCrypt** |
| 🎫 | Autenticación **JWT (HS256)** |
| 🛡️ | Endpoints protegidos con `[RequireAuthorization]` |
