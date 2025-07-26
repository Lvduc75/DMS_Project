-- Script để kiểm tra và sửa dữ liệu ConfigPrice trong database
-- Chạy script này trực tiếp trong SQL Server Management Studio hoặc Azure Data Studio

USE [dorm_management]
GO

-- 1. Kiểm tra dữ liệu hiện tại trong bảng ConfigPrice
PRINT '=== KIỂM TRA DỮ LIỆU HIỆN TẠI ==='
SELECT 
    id,
    type,
    unit_price,
    effective_from
FROM [dbo].[ConfigPrice]
ORDER BY id

-- 2. Kiểm tra constraint hiện tại
PRINT '=== KIỂM TRA CONSTRAINT HIỆN TẠI ==='
SELECT 
    cc.name AS constraint_name,
    cc.definition
FROM sys.check_constraints cc
WHERE cc.name = 'chk_price_type'

-- 3. Cập nhật dữ liệu để phù hợp với schema mới
PRINT '=== CẬP NHẬT DỮ LIỆU THEO SCHEMA MỚI ==='
UPDATE [dbo].[ConfigPrice]
SET type = 'electricity'
WHERE type IN ('ELECTRIC', 'electric', 'electricity')

UPDATE [dbo].[ConfigPrice]
SET type = 'water'
WHERE type IN ('WATER', 'water')

UPDATE [dbo].[ConfigPrice]
SET type = 'room'
WHERE type IN ('ROOM', 'room')

-- 4. Kiểm tra lại dữ liệu sau khi cập nhật
PRINT '=== KIỂM TRA DỮ LIỆU SAU KHI CẬP NHẬT ==='
SELECT 
    id,
    type,
    unit_price,
    effective_from
FROM [dbo].[ConfigPrice]
ORDER BY id

-- 5. Xóa dữ liệu không hợp lệ (nếu có)
PRINT '=== XÓA DỮ LIỆU KHÔNG HỢP LỆ ==='
DELETE FROM [dbo].[ConfigPrice]
WHERE type NOT IN ('electricity', 'water', 'room')

-- 6. Kiểm tra lại constraint
PRINT '=== KIỂM TRA CONSTRAINT SAU KHI SỬA ==='
SELECT 
    cc.name AS constraint_name,
    cc.definition
FROM sys.check_constraints cc
WHERE cc.name = 'chk_price_type'

PRINT '=== HOÀN THÀNH KIỂM TRA VÀ SỬA DỮ LIỆU ===' 