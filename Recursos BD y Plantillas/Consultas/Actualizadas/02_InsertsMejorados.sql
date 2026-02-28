-- ============================================================
--  BASE DE DATOS: DBVENTAWEB
--  Archivo: 02_InsertsMejorados.sql
--  Descripción: Datos iniciales del sistema
--  Mejoras:
--    - Transacciones con TRY/CATCH en cada bloque
--    - Literales NVARCHAR (N'...') para soporte de tildes
--    - Verificación IF NOT EXISTS antes de cada INSERT
--    - Eliminación de comandos comentados innecesarios
--    - Separación clara por sección con comentarios
-- ============================================================

IF DB_ID(N'DBVENTAWEB') IS NULL
BEGIN
    THROW 50001, 'La base de datos DBVENTAWEB no existe. Ejecuta primero 01_BaseDatos_TablasMejoradas.sql.', 1;
END
GO

USE DBVENTAWEB;
GO

-- ─────────────────────────────────────────────────
--  1. ROLES
-- ─────────────────────────────────────────────────
BEGIN TRY
    BEGIN TRANSACTION;

    IF NOT EXISTS (SELECT 1 FROM Rol WHERE descripcion = N'Administrador')
        INSERT INTO Rol (descripcion, esActivo) VALUES (N'Administrador', 1);

    IF NOT EXISTS (SELECT 1 FROM Rol WHERE descripcion = N'Empleado')
        INSERT INTO Rol (descripcion, esActivo) VALUES (N'Empleado', 1);

    IF NOT EXISTS (SELECT 1 FROM Rol WHERE descripcion = N'Supervisor')
        INSERT INTO Rol (descripcion, esActivo) VALUES (N'Supervisor', 1);

    COMMIT TRANSACTION;
    PRINT 'Roles insertados correctamente.';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Error al insertar Roles: ' + ERROR_MESSAGE();
END CATCH
GO

-- ─────────────────────────────────────────────────
--  2. USUARIO ADMINISTRADOR
--  Clave en texto plano: 123
--  Hash SHA-256 almacenado:
--    a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3
--
--  NOTA: En producción usar bcrypt (costo 10+).
--        Sustituir este valor por el hash generado
--        desde la aplicación.
-- ─────────────────────────────────────────────────
BEGIN TRY
    BEGIN TRANSACTION;

    IF NOT EXISTS (SELECT 1 FROM Usuario WHERE correo = N'useradmin@gmail.com')
    BEGIN
        DECLARE @idRolAdmin INT = (SELECT idRol FROM Rol WHERE descripcion = N'Administrador');

        INSERT INTO Usuario (nombre, correo, telefono, idRol, urlFoto, nombreFoto, clave, esActivo)
        VALUES (
            N'User Admin',
            N'useradmin@gmail.com',
            N'000-000-0000',
            @idRolAdmin,
            N'',
            N'',
            N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3',
            1
        );
    END

    COMMIT TRANSACTION;
    PRINT 'Usuario Administrador insertado correctamente.';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Error al insertar Usuario Administrador: ' + ERROR_MESSAGE();
END CATCH
GO

