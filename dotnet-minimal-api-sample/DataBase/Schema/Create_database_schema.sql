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
        Id           INT IDENTITY(1,1) PRIMARY KEY,
        UserName     NVARCHAR(200)     NOT NULL,
        Password     NVARCHAR(200)     NOT NULL,
        Email        NVARCHAR(200)     NOT NULL
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
        Id                  INT IDENTITY(1,1) PRIMARY KEY,
        Name                NVARCHAR(200)     NOT NULL,
        Description         NVARCHAR(MAX)     NULL,
        Code                NVARCHAR(50)      NOT NULL,
        Price               DECIMAL(18,2)     NOT NULL,
        Stock               INT               NOT NULL,
        State               BIT               NOT NULL,
        CreatedByUserId     INT               NOT NULL, 
        CreationDate        DATETIME2         NOT NULL,
        UpdatedByUserId     INT               NULL,
        UpdateDate          DATETIME2         NULL,
        DeletedByUserId     INT               NULL,
        DeletedDate         DATETIME2         NULL,

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

-- TABLA CLIENTES
IF OBJECT_ID('dbo.Customers', 'U') IS NULL
BEGIN
    CREATE TABLE Customers (
        Id                  INT IDENTITY(1,1) PRIMARY KEY,
        FirstName           NVARCHAR(100)     NOT NULL,
        MiddleName          NVARCHAR(100)     NULL,
        LastName            NVARCHAR(100)     NOT NULL,
        SecondLastName      NVARCHAR(100)     NULL,
        DocumentType        NVARCHAR(20)      NOT NULL,
        DocumentNumber      NVARCHAR(30)      NOT NULL,
        Email               NVARCHAR(200)     NULL,
        Phone               NVARCHAR(20)      NULL,
        City                NVARCHAR(100)     NULL,
        Address             NVARCHAR(255)     NULL,
        State               BIT               NOT NULL,
        CreatedByUserId     INT               NOT NULL,
        CreationDate        DATETIME2         NOT NULL,
        UpdatedByUserId     INT               NULL,
        UpdateDate          DATETIME2         NULL,
        DeletedByUserId     INT               NULL,
        DeletedDate         DATETIME2         NULL,

        CONSTRAINT FK_Customers_CreatedBy FOREIGN KEY (CreatedByUserId) REFERENCES Users(Id),
        CONSTRAINT FK_Customers_UpdatedBy FOREIGN KEY (UpdatedByUserId) REFERENCES Users(Id),
        CONSTRAINT FK_Customers_DeletedBy FOREIGN KEY (DeletedByUserId) REFERENCES Users(Id)
    );
    PRINT 'Tabla de clientes creada correctamente.';
END
ELSE
BEGIN
    PRINT 'La tabla de clientes ya existe. No se realizaron cambios.';
END
GO

-- TABLA VENTAS (CABECERA)
IF OBJECT_ID('dbo.Sales', 'U') IS NULL
BEGIN
    CREATE TABLE Sales (
    Id                  INT IDENTITY(1,1)  PRIMARY KEY,
    InvoiceNumber       NVARCHAR(50)       NOT NULL,
    CustomerId          INT                NOT NULL,
    SaleUserId          INT                NOT NULL,
    SaleDateTime        DATETIME2          NOT NULL,
    Observations        NVARCHAR(500)      NULL,
    State               BIT                NOT NULL,
    CancellationDate    DATETIME2          NULL,
    CancellationUserId  INT                NULL,

    CONSTRAINT FK_Sales_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(Id),
    CONSTRAINT FK_Sales_SaleUser FOREIGN KEY (SaleUserId) REFERENCES Users(Id),
    CONSTRAINT FK_Sales_CancellationUser FOREIGN KEY (CancellationUserId) REFERENCES Users(Id),
    );
    PRINT 'Tabla de ventas creada correctamente.';
END
ELSE
BEGIN
    PRINT 'La tabla de ventas ya existe. No se realizaron cambios.';
END
GO

-- TABLA VENTAS (DETALLE)
IF OBJECT_ID('dbo.SaleItems', 'U') IS NULL
BEGIN
    CREATE TABLE SaleItems (
    Id            INT IDENTITY(1,1)  PRIMARY KEY,
    SaleId        INT                NOT NULL,
    ProductId     INT                NOT NULL,
    Quantity      INT                NOT NULL,
    UnitPrice     DECIMAL(18,2)      NOT NULL,
    Discount      DECIMAL(18,2)      NULL,

    CONSTRAINT FK_SaleItems_Sales FOREIGN KEY (SaleId) REFERENCES Sales(Id),
    CONSTRAINT FK_SaleItems_Products FOREIGN KEY (ProductId) REFERENCES Products(Id),
    CONSTRAINT CK_SaleItems_Quantity CHECK (Quantity > 0),
    CONSTRAINT CK_SaleItems_UnitPrice CHECK (UnitPrice >= 0),
    CONSTRAINT CK_SaleItems_Discount CHECK (Discount >= 0)
    );
    PRINT 'Tabla de ventas detalle creada correctamente.';
