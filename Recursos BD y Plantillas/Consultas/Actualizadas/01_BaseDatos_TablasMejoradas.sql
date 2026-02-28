-- ============================================================
--  BASE DE DATOS: DBVENTAWEB
--  Archivo: 01_BaseDatos_TablasMejoradas.sql
--  Descripción: Creación de base de datos y tablas mejoradas
--  Mejoras:
--    - NVARCHAR en lugar de VARCHAR (soporte Unicode / tildes)
--    - Restricciones NOT NULL explícitas
--    - Constraints UNIQUE donde corresponde
--    - Índices sobre claves foráneas para rendimiento
--    - Checks de existencia antes de crear (idempotente)
--    - CHECK constraints para valores lógicos
--    - Longitudes de columna revisadas y ajustadas
-- ============================================================

-- ─────────────────────────────────────────────────
--  1. CREAR BASE DE DATOS
-- ─────────────────────────────────────────────────
-- =====================================================================
-- RECREACIÓN COMPLETA DE LA BASE DE DATOS DB_TDApp
-- Elimina la BD si existe y la vuelve a crear desde cero
-- =====================================================================
-- Cierra todas las conexiones activas antes de eliminar
USE master;
GO

IF DB_ID(N'DBVENTAWEB') IS NOT NULL
BEGIN
    ALTER DATABASE DBVENTAWEB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE DBVENTAWEB;
END
GO

CREATE DATABASE DBVENTAWEB;
GO
USE DBVENTAWEB;
GO

SET NOCOUNT ON;
GO

-- ─────────────────────────────────────────────────
--  2. TABLA: Menu
--  Estructura de menú jerárquico (padre/hijo)
-- ─────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Menu]') AND type = 'U')
BEGIN
    CREATE TABLE Menu (
        idMenu          INT             NOT NULL IDENTITY(1,1),
        descripcion     NVARCHAR(50)    NOT NULL,
        idMenuPadre     INT             NULL        REFERENCES Menu(idMenu),
        icono           NVARCHAR(50)    NULL,
        controlador     NVARCHAR(50)    NULL,
        paginaAccion    NVARCHAR(50)    NULL,
        esActivo        BIT             NOT NULL DEFAULT 1,
        fechaRegistro   DATETIME        NOT NULL DEFAULT GETDATE(),

        CONSTRAINT PK_Menu PRIMARY KEY (idMenu)
    );
END
GO

-- ─────────────────────────────────────────────────
--  3. TABLA: Rol
-- ─────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rol]') AND type = 'U')
BEGIN
    CREATE TABLE Rol (
        idRol           INT             NOT NULL IDENTITY(1,1),
        descripcion     NVARCHAR(30)    NOT NULL,
        esActivo        BIT             NOT NULL DEFAULT 1,
        fechaRegistro   DATETIME        NOT NULL DEFAULT GETDATE(),

        CONSTRAINT PK_Rol PRIMARY KEY (idRol),
        CONSTRAINT UQ_Rol_Descripcion UNIQUE (descripcion)
    );
END
GO

-- ─────────────────────────────────────────────────
--  4. TABLA: RolMenu
--  Relación muchos a muchos entre Rol y Menu
-- ─────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RolMenu]') AND type = 'U')
BEGIN
    CREATE TABLE RolMenu (
        idRolMenu       INT             NOT NULL IDENTITY(1,1),
        idRol           INT             NOT NULL REFERENCES Rol(idRol),
        idMenu          INT             NOT NULL REFERENCES Menu(idMenu),
        esActivo        BIT             NOT NULL DEFAULT 1,
        fechaRegistro   DATETIME        NOT NULL DEFAULT GETDATE(),

        CONSTRAINT PK_RolMenu       PRIMARY KEY (idRolMenu),
        CONSTRAINT UQ_RolMenu_Combo UNIQUE (idRol, idMenu)  -- evita duplicados
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_RolMenu_idRol' AND object_id = OBJECT_ID(N'dbo.RolMenu'))
    CREATE INDEX IX_RolMenu_idRol ON RolMenu(idRol);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_RolMenu_idMenu' AND object_id = OBJECT_ID(N'dbo.RolMenu'))
    CREATE INDEX IX_RolMenu_idMenu ON RolMenu(idMenu);
GO

-- ─────────────────────────────────────────────────
--  5. TABLA: Usuario
-- ─────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usuario]') AND type = 'U')
BEGIN
    CREATE TABLE Usuario (
        idUsuario       INT             NOT NULL IDENTITY(1,1),
        nombre          NVARCHAR(100)   NOT NULL,
        correo          NVARCHAR(100)   NOT NULL,   -- único por negocio
        telefono        NVARCHAR(20)    NULL,
        idRol           INT             NOT NULL REFERENCES Rol(idRol),
        urlFoto         NVARCHAR(500)   NULL,
        nombreFoto      NVARCHAR(100)   NULL,
        clave           NVARCHAR(200)   NOT NULL,   -- hash SHA-256 (64 hex) o bcrypt
        esActivo        BIT             NOT NULL DEFAULT 1,
        fechaRegistro   DATETIME        NOT NULL DEFAULT GETDATE(),

        CONSTRAINT PK_Usuario       PRIMARY KEY (idUsuario),
        CONSTRAINT UQ_Usuario_Correo UNIQUE (correo)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Usuario_idRol' AND object_id = OBJECT_ID(N'dbo.Usuario'))
    CREATE INDEX IX_Usuario_idRol ON Usuario(idRol);
GO

-- ─────────────────────────────────────────────────
--  6. TABLA: Categoria
-- ─────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categoria]') AND type = 'U')
BEGIN
    CREATE TABLE Categoria (
        idCategoria     INT             NOT NULL IDENTITY(1,1),
        descripcion     NVARCHAR(100)   NOT NULL,
        esActivo        BIT             NOT NULL DEFAULT 1,
        fechaRegistro   DATETIME        NOT NULL DEFAULT GETDATE(),

        CONSTRAINT PK_Categoria             PRIMARY KEY (idCategoria),
        CONSTRAINT UQ_Categoria_Descripcion UNIQUE (descripcion)
    );
