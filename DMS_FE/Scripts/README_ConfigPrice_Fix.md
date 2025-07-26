# 🔧 HƯỚNG DẪN SỬA LỖI CONFIGPRICE CONSTRAINT

## 🚨 Vấn đề
Lỗi SQL constraint: `The INSERT statement conflicted with the CHECK constraint "chk_price_type"`

## 🔍 Nguyên nhân
- Database constraint chỉ chấp nhận các giá trị: `'electricity', 'water', 'room'`
- Có thể có dữ liệu cũ trong database đang sử dụng giá trị khác
- Hoặc có nơi nào đó đang tạo ConfigPrice với giá trị không đúng

## ✅ Giải pháp

### Bước 1: Kiểm tra dữ liệu hiện tại
Chạy script `FixConfigPriceData.sql` để kiểm tra dữ liệu trong database:

```sql
-- Mở SQL Server Management Studio hoặc Azure Data Studio
-- Chạy file: DMS_FE/Scripts/FixConfigPriceData.sql
```

### Bước 2: Cập nhật constraint
Chạy script `EnsureConfigPriceConstraint.sql` để đảm bảo constraint đúng:

```sql
-- Chạy file: DMS_FE/Scripts/EnsureConfigPriceConstraint.sql
```

### Bước 3: Chạy migration (nếu cần)
Nếu sử dụng Entity Framework migrations:

```bash
# Trong terminal, chạy:
dotnet ef database update
```

### Bước 4: Kiểm tra lại
Sau khi chạy các script, kiểm tra lại:

1. **Kiểm tra constraint:**
```sql
SELECT 
    cc.name AS constraint_name,
    cc.definition
FROM sys.check_constraints cc
WHERE cc.name = 'chk_price_type'
```

2. **Kiểm tra dữ liệu:**
```sql
SELECT 
    id,
    type,
    unit_price,
    effective_from
FROM [dbo].[ConfigPrice]
ORDER BY id
```

## 🎯 Các giá trị hợp lệ
Database chỉ chấp nhận các giá trị sau:
- `room` - Tiền phòng
- `electricity` - Tiền điện  
- `water` - Tiền nước

## ⚠️ Lưu ý quan trọng
1. **Backup database** trước khi chạy script
2. **Kiểm tra kỹ** dữ liệu trước và sau khi chạy script
3. **Đảm bảo** tất cả code đều sử dụng đúng giá trị
4. **Test lại** chức năng khởi tạo giá mặc định

## 🔄 Quy trình test
1. Chạy script sửa lỗi
2. Vào menu "Cấu hình Hệ thống" → "Khởi tạo Giá Mặc định"
3. Click "Khởi tạo" để tạo giá mặc định
4. Kiểm tra xem có lỗi không
5. Vào menu "Cấu hình Hệ thống" → "Cấu hình Giá" để xem kết quả

## 📞 Hỗ trợ
Nếu vẫn gặp lỗi, hãy:
1. Kiểm tra log lỗi chi tiết
2. Kiểm tra dữ liệu trong database
3. Đảm bảo đã chạy đúng các script
4. Kiểm tra version của Entity Framework 