END
ELSE
BEGIN
    PRINT 'La tabla de ventas detalle ya existe. No se realizaron cambios.';
END
GO

-- TABLA DE CONTADORES PARA LA NUMERACIÓN DE LAS FACTURAS
IF OBJECT_ID('dbo.InvoiceCounters', 'U') IS NULL
BEGIN
    CREATE TABLE InvoiceCounters (
        Prefix        NVARCHAR(10)  PRIMARY KEY,
        LastNumber    INT           NOT NULL DEFAULT 0
    );
    -- Insertamos el prefijo inicial para las facturas
    INSERT INTO InvoiceCounters (Prefix, LastNumber) VALUES ('FAC-', 0);
    PRINT 'Tabla de contadores creada correctamente.';
END
ELSE
BEGIN
    PRINT 'La tabla de contadores ya existe. No se realizaron cambios.';
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

IF NOT EXISTS (
    SELECT * FROM sys.indexes 
    WHERE name = 'IX_Customers_DocumentNumber' 
    AND object_id = OBJECT_ID('Customers')
)
BEGIN
    CREATE UNIQUE INDEX IX_Customers_DocumentNumber ON Customers(DocumentNumber);
    PRINT 'Se ha creado un índice unique IX_Customers_DocumentNumber.';
END
ELSE
BEGIN
    PRINT 'El índice IX_Customers_DocumentNumber ya existe.';
END
GO

IF NOT EXISTS (
    SELECT * FROM sys.indexes 
    WHERE name = 'IX_Sales_InvoiceNumber' 
    AND object_id = OBJECT_ID('Sales')
)
BEGIN
    CREATE UNIQUE INDEX IX_Sales_InvoiceNumber ON Sales(InvoiceNumber);
    PRINT 'Se ha creado un índice unique IX_Sales_InvoiceNumber.';
END
ELSE
BEGIN
    PRINT 'El índice IX_Sales_InvoiceNumber ya existe.';
END
GO

-- DATOS DE PRUEBA
-- REGISTROS TABLA USUARIOS (Clave pass123)
--INSERT INTO Users (UserName, Password, Email) VALUES 
--('Administrador', '$2a$11$fxwk9/mRcKA9a7MOz7WCluRavrHzjqDlHEKbUw/V0YM0757QXMUPW', 'administrador@email.com'),
--('maria.gomez', '$2a$11$fxwk9/mRcKA9a7MOz7WCluRavrHzjqDlHEKbUw/V0YM0757QXMUPW', 'maria.gomez@email.com'),
--('carlos.lopez', '$2a$11$fxwk9/mRcKA9a7MOz7WCluRavrHzjqDlHEKbUw/V0YM0757QXMUPW', 'carlos.lopez@email.com'),
--('ana.torres', '$2a$11$fxwk9/mRcKA9a7MOz7WCluRavrHzjqDlHEKbUw/V0YM0757QXMUPW', 'ana.torres@email.com'),
--('luis.martinez', '$2a$11$fxwk9/mRcKA9a7MOz7WCluRavrHzjqDlHEKbUw/V0YM0757QXMUPW', 'luis.martinez@email.com'),
--('sofia.ramirez', '$2a$11$fxwk9/mRcKA9a7MOz7WCluRavrHzjqDlHEKbUw/V0YM0757QXMUPW', 'sofia.ramirez@email.com'),
--('diego.herrera', '$2a$11$fxwk9/mRcKA9a7MOz7WCluRavrHzjqDlHEKbUw/V0YM0757QXMUPW', 'diego.herrera@email.com'),
--('valentina.rojas', '$2a$11$fxwk9/mRcKA9a7MOz7WCluRavrHzjqDlHEKbUw/V0YM0757QXMUPW', 'valentina.rojas@email.com'),
--('andres.castillo', '$2a$11$fxwk9/mRcKA9a7MOz7WCluRavrHzjqDlHEKbUw/V0YM0757QXMUPW', 'andres.castillo@email.com'),
--('paula.vargas', '$2a$11$fxwk9/mRcKA9a7MOz7WCluRavrHzjqDlHEKbUw/V0YM0757QXMUPW', 'paula.vargas@email.com');
--GO