-- ─────────────────────────────────────────────────
--  3. CONFIGURACIÓN: FireBase Storage y Correo
--
--  IMPORTANTE: Reemplaza los valores vacíos con tus
--  propias credenciales antes de ejecutar en producción.
-- ─────────────────────────────────────────────────
BEGIN TRY
    BEGIN TRANSACTION;

    -- Firebase Storage
    IF NOT EXISTS (SELECT 1 FROM Configuracion WHERE recurso = N'FireBase_Storage' AND propiedad = N'email')
        INSERT INTO Configuracion (recurso, propiedad, valor) VALUES (N'FireBase_Storage', N'email', N'');

    IF NOT EXISTS (SELECT 1 FROM Configuracion WHERE recurso = N'FireBase_Storage' AND propiedad = N'clave')
        INSERT INTO Configuracion (recurso, propiedad, valor) VALUES (N'FireBase_Storage', N'clave', N'');

    IF NOT EXISTS (SELECT 1 FROM Configuracion WHERE recurso = N'FireBase_Storage' AND propiedad = N'ruta')
        INSERT INTO Configuracion (recurso, propiedad, valor) VALUES (N'FireBase_Storage', N'ruta', N'');

    IF NOT EXISTS (SELECT 1 FROM Configuracion WHERE recurso = N'FireBase_Storage' AND propiedad = N'api_key')
        INSERT INTO Configuracion (recurso, propiedad, valor) VALUES (N'FireBase_Storage', N'api_key', N'');

    IF NOT EXISTS (SELECT 1 FROM Configuracion WHERE recurso = N'FireBase_Storage' AND propiedad = N'carpeta_usuario')
        INSERT INTO Configuracion (recurso, propiedad, valor) VALUES (N'FireBase_Storage', N'carpeta_usuario', N'IMAGENES_USUARIO');

    IF NOT EXISTS (SELECT 1 FROM Configuracion WHERE recurso = N'FireBase_Storage' AND propiedad = N'carpeta_producto')
        INSERT INTO Configuracion (recurso, propiedad, valor) VALUES (N'FireBase_Storage', N'carpeta_producto', N'IMAGENES_PRODUCTO');

    IF NOT EXISTS (SELECT 1 FROM Configuracion WHERE recurso = N'FireBase_Storage' AND propiedad = N'carpeta_logo')
        INSERT INTO Configuracion (recurso, propiedad, valor) VALUES (N'FireBase_Storage', N'carpeta_logo', N'IMAGENES_LOGO');

    -- Servicio de Correo
    IF NOT EXISTS (SELECT 1 FROM Configuracion WHERE recurso = N'Servicio_Correo' AND propiedad = N'correo')
        INSERT INTO Configuracion (recurso, propiedad, valor) VALUES (N'Servicio_Correo', N'correo', N'');

    IF NOT EXISTS (SELECT 1 FROM Configuracion WHERE recurso = N'Servicio_Correo' AND propiedad = N'clave')
        INSERT INTO Configuracion (recurso, propiedad, valor) VALUES (N'Servicio_Correo', N'clave', N'');

    IF NOT EXISTS (SELECT 1 FROM Configuracion WHERE recurso = N'Servicio_Correo' AND propiedad = N'alias')
        INSERT INTO Configuracion (recurso, propiedad, valor) VALUES (N'Servicio_Correo', N'alias', N'MiTienda.com');

    IF NOT EXISTS (SELECT 1 FROM Configuracion WHERE recurso = N'Servicio_Correo' AND propiedad = N'host')
        INSERT INTO Configuracion (recurso, propiedad, valor) VALUES (N'Servicio_Correo', N'host', N'smtp.gmail.com');

    IF NOT EXISTS (SELECT 1 FROM Configuracion WHERE recurso = N'Servicio_Correo' AND propiedad = N'puerto')
        INSERT INTO Configuracion (recurso, propiedad, valor) VALUES (N'Servicio_Correo', N'puerto', N'587');

    COMMIT TRANSACTION;
    PRINT 'Configuración insertada correctamente.';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Error al insertar Configuración: ' + ERROR_MESSAGE();
END CATCH
GO

-- ─────────────────────────────────────────────────
--  4. NEGOCIO
--  Registro único (idNegocio = 1).
--  Completa los campos con los datos reales de tu negocio.
-- ─────────────────────────────────────────────────
BEGIN TRY
    BEGIN TRANSACTION;

    IF NOT EXISTS (SELECT 1 FROM Negocio WHERE idNegocio = 1)
    BEGIN
        INSERT INTO Negocio (
            idNegocio, urlLogo, nombreLogo, numeroDocumento,
            nombre, correo, direccion, telefono,
            porcentajeImpuesto, simboloMoneda
        )
        VALUES (
            1, N'', N'', N'',
            N'', N'', N'', N'',
            0.00, N'$'
        );
    END

    COMMIT TRANSACTION;
    PRINT 'Negocio insertado correctamente.';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Error al insertar Negocio: ' + ERROR_MESSAGE();
