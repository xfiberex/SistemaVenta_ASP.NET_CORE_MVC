# 🛒 Sistema de Ventas WEB - ASP.NET Core MVC## Sistema de Ventas WEB

![Sistema de Ventas - Dashboard](https://github.com/xfiberex/SistemaVenta_ASP.NET_CORE_MVC/assets/135444565/8c23599c-33cb-4280-b058-30214a7af884)

![Sistema de Ventas - Gestión](https://github.com/xfiberex/SistemaVenta_ASP.NET_CORE_MVC/assets/135444565/b3232fa9-e409-488c-a0cf-ee900c399297)

## Caracteristicas

## 📋 Descripción

- Inicio de Sesión: Los usuarios pueden iniciar sesión con sus credenciales para acceder al sistema.

Sistema integral de gestión de ventas desarrollado con **ASP.NET Core MVC** que permite administrar de manera eficiente un negocio de productos. El sistema implementa una arquitectura en capas con **Clean Architecture** y patrones como **Repository Pattern** e **Inversión de Dependencias**.- Dashboard: Interfaz principal que proporciona una visión general de las métricas clave del sistema.

- Administración de Usuarios: Funcionalidad para gestionar usuarios por correo, incluyendo crear, editar y eliminar usuarios.

### 🎯 Propósito- Administración de Negocio: Herramientas para administrar aspectos del negocio, como la información del mismo.

Este proyecto fue desarrollado como parte de un proceso de aprendizaje en ASP.NET Core MVC, implementando las mejores prácticas de desarrollo y utilizando plantillas modernas para el frontend.- Inventario: Seguimiento de productos disponibles, cantidades, precios, etc.

- Ventas: Proceso de venta de productos, registro de ventas realizadas.

## ✨ Características Principales- Reportes: Generación de informes y análisis de datos relacionados con las ventas y el negocio.



### 🔐 **Autenticación y Seguridad**## Tecnologías

- Sistema de login con autenticación por cookies

- Gestión de roles y permisos### Backend:

- Sesiones con tiempo de expiración configurable (20 minutos)

- C#

### 📊 **Dashboard Ejecutivo**- ASP.NET Core MVC

- Métricas en tiempo real del negocio- SQL Server

- Indicadores de ventas y rendimiento

- Gráficos estadísticos interactivos### Frontend:



### 👥 **Gestión de Usuarios**- HTML

- CRUD completo de usuarios- JavaScript

- Asignación de roles y permisos- CSS

- Gestión de perfiles con fotos

- Activación/desactivación de cuentas## Requisitos del Sistema



### 🏢 **Administración de Negocio**- .NET 8 Core SDK

- Configuración de información empresarial- Visual Studio o Visual Studio Code (para desarrollo)

- Gestión de datos de contacto- SQL Server (para la base de datos)

- Personalización de la marca

## Instalación

### 📦 **Gestión de Inventario**

- Control de productos por categorías1. Clona este repositorio: git clone https://github.com/xfiberex/SistemaVenta_ASP.NET_CORE_MVC.git

- Seguimiento de stock en tiempo real2. Abre el proyecto en Visual Studio o Visual Studio Code.

- Gestión de precios y descripciones3. Configura la cadena de conexión a la base de datos en appsettings.json.

- Carga de imágenes de productos4. Ejecuta las migraciones de la base de datos para crear las tablas necesarias: dotnet ef database update.

5. Ejecuta la aplicación.

### 💰 **Sistema de Ventas**
- Proceso de venta intuitivo y rápido
- Generación automática de facturas
- Control de tipos de documento (Boleta, Factura)
- Registro detallado de transacciones

### 📈 **Reportes y Análisis**
- Generación de reportes en PDF
- Análisis de ventas por período
- Reportes de productos más vendidos
- Exportación de datos

## 🏗️ Arquitectura del Proyecto

El proyecto sigue una **arquitectura en capas** bien definida:

```
📁 SistemaVenta.AplicacionWeb/     # Capa de Presentación (MVC)
├── Controllers/                  # Controladores MVC
├── Views/                       # Vistas Razor
├── Models/ViewModels/           # ViewModels
├── Utilidades/                  # Utilities y Helpers
└── wwwroot/                     # Recursos estáticos

📁 SistemaVenta.BLL/             # Capa de Lógica de Negocio
├── Interfaces/                  # Contratos de servicio
└── Implementacion/              # Implementación de servicios

📁 SistemaVenta.DAL/             # Capa de Acceso a Datos
├── DBContext/                   # Contexto de Entity Framework
├── Interfaces/                  # Contratos del repositorio
└── Implementacion/              # Implementación del repositorio

📁 SistemaVenta.Entity/          # Capa de Entidades
└── *.cs                        # Modelos de dominio

📁 SistemaVenta.IOC/             # Inversión de Control
└── Dependencia.cs               # Configuración de dependencias
```

## 🛠️ Tecnologías y Herramientas

### **Backend**
- ![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=c-sharp&logoColor=white) **C# 12.0**
- ![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet) **ASP.NET Core MVC 8.0**
- ![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-512BD4?style=flat-square) **Entity Framework Core**
- ![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=flat-square&logo=microsoft-sql-server) **SQL Server**

### **Frontend**
- ![HTML5](https://img.shields.io/badge/HTML5-E34F26?style=flat-square&logo=html5&logoColor=white) **HTML5**
- ![CSS3](https://img.shields.io/badge/CSS3-1572B6?style=flat-square&logo=css3&logoColor=white) **CSS3/SCSS**
- ![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?style=flat-square&logo=javascript&logoColor=black) **JavaScript ES6+**
- ![Bootstrap](https://img.shields.io/badge/Bootstrap-7952B3?style=flat-square&logo=bootstrap&logoColor=white) **Bootstrap 5**

### **Librerías y Paquetes**
- **AutoMapper** - Mapeo de objetos
- **DinkToPdf** - Generación de PDFs
- **Cookie Authentication** - Autenticación
- **FireBase** - Almacenamiento de archivos

## 📋 Requisitos del Sistema

### **Para Desarrollo:**
- ![.NET](https://img.shields.io/badge/.NET-8.0%20SDK-512BD4?style=flat-square) **.NET 8.0 SDK** o superior
- ![Visual Studio](https://img.shields.io/badge/Visual%20Studio-2022-5C2D91?style=flat-square&logo=visual-studio) **Visual Studio 2022** o **VS Code**
- ![SQL Server](https://img.shields.io/badge/SQL%20Server-2019+-CC2927?style=flat-square) **SQL Server 2019+** o **SQL Server Express**

### **Para Producción:**
- **Windows Server** o **Linux** con soporte para .NET 8
- **IIS** o **Kestrel** como servidor web
- **SQL Server** para base de datos

## 🚀 Instalación y Configuración

### **1. Clonar el Repositorio**
```bash
git clone https://github.com/xfiberex/SistemaVenta_ASP.NET_CORE_MVC.git
cd SistemaVenta_ASP.NET_CORE_MVC
```

### **2. Configurar Base de Datos**
1. Abrir `appsettings.json` en `SistemaVenta.AplicacionWeb/`
2. Modificar la cadena de conexión:
```json
{
  "ConnectionStrings": {
    "CadenaSQL": "Server=TU_SERVIDOR;Database=DBVENTAWEB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### **3. Restaurar Paquetes**
```bash
dotnet restore
```

### **4. Crear Base de Datos**
- Ejecutar el script SQL ubicado en `Recursos BD y Plantillas/`
- O usar Entity Framework Migrations:
```bash
dotnet ef database update
```

### **5. Ejecutar la Aplicación**
```bash
dotnet run --project SistemaVenta.AplicacionWeb
```

### **6. Acceso al Sistema**
- **URL:** `https://localhost:5001` o `http://localhost:5000`
- **Usuario por defecto:** Configurar en la base de datos inicial

## 📱 Uso del Sistema

### **Panel Principal**
1. Iniciar sesión con credenciales válidas
2. Acceder al dashboard principal
3. Navegar por los diferentes módulos

### **Gestión de Productos**
1. Ir a **Productos** → **Nuevo Producto**
2. Completar información y subir imagen
3. Asignar categoría y configurar stock

### **Realizar Venta**
1. Acceder a **Ventas** → **Nueva Venta**
2. Buscar y agregar productos
3. Completar datos del cliente
4. Generar factura o boleta

## 🤝 Contribuciones

Las contribuciones son bienvenidas. Para contribuir:

1. Fork el proyecto
2. Crear una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo [LICENSE.txt](LICENSE.txt) para más detalles.

## 👨‍💻 Autor

**xfiberex**
- GitHub: [@xfiberex](https://github.com/xfiberex)

## 📞 Soporte

Si tienes alguna pregunta o problema:
1. Revisar la documentación
2. Buscar en los [Issues](https://github.com/xfiberex/SistemaVenta_ASP.NET_CORE_MVC/issues)
3. Crear un nuevo Issue si es necesario

---

⭐ **¡No olvides darle una estrella al proyecto si te resultó útil!**