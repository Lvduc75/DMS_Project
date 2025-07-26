-- Check if managers exist in the database
SELECT * FROM Users WHERE Role = 'Manager';

-- If no managers exist, create a default manager
-- Uncomment the following lines if you need to create a manager:

/*
INSERT INTO Users (Name, Email, Role) 
VALUES ('Default Manager', 'manager@dms.com', 'Manager');

-- Get the created manager's ID
SELECT Id, Name, Email, Role FROM Users WHERE Role = 'Manager';
*/ 