-- Script để sửa CHECK constraint cho ConfigPrice table
-- Chạy script này trực tiếp trong SQL Server Management Studio hoặc Azure Data Studio

USE [dorm_management]
GO

-- Drop existing constraint if exists
IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'chk_price_type')
BEGIN
    ALTER TABLE [dbo].[ConfigPrice] DROP CONSTRAINT [chk_price_type]
    PRINT 'Đã xóa constraint cũ: chk_price_type'
END
ELSE
BEGIN
    PRINT 'Không tìm thấy constraint: chk_price_type'
END

-- Add new constraint with UPPERCASE values
ALTER TABLE [dbo].[ConfigPrice] 
ADD CONSTRAINT [chk_price_type] 
CHECK ([type] IN ('ROOM', 'ELECTRIC', 'WATER', 'INTERNET', 'CLEANING', 'OTHER'))

PRINT 'Đã thêm constraint mới: chk_price_type với các giá trị UPPERCASE'

-- Verify the constraint
SELECT 
    cc.name AS constraint_name,
    cc.definition
FROM sys.check_constraints cc
WHERE cc.name = 'chk_price_type'

PRINT 'Kiểm tra constraint đã được tạo thành công!' 