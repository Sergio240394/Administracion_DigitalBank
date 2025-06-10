# Administracion_DigitalBank

# 👥 Sistema de Gestión de Usuarios

Este proyecto es una aplicación web desarrollada con **ASP.NET Core** y **Razor Pages** que permite gestionar una base de datos de usuarios. La información se almacena en **SQL Server** y se maneja a través de un procedimiento almacenado (`sp_Usuarios`) que centraliza todas las operaciones CRUD (Crear, Leer, Actualizar, Eliminar).

---

## 🧱 Tecnologías utilizadas

- 🗃️ **SQL Server** – Base de datos relacional
- ⚙️ **.NET 8 (ASP.NET Core)** – Backend y frontend integrados
- 💻 **Razor Pages** – Motor de renderizado para UI
- 📦 **Entity Framework Core / ADO.NET** – Acceso a datos

---

## 📁 Estructura del Proyecto

UsuariosApp/
├── UsuariosDB/ ← Scripts SQL
│ └── script.sql ← Creación de base de datos y SP
├── UsuariosApp/ ← Proyecto ASP.NET Core
│ ├── Pages/ ← Razor Pages (.cshtml)
│ ├── Models/ ← Clases de entidad y ViewModels
│ ├── Data/ ← Lógica de acceso a datos
│ └── appsettings.json ← Configuración (cadena de conexión)
├── README.md ← Este archivo


---

## ⚙️ Despliegue del Proyecto

### 1. Crear la base de datos en SQL Server

1. Abre SQL Server Management Studio (SSMS).
2. Ejecuta el script `UsuariosDB/script.sql`, el cual:
   - Crea la base de datos `UsuariosDB`.
   - Crea la tabla `Usuarios`.
   - Crea el procedimiento almacenado `sp_Usuarios` con validaciones.

> Asegúrate de que la base de datos y el procedimiento se hayan creado correctamente.

---

### 2. Configurar la cadena de conexión

1. Abre el archivo `appsettings.json` en el proyecto `UsuariosApp`.
2. Modifica la sección `"ConnectionStrings"` con tu configuración local:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=UsuariosDB;Trusted_Connection=True;"
}
