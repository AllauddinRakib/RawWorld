-- =============================================
-- Raw World Database Schema
-- MySQL 8.0 | Generated from EF Core Migration
-- Date: 2026-03-19
-- =============================================

CREATE DATABASE IF NOT EXISTS RawWorldDb
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_general_ci;

USE RawWorldDb;

-- =============================================
-- Table: Categories
-- =============================================
CREATE TABLE Categories (
    Id INT NOT NULL AUTO_INCREMENT,
    Name LONGTEXT NOT NULL,
    PRIMARY KEY (Id)
) CHARACTER SET utf8mb4;

-- =============================================
-- Table: Users
-- =============================================
CREATE TABLE Users (
    Id INT NOT NULL AUTO_INCREMENT,
    FullName LONGTEXT NOT NULL,
    Email LONGTEXT NOT NULL,
    PasswordHash LONGTEXT NOT NULL,
    Role LONGTEXT NOT NULL,
    CreatedAt DATETIME(6) NOT NULL,
    PRIMARY KEY (Id)
) CHARACTER SET utf8mb4;

-- =============================================
-- Table: Products (FK → Categories)
-- =============================================
CREATE TABLE Products (
    Id INT NOT NULL AUTO_INCREMENT,
    CategoryId INT NULL,
    Name LONGTEXT NOT NULL,
    Description LONGTEXT NOT NULL,
    Price DECIMAL(65,30) NOT NULL,
    StockQuantity INT NOT NULL,
    ImageUrl LONGTEXT NULL,
    IsActive TINYINT(1) NOT NULL DEFAULT 1,
    CreatedAt DATETIME(6) NOT NULL,
    PRIMARY KEY (Id),
    CONSTRAINT FK_Products_Categories_CategoryId
        FOREIGN KEY (CategoryId) REFERENCES Categories (Id)
) CHARACTER SET utf8mb4;

CREATE INDEX IX_Products_CategoryId ON Products (CategoryId);

-- =============================================
-- Table: RefreshTokens (FK → Users)
-- =============================================
CREATE TABLE RefreshTokens (
    Id INT NOT NULL AUTO_INCREMENT,
    UserId INT NOT NULL,
    Token LONGTEXT NOT NULL,
    ExpiresAt DATETIME(6) NOT NULL,
    IsRevoked TINYINT(1) NOT NULL DEFAULT 0,
    PRIMARY KEY (Id),
    CONSTRAINT FK_RefreshTokens_Users_UserId
        FOREIGN KEY (UserId) REFERENCES Users (Id)
        ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE INDEX IX_RefreshTokens_UserId ON RefreshTokens (UserId);

-- =============================================
-- Table: CartItems (FK → Products)
-- =============================================
CREATE TABLE CartItems (
    Id INT NOT NULL AUTO_INCREMENT,
    UserId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    PRIMARY KEY (Id),
    CONSTRAINT FK_CartItems_Products_ProductId
        FOREIGN KEY (ProductId) REFERENCES Products (Id)
        ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE INDEX IX_CartItems_ProductId ON CartItems (ProductId);
CREATE UNIQUE INDEX IX_CartItems_UserId_ProductId ON CartItems (UserId, ProductId);

-- =============================================
-- Table: Orders
-- =============================================
CREATE TABLE Orders (
    Id INT NOT NULL AUTO_INCREMENT,
    UserId INT NOT NULL,
    TotalAmount DECIMAL(65,30) NOT NULL,
    Status LONGTEXT NOT NULL,
    PaymentStatus LONGTEXT NOT NULL,
    Address LONGTEXT NULL,
    CreatedAt DATETIME(6) NOT NULL,
    UpdatedAt DATETIME(6) NOT NULL,
    PRIMARY KEY (Id)
) CHARACTER SET utf8mb4;

-- =============================================
-- Table: OrderItems (FK → Orders, Products)
-- =============================================
CREATE TABLE OrderItems (
    Id INT NOT NULL AUTO_INCREMENT,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(65,30) NOT NULL,
    PRIMARY KEY (Id),
    CONSTRAINT FK_OrderItems_Orders_OrderId
        FOREIGN KEY (OrderId) REFERENCES Orders (Id)
        ON DELETE CASCADE,
    CONSTRAINT FK_OrderItems_Products_ProductId
        FOREIGN KEY (ProductId) REFERENCES Products (Id)
        ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE INDEX IX_OrderItems_OrderId ON OrderItems (OrderId);
CREATE INDEX IX_OrderItems_ProductId ON OrderItems (ProductId);

-- =============================================
-- Seed Data: Default Categories
-- =============================================
INSERT INTO Categories (Name) VALUES ('Juices'), ('Smoothies'), ('Detox Drinks');

-- =============================================
-- Seed Data: Default Admin Account
-- Password: Admin@123 (BCrypt hashed)
-- =============================================
INSERT INTO Users (FullName, Email, PasswordHash, Role, CreatedAt)
VALUES (
    'Admin User',
    'admin@rawworld.com',
    '$2a$11$placeholder_hash_replaced_at_runtime',
    'Admin',
    UTC_TIMESTAMP()
);
