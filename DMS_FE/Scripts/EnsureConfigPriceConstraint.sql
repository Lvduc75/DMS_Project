-- Script để đảm bảo constraint ConfigPrice được cập nhật đúng
-- Chạy script này trực tiếp trong SQL Server Management Studio hoặc Azure Data Studio

USE [dorm_management]
GO

PRINT '=== ĐẢM BẢO CONSTRAINT CONFIGPRICE ==='

-- 1. Drop existing constraint if exists
IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'chk_price_type')
BEGIN
    ALTER TABLE [dbo].[ConfigPrice] DROP CONSTRAINT [chk_price_type]
    PRINT 'Đã xóa constraint cũ: chk_price_type'
END
ELSE
BEGIN
    PRINT 'Không tìm thấy constraint: chk_price_type'
END

-- 2. Add new constraint with correct values
ALTER TABLE [dbo].[ConfigPrice] 
ADD CONSTRAINT [chk_price_type] 
CHECK ([type] IN ('electricity', 'water', 'room'))

PRINT 'Đã thêm constraint mới: chk_price_type với các giá trị đúng'

-- 3. Verify the constraint
SELECT 
    cc.name AS constraint_name,
    cc.definition
FROM sys.check_constraints cc
WHERE cc.name = 'chk_price_type'

PRINT '=== HOÀN THÀNH CẬP NHẬT CONSTRAINT ===' 