/* =========================================================
   TASK MANAGEMENT SYSTEM - FULL DATABASE SETUP
   Run this file once to initialize the entire system
   ========================================================= */

-- =========================
-- 1. CREATE DATABASE
-- =========================
IF DB_ID('TaskManagementDB') IS NULL
BEGIN
    CREATE DATABASE TaskManagementDB;
END
GO

USE TaskManagementDB;
GO

-- =========================
-- 2. DROP TABLES (SAFE RESET)
-- =========================
IF OBJECT_ID('Tasks', 'U') IS NOT NULL DROP TABLE Tasks;
IF OBJECT_ID('Users', 'U') IS NOT NULL DROP TABLE Users;
GO

-- =========================
-- 3. CREATE USERS TABLE
-- =========================
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Role INT NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- =========================
-- 4. CREATE TASKS TABLE
-- =========================
CREATE TABLE Tasks (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(150) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    Status INT NOT NULL DEFAULT 0, -- 0=Pending, 1=InProgress, 2=Completed
    Priority INT NOT NULL,         -- 1=Low, 2=Medium, 3=High
    DueDate DATETIME NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL,
    UserId INT NOT NULL,

    CONSTRAINT FK_Tasks_Users FOREIGN KEY (UserId)
        REFERENCES Users(Id)
        ON DELETE CASCADE
);
GO

-- =========================
-- 5. SEED DATA (USERS)
-- =========================
INSERT INTO Users (Name, Email, PasswordHash, Role)
VALUES 
(
    'Admin User', 
    'admin@task.com', 
    '$2a$11$WkAYbe/N2/GNakAyUnopDulqZ4DS2Jm3LjwYOmL6wUiuoYBZO62fm'
    1 -- admin
),

(
    'John Doe', 
    'john@task.com', 
    '$2a$11$SahxO536hBaJBcqPEFwDcubZASyhrLDz5EfVAE5Rw9kk2TpJBVjJ.',
    2 -- emplyee
),

(
    'Jane Smith', 
    'jane@task.com', 
    '$2a$11$bUdjM8/pTqZxiUJcrZUdvuiiGmsC3TzJ1PEe7IyQE2rn/vqnqfFkm', 
    2 -- emplyee
);
GO

-- =========================
-- 6. SEED DATA (TASKS)
-- =========================
INSERT INTO Tasks (Title, Description, Status, Priority, DueDate, UserId)
VALUES 
('Setup Project Structure', 'Initialize backend layered architecture', 2, 3, DATEADD(DAY, 5, GETDATE()), 1),
('Implement JWT Authentication', 'Build login and token system', 1, 3, DATEADD(DAY, 3, GETDATE()), 1),
('Fix API Bug', 'Resolve null reference exception in user API', 0, 2, DATEADD(DAY, 2, GETDATE()), 2),
('Design Dashboard UI', 'Create Vue admin dashboard layout', 0, 1, DATEADD(DAY, 7, GETDATE()), 3),
('Task Filtering Feature', 'Add filtering by status and priority', 1, 2, DATEADD(DAY, 6, GETDATE()), 2);
GO

-- =========================
-- 7. VERIFY DATA
-- =========================
SELECT 'USERS' AS TableName;
SELECT * FROM Users;

SELECT 'TASKS' AS TableName;
SELECT * FROM Tasks;
GO