END
GO

-- ─────────────────────────────────────────────────
--  7. TABLA: Producto
-- ─────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Producto]') AND type = 'U')
BEGIN
    CREATE TABLE Producto (
        idProducto      INT             NOT NULL IDENTITY(1,1),
        codigoBarra     NVARCHAR(50)    NOT NULL,
        marca           NVARCHAR(100)   NOT NULL,
        descripcion     NVARCHAR(200)   NOT NULL,
        idCategoria     INT             NOT NULL REFERENCES Categoria(idCategoria),
        stock           INT             NOT NULL DEFAULT 0,
        urlImagen       NVARCHAR(500)   NULL,
        nombreImagen    NVARCHAR(100)   NULL,
        precio          DECIMAL(10,2)   NOT NULL DEFAULT 0.00,
        esActivo        BIT             NOT NULL DEFAULT 1,
        fechaRegistro   DATETIME        NOT NULL DEFAULT GETDATE(),

        CONSTRAINT PK_Producto              PRIMARY KEY (idProducto),
        CONSTRAINT UQ_Producto_CodigoBarra  UNIQUE (codigoBarra),
        CONSTRAINT CK_Producto_Stock        CHECK (stock >= 0),
        CONSTRAINT CK_Producto_Precio       CHECK (precio >= 0)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Producto_idCategoria' AND object_id = OBJECT_ID(N'dbo.Producto'))
    CREATE INDEX IX_Producto_idCategoria ON Producto(idCategoria);
GO

-- ─────────────────────────────────────────────────
--  8. TABLA: NumeroCorrelativo
--  Generador de numeración para documentos
-- ─────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NumeroCorrelativo]') AND type = 'U')
BEGIN
    CREATE TABLE NumeroCorrelativo (
        idNumeroCorrelativo INT             NOT NULL IDENTITY(1,1),
        ultimoNumero        INT             NOT NULL DEFAULT 0,
        cantidadDigitos     INT             NOT NULL DEFAULT 6,
        gestion             NVARCHAR(100)   NOT NULL,
        fechaActualizacion  DATETIME        NOT NULL DEFAULT GETDATE(),

        CONSTRAINT PK_NumeroCorrelativo     PRIMARY KEY (idNumeroCorrelativo),
        CONSTRAINT UQ_NumeroCorrelativo_Gestion UNIQUE (gestion),
        CONSTRAINT CK_NumeroCorrelativo_Num CHECK (ultimoNumero >= 0),
        CONSTRAINT CK_NumeroCorrelativo_Dig CHECK (cantidadDigitos > 0)
    );
END
GO

-- ─────────────────────────────────────────────────
--  9. TABLA: TipoDocumentoVenta
-- ─────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TipoDocumentoVenta]') AND type = 'U')
BEGIN
    CREATE TABLE TipoDocumentoVenta (
        idTipoDocumentoVenta    INT             NOT NULL IDENTITY(1,1),
        descripcion             NVARCHAR(50)    NOT NULL,
        esActivo                BIT             NOT NULL DEFAULT 1,
        fechaRegistro           DATETIME        NOT NULL DEFAULT GETDATE(),

        CONSTRAINT PK_TipoDocumentoVenta             PRIMARY KEY (idTipoDocumentoVenta),
        CONSTRAINT UQ_TipoDocumentoVenta_Descripcion UNIQUE (descripcion)
    );
END
GO

