# DMS API Documentation

## 🎯 **BƯỚC 1: QUẢN LÝ PHÍ VÀ GIAO DỊCH**

### 📋 **1. ConfigPrice API - Quản lý Cấu hình Giá**

#### Khởi tạo giá mặc định
```http
POST /api/configprice/initialize
```
Khởi tạo các giá mặc định cho hệ thống:
- Room: 1,500,000 VND/tháng
- Electric: 3,500 VND/kWh
- Water: 15,000 VND/m³
- Internet: 100,000 VND/tháng
- Cleaning: 50,000 VND/tháng

#### Lấy tất cả cấu hình giá
```http
GET /api/configprice
```

#### Lấy giá theo loại
```http
GET /api/configprice/type/{type}
```

#### Tạo cấu hình giá mới
```http
POST /api/configprice
Content-Type: application/json

{
  "type": "NewService",
  "price": 200000
}
```

#### Cập nhật giá
```http
PUT /api/configprice/{id}
Content-Type: application/json

{
  "type": "Electric",
  "price": 4000
}
```

### 💰 **2. Fee API - Quản lý Phí**

#### Tạo phí mới
```http
POST /api/fee
Content-Type: application/json

{
  "studentId": 2,
  "type": "Room",
  "amount": 1500000,
  "status": "unpaid",
  "dueDate": "2025-02-15"
}
```

#### Lấy phí theo sinh viên
```http
GET /api/fee/student/{studentId}
```

#### Lấy phí theo tháng và loại
```http
GET /api/fee?month=2025-01&type=Room
```

#### Lấy phí quá hạn
```http
GET /api/fee/overdue
```

#### Cập nhật trạng thái phí
```http
PUT /api/fee/{id}/status
Content-Type: application/json

"paid"
```

### 💳 **3. Transaction API - Quản lý Giao dịch**

#### Tạo giao dịch thanh toán
```http
POST /api/transaction
Content-Type: application/json

{
  "feeId": 1,
  "payerName": "Nguyen Van A",
  "amount": 1500000
}
```

#### Lấy giao dịch theo phí
```http
GET /api/transaction/fee/{feeId}
```

#### Lấy giao dịch theo sinh viên
```http
GET /api/transaction/student/{studentId}
```

#### Lấy tổng kết giao dịch
```http
GET /api/transaction/summary?fromDate=2025-01-01&toDate=2025-01-31
```

### ⚡ **4. Utility API - Quản lý Tiện ích**

#### Thêm chỉ số tiện ích
```http
POST /api/utility
Content-Type: application/json

{
  "roomId": 1,
  "readingMonth": "2025-01",
  "electric": 150.5,
  "water": 25.3
}
```

#### Lấy chỉ số theo phòng
```http
GET /api/utility/room/{roomId}
```

#### Lấy chỉ số theo tháng
```http
GET /api/utility/month/{monthStr}
```

#### Tính hóa đơn tiện ích
```http
GET /api/utility/bill/{roomId}/{month}
```

### 📝 **5. Request API - Quản lý Yêu cầu**

#### Tạo yêu cầu mới
```http
POST /api/request
Content-Type: application/json

{
  "studentId": 2,
  "type": "Maintenance",
  "description": "Air conditioner not working properly"
}
```

#### Lấy tất cả yêu cầu
```http
GET /api/request
```

#### Lấy yêu cầu theo sinh viên
```http
GET /api/request/user/{userId}
```

#### Lấy yêu cầu theo trạng thái
```http
GET /api/request/status/{status}
```

#### Cập nhật trạng thái yêu cầu
```http
PUT /api/request/{id}/status
Content-Type: application/json

"Approved"
```

## 🚀 **HƯỚNG DẪN SỬ DỤNG**

### 1. **Khởi tạo hệ thống**
```bash
# 1. Khởi tạo giá mặc định
POST /api/configprice/initialize

# 2. Tạo phí cho sinh viên
POST /api/fee
{
  "studentId": 2,
  "type": "Room",
  "amount": 1500000,
  "status": "unpaid",
  "dueDate": "2025-02-15"
}
```

### 2. **Quy trình thanh toán**
```bash
# 1. Sinh viên xem phí
GET /api/fee/student/2

# 2. Tạo giao dịch thanh toán
POST /api/transaction
{
  "feeId": 1,
  "payerName": "Nguyen Van A",
  "amount": 1500000
}

# 3. Phí tự động chuyển sang trạng thái "paid"
```

### 3. **Quản lý tiện ích**
```bash
# 1. Nhập chỉ số điện/nước
POST /api/utility
{
  "roomId": 1,
  "readingMonth": "2025-01",
  "electric": 150.5,
  "water": 25.3
}

# 2. Tính hóa đơn
GET /api/utility/bill/1/2025-01
```

### 4. **Xử lý yêu cầu**
```bash
# 1. Sinh viên tạo yêu cầu
POST /api/request
{
  "studentId": 2,
  "type": "Maintenance",
  "description": "Air conditioner not working properly"
}

# 2. Manager xem yêu cầu
GET /api/request/status/Pending

# 3. Manager phê duyệt
PUT /api/request/1/status
"Approved"
```

## 📊 **Các Trạng thái**

### Phí (Fee Status)
- `unpaid`: Chưa thanh toán
- `paid`: Đã thanh toán

### Yêu cầu (Request Status)
- `Pending`: Chờ xử lý
- `Approved`: Đã phê duyệt
- `Rejected`: Từ chối
- `Completed`: Hoàn thành

## 🔧 **Lưu ý Kỹ thuật**

1. **Validation**: Tất cả API đều có validation đầy đủ
2. **Error Handling**: Trả về HTTP status codes phù hợp
3. **Data Consistency**: Tự động cập nhật trạng thái liên quan
4. **Audit Trail**: Ghi lại thời gian tạo/cập nhật

## 🎯 **Bước tiếp theo**

Sau khi hoàn thành Bước 1, chúng ta sẽ tiếp tục với:
- **Bước 2**: Tích hợp VNPAY Payment System
- **Bước 3**: Cải thiện Frontend với React
- **Bước 4**: Thêm báo cáo và thống kê 