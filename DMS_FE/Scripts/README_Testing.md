# Testing Guide for Request System

## Issue Fixed: "Manager not found" Error

The error "Manager not found" was occurring because:
1. The system was hardcoding `managerId = 1` in forms
2. User with ID = 1 might not exist or might not be a Manager
3. The backend API validates that the Manager ID exists in the database

## Solution Implemented

1. **Manager ID from Session**: Now the system gets the Manager ID from the user's session instead of hardcoding
2. **Removed Form Parameters**: Removed `managerId` from all forms since it's now retrieved from session
3. **Better Error Handling**: Added proper validation and error messages

## Testing Steps

### 1. Create Test Data
Run the SQL script `TestData.sql` in your database to create test users:

```sql
-- This will create:
-- Students: Test Student 1 (ID: varies), Test Student 2 (ID: varies)
-- Managers: Test Manager 1 (ID: varies), Test Manager 2 (ID: varies)
```

### 2. Test Manager Login
1. Login as a Manager (use one of the test manager emails)
2. Navigate to "Quản Lý Yêu Cầu" (Request Management)
3. You should see all requests

### 3. Test Approve/Reject Functionality
1. Find a request with "Pending" status
2. Click "Approve" or "Reject" button
3. The system should:
   - Get your Manager ID from session
   - Update the request status
   - Set you as the manager who approved/rejected
   - Show success message

### 4. Test Student Functionality
1. Login as a Student
2. Navigate to "Yêu Cầu & Hỗ Trợ" (Requests & Support)
3. Create a new request
4. View your requests

## Expected Behavior

### Manager Side:
- ✅ Can view all requests
- ✅ Can approve/reject pending requests
- ✅ Manager ID is automatically set from session
- ✅ Success/error messages are displayed
- ✅ Request status updates correctly

### Student Side:
- ✅ Can view only their own requests
- ✅ Can create new requests
- ✅ Cannot approve/reject requests (no action buttons)
- ✅ Proper navigation between pages

## Troubleshooting

### If you still get "Manager not found":
1. Check if you're logged in as a Manager
2. Verify the Manager exists in the database
3. Check the session contains the correct UserId
4. Run the TestData.sql script to create test users

### If approve/reject doesn't work:
1. Check browser console for JavaScript errors
2. Check server logs for API errors
3. Verify the request status is "Pending"
4. Ensure you're logged in with a valid session

## Database Verification

To verify the data is correct, run:

```sql
-- Check all users
SELECT Id, Name, Email, Role FROM Users ORDER BY Role, Id;

-- Check requests with manager info
SELECT 
    r.Id,
    r.Type,
    r.Status,
    s.Name as StudentName,
    m.Name as ManagerName,
    r.CreatedAt
FROM Requests r
LEFT JOIN Users s ON r.StudentId = s.Id
LEFT JOIN Users m ON r.ManagerId = m.Id
ORDER BY r.CreatedAt DESC;
``` 