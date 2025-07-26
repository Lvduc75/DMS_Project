# Request System - Frontend Implementation

## Overview
The Request System allows students to submit requests and managers to approve/reject them. This system provides a complete workflow for handling various types of requests in the dormitory management system.

## Features

### For Managers:
- **Request Management**: View all requests in a comprehensive table
- **Request Approval**: Approve or reject pending requests
- **Status Tracking**: Filter requests by status (Pending, Approved, Rejected)
- **Request Details**: View detailed information about each request
- **Dashboard Integration**: Statistics widget for quick overview

### For Students:
- **Request Creation**: Submit new requests with different types
- **My Requests**: View and track their own requests
- **Status Tracking**: Monitor the status of submitted requests
- **Request Details**: View detailed information about their requests

## Request Types
- Maintenance Request
- Facility Request
- Room Change Request
- Complaint
- General Inquiry
- Other

## Request Statuses
- **Pending**: Awaiting manager approval
- **Approved**: Request has been approved
- **Rejected**: Request has been rejected

## Files Structure

### Controllers
- `RequestController.cs` - Main controller handling all request operations

### Models
- `RequestCreateViewModel.cs` - ViewModel for creating new requests
- `RequestManageViewModel.cs` - ViewModel for managing requests
- `Request.cs` - Main request model

### Views
- `Manage.cshtml` - Manager view for all requests
- `MyRequests.cshtml` - Student view for their requests
- `Create.cshtml` - Create new request form
- `Details.cshtml` - Request details view
- `Approve.cshtml` - Approve/reject request interface
- `Status.cshtml` - Filter by status view
- `DashboardWidget.cshtml` - Dashboard statistics widget

## API Endpoints Used
- `GET /api/request` - Get all requests
- `GET /api/request/{id}` - Get specific request
- `GET /api/request/user/{userId}` - Get requests by user
- `GET /api/request/status/{status}` - Get requests by status
- `POST /api/request` - Create new request
- `PUT /api/request/{id}` - Update request

## Navigation Integration
The Request System is integrated into both the main layout and student layout:

### Main Layout (Manager)
- Quản Lý Yêu Cầu > Danh Sách Yêu Cầu
- Quản Lý Yêu Cầu > Yêu Cầu Sửa Chữa
- Quản Lý Yêu Cầu > Giải Thích Vi Phạm
- Quản Lý Yêu Cầu > Yêu Cầu Nghỉ Tạm
- Quản Lý Yêu Cầu > Nhắc Nhở Thanh Toán Tự Động

### Student Layout
- Yêu Cầu & Hỗ Trợ > Yêu Cầu Của Tôi
- Yêu Cầu & Hỗ Trợ > Tạo Yêu Cầu Mới
- Yêu Cầu & Hỗ Trợ > Yêu Cầu Sửa Chữa
- Yêu Cầu & Hỗ Trợ > Yêu Cầu Nghỉ Tạm

## Usage

### For Students:
1. Navigate to "Yêu Cầu & Hỗ Trợ" > "Tạo Yêu Cầu Mới"
2. Fill in the request form with type and description
3. Submit the request
4. Track status in "Yêu Cầu Của Tôi"

### For Managers:
1. Navigate to "Quản Lý Yêu Cầu" > "Danh Sách Yêu Cầu"
2. View all requests in the table
3. Click "View" to see details
4. Click "Approve" or "Reject" for pending requests
5. Use status filters to view specific request types

## Technical Details
- Uses Bootstrap 5 for responsive design
- Implements AJAX for dynamic interactions
- Includes form validation
- Provides success/error feedback
- Supports role-based access control
- Integrates with existing API structure

## Future Enhancements
- Email notifications for status changes
- Request templates for common requests
- File attachments for requests
- Request priority levels
- Request escalation workflow
- Mobile-responsive improvements 