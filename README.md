# 🛒 Sistema de Ventas WEB - ASP.NET Core MVC

![Sistema de Ventas - Dashboard](https://github.com/xfiberex/SistemaVenta_ASP.NET_CORE_MVC/assets/135444565/8c23599c-33cb-4280-b058-30214a7af884)

![Sistema de Ventas - Gestión](https://github.com/xfiberex/SistemaVenta_ASP.NET_CORE_MVC/assets/135444565/b3232fa9-e409-488c-a0cf-ee900c399297)

## 📋 Descripción

Aplicación web para gestión de ventas, inventario y administración de negocio, desarrollada con **ASP.NET Core MVC (.NET 8)** y organizada en una arquitectura por capas:

- **Presentación:** `SistemaVenta.AplicacionWeb`
- **Lógica de negocio:** `SistemaVenta.BLL`
- **Acceso a datos:** `SistemaVenta.DAL`
- **Entidades:** `SistemaVenta.Entity`
- **Inyección de dependencias:** `SistemaVenta.IOC`

El proyecto usa autenticación por cookies, permisos por rol, generación de PDF para comprobantes, y carga de archivos (usuarios/productos/logo) mediante Firebase Storage.

## ✨ Funcionalidades

- 🔐 **Autenticación y autorización** (login por cookies y vistas protegidas con `[Authorize]`)
- 📊 **Dashboard** con indicadores de ventas
- 👥 **Gestión de usuarios** (altas, edición, estado, foto)
- 🏢 **Gestión del negocio** (datos generales e identidad visual)
- 📦 **Inventario** (categorías, productos, stock, precio)
- 💰 **Ventas** (nueva venta, historial, detalle)
- 🧾 **Comprobantes PDF** de venta (DinkToPdf + plantilla Razor)
- 📈 **Reporte de ventas por rango de fechas**
- 📧 **Restablecimiento de clave por correo** (SMTP)

## 🏗️ Estructura de la solución

```text
SolucionSistemaVenta.sln
│
├─ SistemaVenta.AplicacionWeb/   -> MVC (Controllers, Views, ViewModels, wwwroot)
├─ SistemaVenta.BLL/             -> Servicios de negocio
├─ SistemaVenta.DAL/             -> Repositorios + DBContext
├─ SistemaVenta.Entity/          -> Entidades de dominio
├─ SistemaVenta.IOC/             -> Registro de dependencias
└─ Recursos BD y Plantillas/     -> Scripts SQL y plantillas de apoyo
```

## 🛠️ Stack tecnológico

### Backend
- **.NET 8 / ASP.NET Core MVC**
- **Entity Framework Core** (DB First con `DBVENTAContext`)
- **SQL Server**
- **AutoMapper**
- **DinkToPdf**
- **FirebaseAuthentication.net / FirebaseStorage.net**

### Frontend
- **Razor Views**
- **Bootstrap** (plantilla SB Admin 2)
- **jQuery**
- **DataTables**
- **Chart.js**
- **Select2**
- **Toastr / SweetAlert**

## 📋 Requisitos

- **.NET SDK 8.0+**
- **SQL Server** (Express/Developer/Standard)
- **Visual Studio 2022** o **VS Code**

## 🚀 Puesta en marcha

### 1) Clonar repositorio

```bash
git clone https://github.com/xfiberex/SistemaVenta_ASP.NET_CORE_MVC.git
cd SistemaVenta_ASP.NET_CORE_MVC
```

### 2) Crear base de datos y estructura

Ejecuta, en este orden, los scripts:

1. `Recursos BD y Plantillas/Consultas/001_BaseDatos_Tablas.sql`
2. `Recursos BD y Plantillas/Consultas/002_Inserts.sql`

> Nota: Este repositorio ya incluye scripts SQL completos para estructura y datos iniciales.

### 3) Configurar conexión a SQL Server

Edita `SistemaVenta.AplicacionWeb/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "CadenaSQL": "Server=TU_SERVIDOR;Database=DBVENTAWEB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 4) Configurar servicios externos (opcional pero recomendado)

La app toma credenciales desde la tabla `Configuracion`:

- Recurso `FireBase_Storage`: email, clave, ruta, api_key, carpetas
- Recurso `Servicio_Correo`: correo, clave, alias, host, puerto

El script `002_Inserts.sql` crea estas claves, pero debes completar sus valores.

### 5) Restaurar y ejecutar

```bash
dotnet restore
dotnet run --project SistemaVenta.AplicacionWeb
```

En desarrollo, la URL por defecto suele iniciar en:

- `http://localhost:5152`

La ruta inicial configurada es:

- `Acceso/Login`

## 🔐 Seguridad y acceso

- Autenticación con **Cookie Authentication**
- Expiración de sesión configurada en **20 minutos**
- Menú dinámico por rol (`Rol`, `Menu`, `RolMenu`)

## 🧾 Base de datos (resumen)

Tablas principales:

- `Usuario`, `Rol`, `Menu`, `RolMenu`
- `Categoria`, `Producto`
- `Venta`, `DetalleVenta`, `TipoDocumentoVenta`, `NumeroCorrelativo`
- `Negocio`, `Configuracion`

## 🤝 Contribuciones

Las contribuciones son bienvenidas:

1. Haz un fork del proyecto
2. Crea una rama: `git checkout -b feature/mi-feature`
3. Realiza tus cambios y commits
4. Publica tu rama y abre un Pull Request

## 📄 Licencia

Este proyecto está bajo licencia MIT. Revisa [LICENSE.txt](LICENSE.txt).

## 👨‍💻 Autor

**xfiberex**

- GitHub: [@xfiberex](https://github.com/xfiberex)

## 📞 Soporte

Si encuentras un problema o tienes dudas:

1. Revisa la documentación del repositorio
2. Busca en los [Issues](https://github.com/xfiberex/SistemaVenta_ASP.NET_CORE_MVC/issues)
3. Abre un nuevo Issue con el detalle

---

⭐ Si este proyecto te fue útil, considera darle una estrella.