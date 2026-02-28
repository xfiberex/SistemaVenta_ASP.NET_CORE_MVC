
--________________________________ INSERTAR ROLES ________________________________
insert into rol(descripcion,esActivo) values
('Administrador',1),
('Empleado',1),
('Supervisor',1)


--________________________________ INSERTAR USUARIO ________________________________
SELECT * FROM Rol
SELECT * FROM Usuario
--clave : 159 o 123
insert into Usuario(nombre,correo,telefono,idRol,urlFoto,nombreFoto,clave,esActivo) values
('User Admin','useradmin@gmail.com','000-000-0000',1,'','','a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3',1)

--________________________________ RECURSOS DE FIREBASE_STORAGE Y CORREO ________________________________
--(AQUI DEBES INCLUIR TUS PROPIAS CLAVES Y CRENDENCIALES)

select * from Configuracion

insert into Configuracion(recurso,propiedad,valor) values
('FireBase_Storage','email',''),
('FireBase_Storage','clave',''),
('FireBase_Storage','ruta',''),
('FireBase_Storage','api_key',''),
('FireBase_Storage','carpeta_usuario','IMAGENES_USUARIO'),
('FireBase_Storage','carpeta_producto','IMAGENES_PRODUCTO'),
('FireBase_Storage','carpeta_logo','IMAGENES_LOGO')

insert into Configuracion(recurso,propiedad,valor) values
('Servicio_Correo','correo',''),
('Servicio_Correo','clave',''),
('Servicio_Correo','alias','MiTienda.com'),
('Servicio_Correo','host','smtp.gmail.com'),
('Servicio_Correo','puerto','587')


--________________________________ INSERTAR NEGOCIO ________________________________
select * from Negocio

insert into Negocio(idNegocio,urlLogo,nombreLogo,numeroDocumento,nombre,correo,direccion,telefono,porcentajeImpuesto,simboloMoneda)
values(1,'','','','','','','',0,'')


--________________________________ INSERTAR CATEGORIAS ________________________________
SELECT * FROM Categoria
--delete from Producto
--delete from Categoria

--DBCC CHECKIDENT ('Producto', RESEED, 0);

INSERT INTO Categoria(descripcion,esActivo) values
('Computadoras',1),
('Laptops',1),
('Teclados',1),
('Monitores',1),
('Microfonos',1)


--________________________________ INSERTAR TIPO DOCUMENTO VENTA ________________________________

select * from TipoDocumentoVenta

insert into TipoDocumentoVenta(descripcion,esActivo) values
('Boleta',1),
('Factura',1)


--________________________________ INSERTAR NUMERO CORRELATIVO ________________________________
select * from NumeroCorrelativo
--000001
insert into NumeroCorrelativo(ultimoNumero,cantidadDigitos,gestion,fechaActualizacion) values
(0,6,'venta',getdate())


--________________________________ INSERTAR MENUS ________________________________
select * from Menu

--*menu padre
insert into Menu(descripcion,icono,controlador,paginaAccion,esActivo) values
('DashBoard','fas fa-fw fa-tachometer-alt','DashBoard','Index',1)

insert into Menu(descripcion,icono,esActivo) values
('Administraci�n','fas fa-fw fa-cog',1),
('Inventario','fas fa-fw fa-clipboard-list',1),
('Ventas','fas fa-fw fa-tags',1),
('Reportes','fas fa-fw fa-chart-area',1)


--*menu hijos Administracion
insert into Menu(descripcion,idMenuPadre, controlador,paginaAccion,esActivo) values
('Usuarios',2,'Usuario','Index',1),
('Negocio',2,'Negocio','Index',1)


--*menu hijos - Inventario
insert into Menu(descripcion,idMenuPadre, controlador,paginaAccion,esActivo) values
('Categorias',3,'Categoria','Index',1),
('Productos',3,'Producto','Index',1)

--*menu hijos - Ventas
insert into Menu(descripcion,idMenuPadre, controlador,paginaAccion,esActivo) values
('Nueva Venta',4,'Venta','NuevaVenta',1),
('Historial Venta',4,'Venta','HistorialVenta',1)

--*menu hijos - Reportes
insert into Menu(descripcion,idMenuPadre, controlador,paginaAccion,esActivo) values
('Reporte de Ventas',5,'Reporte','Index',1)


UPDATE Menu SET idMenuPadre = idMenu where idMenuPadre is null


--________________________________ INSERTAR ROL MENU ________________________________
select * from Menu
select * from RolMenu
SELECT * FROM ROL

--*administrador
INSERT INTO RolMenu(idRol,idMenu,esActivo) values
(1,1,1),
(1,6,1),
(1,7,1),
(1,8,1),
(1,9,1),
(1,10,1),
(1,11,1),
(1,12,1)

--*Empleado
INSERT INTO RolMenu(idRol,idMenu,esActivo) values
(2,10,1),
(2,11,1)

--*Supervisor
INSERT INTO RolMenu(idRol,idMenu,esActivo) values
(3,8,1),
(3,9,1),
(3,10,1),
(3,11,1)

--delete from Producto

update Producto SET nombreImagen = '02G-P4-6157-RX_LG_1.png' where idProducto = 1
 

select * from Usuario
select * from Categoria
select * from Producto
select * from Venta
select * from DetalleVenta
select * from NumeroCorrelativo

--000001
--insert into NumeroCorrelativo(ultimoNumero,cantidadDigitos,gestion,fechaActualizacion) values
--(0,6,'venta',getdate())

--delete from Usuario
--delete from Venta
--delete from DetalleVenta
--delete from NumeroCorrelativo

--DBCC CHECKIDENT ('DetalleVenta', RESEED, 0);

--INSERT INTO Producto(codigoBarra, marca, descripcion, idCategoria, stock, precio, esActivo) values
--('90XB00G0B0UA10', 'Razer', 'Teclado Gamer Huntsman V2', 3, 10, 6450, 1),
--('91XB10G1B0UA11', 'Logitech', 'Mouse Gamer G502 HERO', 3, 15, 2650, 1),
--('92XB20G2B0UA12', 'ASUS', 'TUF Gaming 27" 2K', 4, 8, 18200, 1),
--('93XB30G3B0UA13', 'HyperX', 'Aud�fonos Gamer Cloud II', 3, 12, 3550, 1),
--('94XB40G4B0UA14', 'Corsair', 'Vengeance LPX - RAM DDR4 32GB', 6, 5, 4100, 1),
--('95XB50G5B0UA15', 'NZXT', 'Gabinete Gamer H510i', 6, 7, 5300, 1),
--('96XB60G6B0UA16', 'MSI', 'Placa Madre Z690-A WIFI', 6, 3, 9800, 1),
--('97XB70G7B0UA17', 'EVGA', 'XC Gaming RTX 3060 12GB', 6, 2, 30000, 1),
--('98XB80G8B0UA18', 'Intel', 'Procesador Core i5-12600K', 6, 4, 11200, 1);

--insert into Producto(
--codigoBarra,
--marca,
--descripcion,
--idCategoria,
--stock,
--precio,
--esActivo
--)values
--(
--'30',
--'NVIDIA',
--'GTX 1050 FTW 2GB GDDR5',
--2,
--5,
--4000,
--1
--)