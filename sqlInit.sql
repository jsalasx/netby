-- ==========================================
-- CREACIÓN DE TABLAS
-- ==========================================

-- Tabla de Usuarios
CREATE TABLE [Users] (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    Password NVARCHAR(200) NOT NULL,
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL
);

-- Tabla de Productos
CREATE TABLE [Products] (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    Code NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(300) NOT NULL,
    Category NVARCHAR(100) NOT NULL,
    ImageUri NVARCHAR(300) NOT NULL,
    Price INT NOT NULL,
    Stock INT NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT(0),
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL
);

-- Tabla de Transacciones
CREATE TABLE [Transactions] (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Type INT NOT NULL, -- TransactionTypeEnum
    TotalAmount INT NOT NULL,
    Comment NVARCHAR(300) NULL,
    IsDeleted BIT NOT NULL DEFAULT(0),
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL
);

-- Tabla de Detalles de Transacción
CREATE TABLE [TransactionDetails] (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    TransactionId UNIQUEIDENTIFIER NOT NULL,
    ProductId UNIQUEIDENTIFIER NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice INT NOT NULL,
    Total INT NOT NULL,
    FOREIGN KEY (TransactionId) REFERENCES [Transactions](Id) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES [Products](Id)
);

-- ==========================================
-- DATOS DE EJEMPLO
-- ==========================================

-- Usuario de prueba
INSERT INTO [Users] (Id, Name, Email, Password, CreatedAt, UpdatedAt)
VALUES (
    NEWID(),
    'Alejandro Test',
    'alejandro@example.com',
    'hashed_password_123', -- reemplazar por hash real
    SYSUTCDATETIME(),
    SYSUTCDATETIME()
);

-- Productos de prueba
INSERT INTO [Products] (Id, Name, Code, Description, Category, ImageUri, Price, Stock, IsDeleted, CreatedAt, UpdatedAt)
VALUES
(NEWID(), 'Laptop Dell', 'PROD001', 'Laptop de 15 pulgadas', 'Tecnología', 'https://picsum.photos/200/300?1', 800, 10, 0, SYSUTCDATETIME(), SYSUTCDATETIME()),
(NEWID(), 'Auriculares Sony', 'PROD002', 'Auriculares inalámbricos', 'Accesorios', 'https://picsum.photos/200/300?2', 150, 30, 0, SYSUTCDATETIME(), SYSUTCDATETIME()),
(NEWID(), 'Silla Gamer', 'PROD003', 'Silla ergonómica para oficina/gaming', 'Muebles', 'https://picsum.photos/200/300?3', 200, 5, 0, SYSUTCDATETIME(), SYSUTCDATETIME());
