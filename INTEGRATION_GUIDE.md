# 🎯 **HƯỚNG DẪN TÍCH HỢP VÀ SỬ DỤNG HỆ THỐNG**

## ✅ **HOÀN THÀNH TÍCH HỢP BƯỚC 1**

### 📋 **Các chức năng đã tích hợp:**

1. **🔄 API Backend (DMS.API)**
   - ✅ FeeController - Quản lý phí
   - ✅ TransactionController - Quản lý giao dịch
   - ✅ UtilityController - Quản lý tiện ích
   - ✅ RequestController - Quản lý yêu cầu
   - ✅ ConfigPriceController - Cấu hình giá

2. **🖥️ Frontend Controllers (DMS_FE)**
   - ✅ FeeController - Giao diện quản lý phí
   - ✅ TransactionController - Giao diện quản lý giao dịch
   - ✅ UtilityController - Giao diện quản lý tiện ích
   - ✅ ConfigPriceController - Giao diện cấu hình giá

3. **🎨 Layout & Navigation**
   - ✅ Cập nhật _Layout.cshtml với menu mới
   - ✅ Thêm menu Quản lý Tiện ích
   - ✅ Thêm menu Cấu hình Hệ thống
   - ✅ Cải thiện menu Quản lý Phí & Giao dịch

4. **📱 Views**
   - ✅ Fee/Manage.cshtml - Quản lý phí
   - ✅ ConfigPrice/Manage.cshtml - Cấu hình giá

## 🚀 **HƯỚNG DẪN SỬ DỤNG**

### **1. Khởi động hệ thống**

```bash
# 1. Chạy DMS_BE project
cd DMS_BE
dotnet run

# 2. Truy cập Frontend
http://localhost:5000 (hoặc port được cấu hình)
```

### **2. Khởi tạo hệ thống lần đầu**

1. **Đăng nhập với tài khoản Manager**
2. **Vào menu "Cấu hình Hệ thống" → "Khởi tạo Giá Mặc định"**
3. **Click "Khởi tạo" để tạo các giá mặc định:**
   - Room: 1,500,000 VND/tháng
   - Electric: 3,500 VND/kWh
   - Water: 15,000 VND/m³
   - Internet: 100,000 VND/tháng
   - Cleaning: 50,000 VND/tháng

### **3. Quy trình sử dụng cơ bản**

#### **A. Quản lý Phí**
1. **Vào menu "Quản lý Phí & Giao dịch" → "Quản lý Phí"**
2. **Tạo phí mới:**
   - Click "Tạo Phí Mới"
   - Chọn sinh viên
   - Chọn loại phí (Room, Electric, Water, etc.)
   - Nhập số tiền
   - Đặt hạn thanh toán
3. **Theo dõi phí:**
   - Xem danh sách phí
   - Lọc theo trạng thái
   - Xem phí quá hạn

#### **B. Quản lý Giao dịch**
1. **Vào menu "Quản lý Phí & Giao dịch" → "Quản lý Giao dịch"**
2. **Tạo giao dịch thanh toán:**
   - Click "Tạo Giao dịch Mới"
   - Chọn phí cần thanh toán
   - Nhập tên người thanh toán
   - Nhập số tiền
   - Hệ thống tự động cập nhật trạng thái phí thành "paid"

#### **C. Quản lý Tiện ích**
1. **Vào menu "Quản lý Tiện ích" → "Quản lý Chỉ Số"**
2. **Thêm chỉ số tiện ích:**
   - Click "Thêm Chỉ Số Tiện ích"
   - Chọn phòng
   - Nhập tháng (yyyy-MM)
   - Nhập chỉ số điện và nước
3. **Tính hóa đơn:**
   - Vào "Tính Hóa Đơn Tiện ích"
   - Chọn phòng và tháng
   - Hệ thống tự động tính toán dựa trên giá đã cấu hình

#### **D. Cấu hình Giá**
1. **Vào menu "Cấu hình Hệ thống" → "Cấu hình Giá"**
2. **Chỉnh sửa giá:**
   - Click nút "Sửa" bên cạnh loại giá cần thay đổi
   - Nhập giá mới
   - Lưu thay đổi
3. **Thêm loại giá mới:**
   - Click "Thêm Cấu hình Mới"
   - Nhập tên loại và giá

## 🔧 **API Endpoints**

### **Fee Management**
```http
GET    /api/fee                    # Lấy danh sách phí
GET    /api/fee/student/{id}       # Lấy phí theo sinh viên
GET    /api/fee/overdue           # Lấy phí quá hạn
POST   /api/fee                   # Tạo phí mới
PUT    /api/fee/{id}              # Cập nhật phí
DELETE /api/fee/{id}              # Xóa phí
```

### **Transaction Management**
```http
GET    /api/transaction           # Lấy danh sách giao dịch
GET    /api/transaction/fee/{id}  # Lấy giao dịch theo phí
GET    /api/transaction/summary   # Tổng kết giao dịch
POST   /api/transaction           # Tạo giao dịch mới
```

### **Utility Management**
```http
GET    /api/utility/room/{id}     # Lấy chỉ số theo phòng
GET    /api/utility/bill/{roomId}/{month}  # Tính hóa đơn
POST   /api/utility               # Thêm chỉ số mới
```

### **Config Price**
```http
GET    /api/configprice           # Lấy cấu hình giá
POST   /api/configprice/initialize # Khởi tạo giá mặc định
```

## 📊 **Cấu trúc Database**

### **Các bảng chính:**
- `Fees` - Lưu thông tin phí
- `Transactions` - Lưu giao dịch thanh toán
- `UtilityReadings` - Lưu chỉ số tiện ích
- `ConfigPrices` - Lưu cấu hình giá
- `Requests` - Lưu yêu cầu từ sinh viên

### **Mối quan hệ:**
- `Fee` ↔ `Transaction` (1:N)
- `Fee` ↔ `User` (N:1) - Sinh viên
- `UtilityReading` ↔ `Room` (N:1)
- `Request` ↔ `User` (N:1) - Sinh viên tạo yêu cầu

## 🎯 **Bước tiếp theo**

Sau khi hoàn thành tích hợp Bước 1, bạn có thể:

1. **Test các chức năng cơ bản**
2. **Tạo thêm Views cho các chức năng khác**
3. **Tiếp tục với Bước 2: Tích hợp VNPAY Payment System**
4. **Cải thiện UI/UX với React Frontend**

## 🔍 **Troubleshooting**

### **Lỗi thường gặp:**

1. **"Chưa cấu hình giá tiện ích"**
   - Giải pháp: Vào "Cấu hình Hệ thống" → "Khởi tạo Giá Mặc định"

2. **"Phí không tồn tại"**
   - Giải pháp: Kiểm tra ID phí hoặc tạo phí mới trước

3. **"Đã có chỉ số cho phòng này trong tháng"**
   - Giải pháp: Chọn tháng khác hoặc cập nhật chỉ số hiện có

## 📞 **Hỗ trợ**

Nếu gặp vấn đề, hãy kiểm tra:
1. Database connection
2. API endpoints
3. Log files
4. Browser console errors 