END CATCH
GO

-- ─────────────────────────────────────────────────
--  5. CATEGORÍAS
-- ─────────────────────────────────────────────────
BEGIN TRY
    BEGIN TRANSACTION;

    IF NOT EXISTS (SELECT 1 FROM Categoria WHERE descripcion = N'Computadoras')
        INSERT INTO Categoria (descripcion, esActivo) VALUES (N'Computadoras', 1);

    IF NOT EXISTS (SELECT 1 FROM Categoria WHERE descripcion = N'Laptops')
        INSERT INTO Categoria (descripcion, esActivo) VALUES (N'Laptops', 1);

    IF NOT EXISTS (SELECT 1 FROM Categoria WHERE descripcion = N'Teclados')
        INSERT INTO Categoria (descripcion, esActivo) VALUES (N'Teclados', 1);

    IF NOT EXISTS (SELECT 1 FROM Categoria WHERE descripcion = N'Monitores')
        INSERT INTO Categoria (descripcion, esActivo) VALUES (N'Monitores', 1);

    IF NOT EXISTS (SELECT 1 FROM Categoria WHERE descripcion = N'Micrófonos')
        INSERT INTO Categoria (descripcion, esActivo) VALUES (N'Micrófonos', 1);

    COMMIT TRANSACTION;
    PRINT 'Categorías insertadas correctamente.';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Error al insertar Categorías: ' + ERROR_MESSAGE();
END CATCH
GO

-- ─────────────────────────────────────────────────
--  6. TIPOS DE DOCUMENTO DE VENTA
-- ─────────────────────────────────────────────────
BEGIN TRY
    BEGIN TRANSACTION;

    IF NOT EXISTS (SELECT 1 FROM TipoDocumentoVenta WHERE descripcion = N'Boleta')
        INSERT INTO TipoDocumentoVenta (descripcion, esActivo) VALUES (N'Boleta', 1);

    IF NOT EXISTS (SELECT 1 FROM TipoDocumentoVenta WHERE descripcion = N'Factura')
        INSERT INTO TipoDocumentoVenta (descripcion, esActivo) VALUES (N'Factura', 1);

    COMMIT TRANSACTION;
    PRINT 'Tipos de documento de venta insertados correctamente.';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Error al insertar TipoDocumentoVenta: ' + ERROR_MESSAGE();
END CATCH
GO

-- ─────────────────────────────────────────────────
--  7. NÚMERO CORRELATIVO
--  Inicia en 0; el sistema genera: 000001, 000002, ...
-- ─────────────────────────────────────────────────
BEGIN TRY
    BEGIN TRANSACTION;

    IF NOT EXISTS (SELECT 1 FROM NumeroCorrelativo WHERE gestion = N'venta')
        INSERT INTO NumeroCorrelativo (ultimoNumero, cantidadDigitos, gestion, fechaActualizacion)
        VALUES (0, 6, N'venta', GETDATE());

    COMMIT TRANSACTION;
    PRINT 'Número correlativo insertado correctamente.';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Error al insertar NumeroCorrelativo: ' + ERROR_MESSAGE();
END CATCH
GO

