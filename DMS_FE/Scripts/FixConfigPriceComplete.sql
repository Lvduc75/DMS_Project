-- Script tổng hợp để sửa lỗi ConfigPrice hoàn chỉnh
-- Chạy script này trực tiếp trong SQL Server Management Studio hoặc Azure Data Studio

USE [dorm_management]
GO

PRINT '=== BẮT ĐẦU SỬA LỖI CONFIGPRICE ==='

-- 1. Kiểm tra dữ liệu hiện tại
PRINT '=== BƯỚC 1: KIỂM TRA DỮ LIỆU HIỆN TẠI ==='
SELECT 
    id,
    type,
    unit_price,
    effective_from
FROM [dbo].[ConfigPrice]
ORDER BY id

-- 2. Kiểm tra constraint hiện tại
PRINT '=== BƯỚC 2: KIỂM TRA CONSTRAINT HIỆN TẠI ==='
SELECT 
    cc.name AS constraint_name,
    cc.definition
FROM sys.check_constraints cc
WHERE cc.name = 'chk_price_type'

-- 3. Drop existing constraint if exists
PRINT '=== BƯỚC 3: XÓA CONSTRAINT CŨ ==='
IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'chk_price_type')
BEGIN
    ALTER TABLE [dbo].[ConfigPrice] DROP CONSTRAINT [chk_price_type]
    PRINT 'Đã xóa constraint cũ: chk_price_type'
END
ELSE
BEGIN
    PRINT 'Không tìm thấy constraint: chk_price_type'
END

-- 4. Cập nhật dữ liệu để phù hợp với schema mới
PRINT '=== BƯỚC 4: CẬP NHẬT DỮ LIỆU ==='
UPDATE [dbo].[ConfigPrice]
SET type = 'electricity'
WHERE type IN ('ELECTRIC', 'electric', 'electricity')

UPDATE [dbo].[ConfigPrice]
SET type = 'water'
WHERE type IN ('WATER', 'water')

UPDATE [dbo].[ConfigPrice]
SET type = 'room'
WHERE type IN ('ROOM', 'room')

-- 5. Xóa dữ liệu không hợp lệ (nếu có)
PRINT '=== BƯỚC 5: XÓA DỮ LIỆU KHÔNG HỢP LỆ ==='
DELETE FROM [dbo].[ConfigPrice]
WHERE type NOT IN ('electricity', 'water', 'room')

-- 6. Add new constraint with correct values
PRINT '=== BƯỚC 6: THÊM CONSTRAINT MỚI ==='
ALTER TABLE [dbo].[ConfigPrice] 
ADD CONSTRAINT [chk_price_type] 
CHECK ([type] IN ('electricity', 'water', 'room'))

PRINT 'Đã thêm constraint mới: chk_price_type với các giá trị đúng'

-- 7. Kiểm tra lại dữ liệu sau khi cập nhật
PRINT '=== BƯỚC 7: KIỂM TRA DỮ LIỆU SAU KHI CẬP NHẬT ==='
SELECT 
    id,
    type,
    unit_price,
    effective_from
FROM [dbo].[ConfigPrice]
ORDER BY id

-- 8. Kiểm tra lại constraint
PRINT '=== BƯỚC 8: KIỂM TRA CONSTRAINT SAU KHI SỬA ==='
SELECT 
    cc.name AS constraint_name,
    cc.definition
FROM sys.check_constraints cc
WHERE cc.name = 'chk_price_type'

PRINT '=== HOÀN THÀNH SỬA LỖI CONFIGPRICE ==='
PRINT 'Bây giờ bạn có thể chạy ứng dụng và test chức năng khởi tạo giá mặc định' 