-- ─────────────────────────────────────────────────
--  10. TABLA: Venta
-- ─────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Venta]') AND type = 'U')
BEGIN
    CREATE TABLE Venta (
        idVenta                 INT             NOT NULL IDENTITY(1,1),
        numeroVenta             NVARCHAR(10)    NOT NULL,   -- ampliado a 10 para mayor rango
        idTipoDocumentoVenta    INT             NOT NULL REFERENCES TipoDocumentoVenta(idTipoDocumentoVenta),
        idUsuario               INT             NOT NULL REFERENCES Usuario(idUsuario),
        documentoCliente        NVARCHAR(20)    NULL,
        nombreCliente           NVARCHAR(100)   NULL,
        subTotal                DECIMAL(10,2)   NOT NULL DEFAULT 0.00,
        impuestoTotal           DECIMAL(10,2)   NOT NULL DEFAULT 0.00,
        total                   DECIMAL(10,2)   NOT NULL DEFAULT 0.00,
        fechaRegistro           DATETIME        NOT NULL DEFAULT GETDATE(),

        CONSTRAINT PK_Venta             PRIMARY KEY (idVenta),
        CONSTRAINT UQ_Venta_NumeroVenta UNIQUE (numeroVenta),
        CONSTRAINT CK_Venta_SubTotal    CHECK (subTotal >= 0),
        CONSTRAINT CK_Venta_Impuesto    CHECK (impuestoTotal >= 0),
        CONSTRAINT CK_Venta_Total       CHECK (total >= 0)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Venta_idTipoDocumento' AND object_id = OBJECT_ID(N'dbo.Venta'))
    CREATE INDEX IX_Venta_idTipoDocumento ON Venta(idTipoDocumentoVenta);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Venta_idUsuario' AND object_id = OBJECT_ID(N'dbo.Venta'))
    CREATE INDEX IX_Venta_idUsuario ON Venta(idUsuario);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Venta_FechaRegistro' AND object_id = OBJECT_ID(N'dbo.Venta'))
    CREATE INDEX IX_Venta_FechaRegistro ON Venta(fechaRegistro);
GO

-- ─────────────────────────────────────────────────
--  11. TABLA: DetalleVenta
--  Snapshot de datos del producto al momento de venta
-- ─────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DetalleVenta]') AND type = 'U')
BEGIN
    CREATE TABLE DetalleVenta (
        idDetalleVenta          INT             NOT NULL IDENTITY(1,1),
        idVenta                 INT             NOT NULL REFERENCES Venta(idVenta),
        idProducto              INT             NOT NULL,   -- sin FK: snapshot histórico
        marcaProducto           NVARCHAR(100)   NOT NULL,
        descripcionProducto     NVARCHAR(200)   NOT NULL,
        categoriaProducto       NVARCHAR(100)   NOT NULL,
        cantidad                INT             NOT NULL,
        precio                  DECIMAL(10,2)   NOT NULL,
        total                   DECIMAL(10,2)   NOT NULL,

        CONSTRAINT PK_DetalleVenta      PRIMARY KEY (idDetalleVenta),
        CONSTRAINT CK_DetalleVenta_Cant CHECK (cantidad > 0),
        CONSTRAINT CK_DetalleVenta_Prec CHECK (precio >= 0),
        CONSTRAINT CK_DetalleVenta_Tot  CHECK (total >= 0)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_DetalleVenta_idVenta' AND object_id = OBJECT_ID(N'dbo.DetalleVenta'))
    CREATE INDEX IX_DetalleVenta_idVenta ON DetalleVenta(idVenta);
GO

-- ─────────────────────────────────────────────────
--  12. TABLA: Negocio
--  Registro único de configuración del negocio
-- ─────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Negocio]') AND type = 'U')
BEGIN
    CREATE TABLE Negocio (
        idNegocio           INT             NOT NULL,   -- PK manual: solo existe 1 fila
        urlLogo             NVARCHAR(500)   NULL,
        nombreLogo          NVARCHAR(100)   NULL,
        numeroDocumento     NVARCHAR(50)    NULL,
        nombre              NVARCHAR(100)   NULL,
        correo              NVARCHAR(100)   NULL,
        direccion           NVARCHAR(200)   NULL,
        telefono            NVARCHAR(20)    NULL,
        porcentajeImpuesto  DECIMAL(5,2)    NOT NULL DEFAULT 0.00,
        simboloMoneda       NVARCHAR(5)     NOT NULL DEFAULT N'$',

        CONSTRAINT PK_Negocio           PRIMARY KEY (idNegocio),
        CONSTRAINT CK_Negocio_Impuesto  CHECK (porcentajeImpuesto >= 0 AND porcentajeImpuesto <= 100)
    );
END
GO

-- ─────────────────────────────────────────────────
--  13. TABLA: Configuracion
--  Pares clave/valor para parámetros de sistema
-- ─────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Configuracion]') AND type = 'U')
BEGIN
    CREATE TABLE Configuracion (
        recurso     NVARCHAR(50)    NOT NULL,
        propiedad   NVARCHAR(50)    NOT NULL,
        valor       NVARCHAR(200)   NULL,

        CONSTRAINT PK_Configuracion PRIMARY KEY (recurso, propiedad)
    );
END
GO