-- ─────────────────────────────────────────────────
--  8. MENÚS
--  Estructura:
--    Menús padre (sin controlador): Administración,
--    Inventario, Ventas, Reportes.
--    Menús hijo enlazados al padre correspondiente.
--
--  Al final, los menús padre se auto-referencian
--  (idMenuPadre = idMenu) para que MenuService
--  pueda distinguirlos de los hijos.
-- ─────────────────────────────────────────────────
BEGIN TRY
    BEGIN TRANSACTION;

    -- Menús padre
    IF NOT EXISTS (SELECT 1 FROM Menu WHERE descripcion = N'DashBoard')
        INSERT INTO Menu (descripcion, icono, controlador, paginaAccion, esActivo)
        VALUES (N'DashBoard', N'fas fa-fw fa-tachometer-alt', N'DashBoard', N'Index', 1);

    IF NOT EXISTS (SELECT 1 FROM Menu WHERE descripcion = N'Administración')
        INSERT INTO Menu (descripcion, icono, esActivo)
        VALUES (N'Administración', N'fas fa-fw fa-cog', 1);

    IF NOT EXISTS (SELECT 1 FROM Menu WHERE descripcion = N'Inventario')
        INSERT INTO Menu (descripcion, icono, esActivo)
        VALUES (N'Inventario', N'fas fa-fw fa-clipboard-list', 1);

    IF NOT EXISTS (SELECT 1 FROM Menu WHERE descripcion = N'Ventas')
        INSERT INTO Menu (descripcion, icono, esActivo)
        VALUES (N'Ventas', N'fas fa-fw fa-tags', 1);

    IF NOT EXISTS (SELECT 1 FROM Menu WHERE descripcion = N'Reportes')
        INSERT INTO Menu (descripcion, icono, esActivo)
        VALUES (N'Reportes', N'fas fa-fw fa-chart-area', 1);

    -- Menús hijo: Administración
    IF NOT EXISTS (SELECT 1 FROM Menu WHERE descripcion = N'Usuarios')
    BEGIN
        DECLARE @idAdm INT = (SELECT idMenu FROM Menu WHERE descripcion = N'Administración');
        INSERT INTO Menu (descripcion, idMenuPadre, controlador, paginaAccion, esActivo)
        VALUES (N'Usuarios', @idAdm, N'Usuario', N'Index', 1);
    END

    IF NOT EXISTS (SELECT 1 FROM Menu WHERE descripcion = N'Negocio')
    BEGIN
        DECLARE @idAdm2 INT = (SELECT idMenu FROM Menu WHERE descripcion = N'Administración');
        INSERT INTO Menu (descripcion, idMenuPadre, controlador, paginaAccion, esActivo)
        VALUES (N'Negocio', @idAdm2, N'Negocio', N'Index', 1);
    END

    -- Menús hijo: Inventario
    IF NOT EXISTS (SELECT 1 FROM Menu WHERE descripcion = N'Categorias')
    BEGIN
        DECLARE @idInv INT = (SELECT idMenu FROM Menu WHERE descripcion = N'Inventario');
        INSERT INTO Menu (descripcion, idMenuPadre, controlador, paginaAccion, esActivo)
        VALUES (N'Categorias', @idInv, N'Categoria', N'Index', 1);
    END

    IF NOT EXISTS (SELECT 1 FROM Menu WHERE descripcion = N'Productos')
    BEGIN
        DECLARE @idInv2 INT = (SELECT idMenu FROM Menu WHERE descripcion = N'Inventario');
        INSERT INTO Menu (descripcion, idMenuPadre, controlador, paginaAccion, esActivo)
        VALUES (N'Productos', @idInv2, N'Producto', N'Index', 1);
    END

    -- Menús hijo: Ventas
    IF NOT EXISTS (SELECT 1 FROM Menu WHERE descripcion = N'Nueva Venta')
    BEGIN
        DECLARE @idVta INT = (SELECT idMenu FROM Menu WHERE descripcion = N'Ventas');
        INSERT INTO Menu (descripcion, idMenuPadre, controlador, paginaAccion, esActivo)
        VALUES (N'Nueva Venta', @idVta, N'Venta', N'NuevaVenta', 1);
    END

    IF NOT EXISTS (SELECT 1 FROM Menu WHERE descripcion = N'Historial Venta')
    BEGIN
        DECLARE @idVta2 INT = (SELECT idMenu FROM Menu WHERE descripcion = N'Ventas');
        INSERT INTO Menu (descripcion, idMenuPadre, controlador, paginaAccion, esActivo)
        VALUES (N'Historial Venta', @idVta2, N'Venta', N'HistorialVenta', 1);
    END

    -- Menús hijo: Reportes
    IF NOT EXISTS (SELECT 1 FROM Menu WHERE descripcion = N'Reporte de Ventas')
    BEGIN
        DECLARE @idRep INT = (SELECT idMenu FROM Menu WHERE descripcion = N'Reportes');
        INSERT INTO Menu (descripcion, idMenuPadre, controlador, paginaAccion, esActivo)
        VALUES (N'Reporte de Ventas', @idRep, N'Reporte', N'Index', 1);
    END

    -- Auto-referencia de menús padre (idMenuPadre = idMenu)
    UPDATE Menu SET idMenuPadre = idMenu WHERE idMenuPadre IS NULL;

    COMMIT TRANSACTION;
    PRINT 'Menús insertados correctamente.';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Error al insertar Menús: ' + ERROR_MESSAGE();
