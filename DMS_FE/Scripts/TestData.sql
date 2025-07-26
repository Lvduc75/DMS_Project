-- Test Data Script for DMS Request System
-- This script creates test users for both Student and Manager roles

-- First, check existing users
SELECT 'Existing Users:' as Info;
SELECT Id, Name, Email, Role FROM Users;

-- Create test students if they don't exist
IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'student1@test.com')
BEGIN
    INSERT INTO Users (Name, Email, Role) 
    VALUES ('Test Student 1', 'student1@test.com', 'Student');
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'student2@test.com')
BEGIN
    INSERT INTO Users (Name, Email, Role) 
    VALUES ('Test Student 2', 'student2@test.com', 'Student');
END

-- Create test managers if they don't exist
IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'manager1@test.com')
BEGIN
    INSERT INTO Users (Name, Email, Role) 
    VALUES ('Test Manager 1', 'manager1@test.com', 'Manager');
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'manager2@test.com')
BEGIN
    INSERT INTO Users (Name, Email, Role) 
    VALUES ('Test Manager 2', 'manager2@test.com', 'Manager');
END

-- Show all users after creation
SELECT 'All Users After Creation:' as Info;
SELECT Id, Name, Email, Role FROM Users ORDER BY Role, Id;

-- Show specific users for testing
SELECT 'Students for testing:' as Info;
SELECT Id, Name, Email FROM Users WHERE Role = 'Student';

SELECT 'Managers for testing:' as Info;
SELECT Id, Name, Email FROM Users WHERE Role = 'Manager'; 