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
        CreationDate DATETIME2 NOT NULL,
        UpdateDate DATETIME2 NULL
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
--INSERT INTO Products (Name, Code, Description, Price, Stock, State, CreationDate) VALUES
--('Laptop Dell XPS 13', 'LAP-DEL-001', 'Ultrabook de 13 pulgadas', 1299.99, 15, 1, '2024-01-15 09:30:25.1234567'),
--('Mouse Logitech MX Master', 'MOU-LOG-001', 'Mouse ergonómico inalámbrico', 99.99, 50, 1, '2024-01-16 14:45:10.9876543'),
--('Monitor Samsung 4K', 'MON-SAM-001', 'Monitor profesional 32"', 499.50, 0, 0, '2024-01-10 11:20:55.0000000'),
--('Teclado Mecánico Keychron', 'KEY-KEY-001', 'Teclado RGB switches rojos', 89.99, 25, 1, '2024-01-18 16:15:33.5555555'),
--('Audífonos Sony WH-1000XM5', 'AUD-SON-001', 'Cancelación de ruido premium', 348.00, 12, 1, '2024-02-01 10:00:00.0000001'),
--('Webcam Logitech C920', 'WEB-LOG-001', 'Full HD 1080p para streaming', 79.99, 30, 1, '2024-02-05 13:30:45.7777777'),
--('Disco SSD Externo Samsung T7', 'SSD-SAM-001', '1TB USB-C portable', 119.50, 8, 1, '2024-02-10 09:45:15.1111111'),
--('Hub USB-C Anker 7 en 1', 'HUB-ANK-001', 'Adaptador multipuerto HDMI', 59.99, 0, 0, '2024-02-12 15:20:59.9999999'),
--('Soporte Laptop Ajustable', 'STA-ALI-001', 'Aluminio, altura regulable', 45.00, 20, 1, '2024-02-15 11:00:00.5000000'),
--('Cable USB-C a USB-C 2m', 'CAB-USB-001', 'Carga rápida 100W', 15.99, 100, 1, '2024-02-20 08:30:00.0000000');
--GO

PRINT 'El script se ha completado correctamente.';