END CATCH
GO

-- ─────────────────────────────────────────────────
--  9. ROL - MENÚ (permisos por rol)
--
--  Administrador : acceso completo (todos los menús)
--  Empleado      : solo módulo Ventas
--  Supervisor    : Inventario + Ventas
-- ─────────────────────────────────────────────────
BEGIN TRY
    BEGIN TRANSACTION;

    -- Variables de menús
    DECLARE @mDashBoard     INT = (SELECT idMenu FROM Menu WHERE descripcion = N'DashBoard');
    DECLARE @mUsuarios      INT = (SELECT idMenu FROM Menu WHERE descripcion = N'Usuarios');
    DECLARE @mNegocio       INT = (SELECT idMenu FROM Menu WHERE descripcion = N'Negocio');
    DECLARE @mCategorias    INT = (SELECT idMenu FROM Menu WHERE descripcion = N'Categorias');
    DECLARE @mProductos     INT = (SELECT idMenu FROM Menu WHERE descripcion = N'Productos');
    DECLARE @mNuevaVenta    INT = (SELECT idMenu FROM Menu WHERE descripcion = N'Nueva Venta');
    DECLARE @mHistVenta     INT = (SELECT idMenu FROM Menu WHERE descripcion = N'Historial Venta');
    DECLARE @mReporte       INT = (SELECT idMenu FROM Menu WHERE descripcion = N'Reporte de Ventas');

    -- Variables de roles
    DECLARE @rAdmin     INT = (SELECT idRol FROM Rol WHERE descripcion = N'Administrador');
    DECLARE @rEmpleado  INT = (SELECT idRol FROM Rol WHERE descripcion = N'Empleado');
    DECLARE @rSuperv    INT = (SELECT idRol FROM Rol WHERE descripcion = N'Supervisor');

    -- Administrador: acceso completo
    IF NOT EXISTS (SELECT 1 FROM RolMenu WHERE idRol = @rAdmin AND idMenu = @mDashBoard)
        INSERT INTO RolMenu (idRol, idMenu, esActivo) VALUES (@rAdmin, @mDashBoard, 1);

    IF NOT EXISTS (SELECT 1 FROM RolMenu WHERE idRol = @rAdmin AND idMenu = @mUsuarios)
        INSERT INTO RolMenu (idRol, idMenu, esActivo) VALUES (@rAdmin, @mUsuarios, 1);

    IF NOT EXISTS (SELECT 1 FROM RolMenu WHERE idRol = @rAdmin AND idMenu = @mNegocio)
        INSERT INTO RolMenu (idRol, idMenu, esActivo) VALUES (@rAdmin, @mNegocio, 1);

    IF NOT EXISTS (SELECT 1 FROM RolMenu WHERE idRol = @rAdmin AND idMenu = @mCategorias)
        INSERT INTO RolMenu (idRol, idMenu, esActivo) VALUES (@rAdmin, @mCategorias, 1);

    IF NOT EXISTS (SELECT 1 FROM RolMenu WHERE idRol = @rAdmin AND idMenu = @mProductos)
        INSERT INTO RolMenu (idRol, idMenu, esActivo) VALUES (@rAdmin, @mProductos, 1);

    IF NOT EXISTS (SELECT 1 FROM RolMenu WHERE idRol = @rAdmin AND idMenu = @mNuevaVenta)
        INSERT INTO RolMenu (idRol, idMenu, esActivo) VALUES (@rAdmin, @mNuevaVenta, 1);

    IF NOT EXISTS (SELECT 1 FROM RolMenu WHERE idRol = @rAdmin AND idMenu = @mHistVenta)
        INSERT INTO RolMenu (idRol, idMenu, esActivo) VALUES (@rAdmin, @mHistVenta, 1);

    IF NOT EXISTS (SELECT 1 FROM RolMenu WHERE idRol = @rAdmin AND idMenu = @mReporte)
        INSERT INTO RolMenu (idRol, idMenu, esActivo) VALUES (@rAdmin, @mReporte, 1);

    -- Empleado: solo Ventas
    IF NOT EXISTS (SELECT 1 FROM RolMenu WHERE idRol = @rEmpleado AND idMenu = @mNuevaVenta)
        INSERT INTO RolMenu (idRol, idMenu, esActivo) VALUES (@rEmpleado, @mNuevaVenta, 1);

    IF NOT EXISTS (SELECT 1 FROM RolMenu WHERE idRol = @rEmpleado AND idMenu = @mHistVenta)
        INSERT INTO RolMenu (idRol, idMenu, esActivo) VALUES (@rEmpleado, @mHistVenta, 1);

    -- Supervisor: Inventario + Ventas
    IF NOT EXISTS (SELECT 1 FROM RolMenu WHERE idRol = @rSuperv AND idMenu = @mCategorias)
        INSERT INTO RolMenu (idRol, idMenu, esActivo) VALUES (@rSuperv, @mCategorias, 1);

    IF NOT EXISTS (SELECT 1 FROM RolMenu WHERE idRol = @rSuperv AND idMenu = @mProductos)
        INSERT INTO RolMenu (idRol, idMenu, esActivo) VALUES (@rSuperv, @mProductos, 1);

    IF NOT EXISTS (SELECT 1 FROM RolMenu WHERE idRol = @rSuperv AND idMenu = @mNuevaVenta)
        INSERT INTO RolMenu (idRol, idMenu, esActivo) VALUES (@rSuperv, @mNuevaVenta, 1);

    IF NOT EXISTS (SELECT 1 FROM RolMenu WHERE idRol = @rSuperv AND idMenu = @mHistVenta)
        INSERT INTO RolMenu (idRol, idMenu, esActivo) VALUES (@rSuperv, @mHistVenta, 1);

    COMMIT TRANSACTION;
    PRINT 'RolMenu insertado correctamente.';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Error al insertar RolMenu: ' + ERROR_MESSAGE();
END CATCH
GO

-- ─────────────────────────────────────────────────
--  VERIFICACIÓN FINAL
-- ─────────────────────────────────────────────────
SELECT 'Rol'                AS Tabla, COUNT(*) AS Registros FROM Rol             UNION ALL
SELECT 'Usuario'            AS Tabla, COUNT(*) AS Registros FROM Usuario          UNION ALL
SELECT 'Configuracion'      AS Tabla, COUNT(*) AS Registros FROM Configuracion    UNION ALL
SELECT 'Negocio'            AS Tabla, COUNT(*) AS Registros FROM Negocio          UNION ALL
SELECT 'Categoria'          AS Tabla, COUNT(*) AS Registros FROM Categoria        UNION ALL
SELECT 'TipoDocumentoVenta' AS Tabla, COUNT(*) AS Registros FROM TipoDocumentoVenta UNION ALL
SELECT 'NumeroCorrelativo'  AS Tabla, COUNT(*) AS Registros FROM NumeroCorrelativo UNION ALL
SELECT 'Menu'               AS Tabla, COUNT(*) AS Registros FROM Menu             UNION ALL
SELECT 'RolMenu'            AS Tabla, COUNT(*) AS Registros FROM RolMenu;
GO
