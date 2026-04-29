-- =============================================
-- Script completo: Base de datos + Esquema
-- =============================================

-- CREAMOS LA BASE DE DATOS (Si no existe)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'SALES_PRODUCTS')
BEGIN
    CREATE DATABASE SALES_PRODUCTS;
    PRINT 'Base de datos SALES_PRODUCTS creada correctamente';
END
ELSE
BEGIN
    PRINT 'La base de datos SALES_PRODUCTS ya existe.';
END
GO

-- CAMBIAR EL CONTEXTO A LA NUEVA BASE DE DATOS
-- (La instrucción GO es necesaria aquí para que la instrucción USE funcione)
USE SALES_PRODUCTS;
GO

-- CREAR TABLAS
-- TABLA USUARIOS
IF OBJECT_ID('dbo.Users', 'U') IS NULL
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserName NVARCHAR(200) NOT NULL,
        Password VARBINARY(200) NOT NULL,
        EMail NVARCHAR(200) NOT NULL
    );
    PRINT 'Tabla de usuarios creada correctamente.';
END
ELSE
BEGIN
    PRINT 'La tabla de usuarios ya existe. No se realizaron cambios.';
END
GO


-- TABLA PRODUCTOS
IF OBJECT_ID('dbo.Products', 'U') IS NULL
BEGIN
    CREATE TABLE Products (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        Code NVARCHAR(50) NOT NULL,
        Price DECIMAL(18,2) NOT NULL,
        Stock INT NOT NULL,
        State BIT NOT NULL,
        CreatedByUserId INT NOT NULL, 
        CreationDate DATETIME2 NOT NULL,
        UpdatedByUserId INT NULL,
        UpdateDate DATETIME2 NULL,
        DeletedByUserId INT NULL,
        DeletedDate DATETIME2 NULL,
        CONSTRAINT FK_Products_CreatedBy FOREIGN KEY (CreatedByUserId) REFERENCES Users(Id),
        CONSTRAINT FK_Products_UpdatedBy FOREIGN KEY (UpdatedByUserId) REFERENCES Users(Id),
        CONSTRAINT FK_Products_DeletedBy FOREIGN KEY (DeletedByUserId) REFERENCES Users(Id)
    );
    PRINT 'Tabla de productos creada correctamente.';
END
ELSE
BEGIN
    PRINT 'La tabla de Productos ya existe. No se realizaron cambios.';
END
GO


-- RESTRICCIONES(CONSTRAINTS) E ÍNDICES
IF NOT EXISTS (
    SELECT * FROM sys.indexes 
    WHERE name = 'IX_Products_Code' 
    AND object_id = OBJECT_ID('Products')
)
BEGIN
    CREATE UNIQUE INDEX IX_Products_Code ON Products(Code);
    PRINT 'Se ha creado un índice unique IX_Products_Code.';
END
ELSE
BEGIN
    PRINT 'El índice IX_Products_Code ya existe.';
END
GO

-- DATOS DE PRUEBA
-- REGISTROS TABLA USUARIOS
EXEC dbo.CreateUser 'Administrador', 'password123', 'administrador@email.com';
EXEC dbo.CreateUser 'maria.gomez', 'maria123', 'maria.gomez@email.com';
EXEC dbo.CreateUser 'carlos.lopez', 'carlos123', 'carlos.lopez@email.com';
EXEC dbo.CreateUser 'ana.torres', 'ana123', 'ana.torres@email.com';
EXEC dbo.CreateUser 'luis.martinez', 'luis123', 'luis.martinez@email.com';
EXEC dbo.CreateUser 'sofia.ramirez', 'sofia123', 'sofia.ramirez@email.com';
EXEC dbo.CreateUser 'diego.herrera', 'diego123', 'diego.herrera@email.com';
EXEC dbo.CreateUser 'valentina.rojas', 'valentina123', 'valentina.rojas@email.com';
EXEC dbo.CreateUser 'andres.castillo', 'andres123', 'andres.castillo@email.com';
EXEC dbo.CreateUser 'paula.vargas', 'paula123', 'paula.vargas@email.com';
GO


-- REGISTROS TABLA PRODUCTOS
EXEC dbo.CreateProduct 'Laptop Dell XPS 13', 'LAP-DEL-001', 'Ultrabook de 13 pulgadas', 1299.99, 15, 1, '2026-04-20 21:56:41.0400735', 1;
EXEC dbo.CreateProduct 'Mouse Logitech MX Master', 'MOU-LOG-001', 'Mouse ergonómico inalámbrico', 99.99, 50, 1, '2026-04-20 21:56:41.0400735', 1;
EXEC dbo.CreateProduct 'Monitor Samsung 4K', 'MON-SAM-001', 'Monitor profesional 32"', 499.50, 10, 1, '2026-04-20 21:56:41.0400735', 1;
EXEC dbo.CreateProduct 'Teclado Mecánico Keychron', 'KEY-KEY-001', 'Teclado RGB switches rojos', 89.99, 25, 1, '2026-04-20 21:56:41.0400735', 1;
EXEC dbo.CreateProduct 'Audífonos Sony WH-1000XM5', 'AUD-SON-001', 'Cancelación de ruido premium', 348.00, 12, 1, '2026-04-20 21:56:41.0400735', 1;
EXEC dbo.CreateProduct 'Webcam Logitech C920', 'WEB-LOG-001', 'Full HD 1080p para streaming', 79.99, 30, 1, '2026-04-20 21:56:41.0400735', 1;
EXEC dbo.CreateProduct 'Disco SSD Externo Samsung T7', 'SSD-SAM-001', '1TB USB-C portable', 119.50, 8, 1, '2026-04-20 21:56:41.0400735', 1;
EXEC dbo.CreateProduct 'Hub USB-C Anker 7 en 1', 'HUB-ANK-001', 'Adaptador multipuerto HDMI', 59.99, 10, 1, '2026-04-20 21:56:41.0400735', 1;
EXEC dbo.CreateProduct 'Soporte Laptop Ajustable', 'STA-ALI-001', 'Aluminio, altura regulable', 45.00, 20, 1, '2026-04-20 21:56:41.0400735', 1;
EXEC dbo.CreateProduct 'Cable USB-C a USB-C 2m', 'CAB-USB-001', 'Carga rápida 100W', 15.99, 100, 1, '2026-04-20 21:56:41.0400735', 1;
GO

PRINT 'El script se ha completado correctamente.';