-- REGISTROS TABLA PRODUCTOS
--INSERT INTO Products (Name, Code, Description, Price, Stock, State, CreationDate, CreatedByUserId) VALUES
--('Laptop Dell XPS 13', 'LAP-DEL-001', 'Ultrabook de 13 pulgadas', 1299.99, 15, 1, '2026-04-20 21:56:41.0400735', 1),
--('Mouse Logitech MX Master', 'MOU-LOG-001', 'Mouse ergonómico inalámbrico', 99.99, 50, 1, '2026-04-20 21:56:41.0400735', 1),
--('Monitor Samsung 4K', 'MON-SAM-001', 'Monitor profesional 32"', 499.50, 10, 1, '2026-04-20 21:56:41.0400735', 1),
--('Teclado Mecánico Keychron', 'KEY-KEY-001', 'Teclado RGB switches rojos', 89.99, 25, 1, '2026-04-20 21:56:41.0400735', 1),
--('Audífonos Sony WH-1000XM5', 'AUD-SON-001', 'Cancelación de ruido premium', 348.00, 12, 1, '2026-04-20 21:56:41.0400735', 1),
--('Webcam Logitech C920', 'WEB-LOG-001', 'Full HD 1080p para streaming', 79.99, 30, 1, '2026-04-20 21:56:41.0400735', 1),
--('Disco SSD Externo Samsung T7', 'SSD-SAM-001', '1TB USB-C portable', 119.50, 8, 1, '2026-04-20 21:56:41.0400735', 1),
--('Hub USB-C Anker 7 en 1', 'HUB-ANK-001', 'Adaptador multipuerto HDMI', 59.99, 10, 1, '2026-04-20 21:56:41.0400735', 1),
--('Soporte Laptop Ajustable', 'STA-ALI-001', 'Aluminio, altura regulable', 45.00, 20, 1, '2026-04-20 21:56:41.0400735', 1),
--('Cable USB-C a USB-C 2m', 'CAB-USB-001', 'Carga rápida 100W', 15.99, 100, 1, '2026-04-20 21:56:41.0400735', 1);
--GO

-- REGISTROS TABLA CLIENTES
--INSERT INTO Customers (FirstName, MiddleName, LastName, SecondLastName, DocumentType, DocumentNumber, Email, Phone, City, Address, State, CreationDate, CreatedByUserId) VALUES
--('Carlos', 'Andrés', 'García', 'López', 'CC', '10203040', 'carlos.garcia@email.com', '3001234567', 'Bogotá', 'Calle 100 #15-20', 1, '2026-05-13 10:00:00', 1),
--('Ana', NULL, 'Rodríguez', 'Pérez', 'CC', '20304050', 'ana.rod@email.com', '3109876543', 'Medellín', 'Carrera 43A #1-10', 1, '2026-05-13 10:05:00', 1),
--('Luis', 'Alberto', 'Martínez', NULL, 'CE', '30405060', 'luis.mtz@email.com', '3201112233', 'Cali', 'Av. 6 Norte #12-30', 1, '2026-05-13 10:10:00', 1),
--('María', 'Lucía', 'Sánchez', 'Gómez', 'CC', '40506070', 'maria.s@email.com', NULL, 'Barranquilla', 'Calle 72 #45-10', 1, '2026-05-13 10:15:00', 1),
--('Jorge', NULL, 'Ramírez', 'Torres', 'CC', '50607080', 'jorge.ram@email.com', '3154445566', 'Bucaramanga', NULL, 1, '2026-05-13 10:20:00', 1),
--('Claudia', 'Elena', 'Castro', 'Sánchez', 'CE', '60708090', NULL, '3007778899', 'Pereira', 'Calle 20 #10-05', 1, '2026-05-13 10:25:00', 1),
--('Andrés', 'Felipe', 'Valencia', NULL, 'CC', '70809010', 'andres.val@email.com', '3112223344', 'Manizales', 'Diagonal 40 #5-12', 1, '2026-05-13 10:30:00', 1),
--('Sonia', NULL, 'Mendoza', 'Ríos', 'CC', '80901020', 'sonia.m@email.com', '3005556677', 'Cartagena', 'Av. Guardería #2-10', 1, '2026-05-13 10:35:00', 1),
--('Ricardo', 'José', 'Ortiz', 'Castillo', 'CE', '90102030', 'ricardo.o@email.com', '3189990011', 'Neiva', NULL, 1, '2026-05-13 10:40:00', 1),
--('Elena', NULL, 'Vargas', 'Ruiz', 'CC', '11223344', 'elena.vargas@email.com', '3123334455', 'Armenia', 'Carrera 12 #8-40', 1, '2026-05-13 10:45:00', 1);
--GO

PRINT 'El script se ha completado correctamente.';
