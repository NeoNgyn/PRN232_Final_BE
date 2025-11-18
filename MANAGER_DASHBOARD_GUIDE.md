# Hướng dẫn sử dụng tính năng Manager Dashboard

## Tổng quan
Tính năng Manager Dashboard cho phép quản lý phân công chấm bài cho giảng viên bằng cách upload file RAR chứa bài nộp của sinh viên.

## Các thành phần đã triển khai

### Backend (PRN232_Final_BE)

#### 1. API Endpoints

**a. Lấy danh sách giảng viên**
```
GET /api/v1/Users/teachers
```
- Trả về danh sách tất cả giảng viên (users có RoleId)
- Response: Array of UserResponse

**b. Upload và giải nén file RAR**
```
POST /api/v1/Files/extract-rar
Content-Type: multipart/form-data

Body:
- RARFile: File RAR chứa bài nộp
- ExamId: GUID của kỳ thi
- ExaminerId: GUID của giảng viên chấm bài
```

#### 2. Cải tiến ExtractRarAndCreateSubmissions

**Tính năng:**
- Tự động parse StudentId từ tên file theo các pattern:
  - `SE123456_TenSinhVien.docx` → StudentId: SE123456
  - `HE123456_TenSinhVien.docx` → StudentId: HE123456
  - `123456_TenSinhVien.docx` → StudentId: 123456
  
- Xử lý lỗi chi tiết:
  - Logging đầy đủ mỗi bước xử lý
  - Trả về danh sách file thành công và thất bại
  - Tự động cleanup temp files
  
- Response format:
```json
{
  "statusCode": 200,
  "message": "RAR extracted and submissions created",
  "data": {
    "message": "RAR file extracted and processed",
    "totalFiles": 10,
    "successfulSubmissions": 8,
    "failedFiles": 2,
    "submissions": [...],
    "errors": [
      "Không thể trích xuất mã sinh viên từ file: InvalidFileName.docx"
    ]
  }
}
```

#### 3. Model Updates

**CreateSubmissionRequest** - Đã thêm StudentId:
```csharp
public class CreateSubmissionRequest
{
    [Required]
    public Guid ExamId { get; set; }
    
    [Required]
    public Guid ExaminerId { get; set; }
    
    public string? StudentId { get; set; }
}
```

**SubmissionService** - Ưu tiên StudentId từ request:
- Nếu request có StudentId → dùng StudentId đó
- Nếu không có → parse từ filename (logic cũ)

### Frontend (student-grading-platform)

#### 1. Manager Service (`src/services/managerService.js`)

**Methods:**
- `getAllTeachers()` - Lấy danh sách giảng viên
- `uploadRarFile(rarFile, examId, examinerId)` - Upload file RAR
- `getAllExams()` - Lấy danh sách kỳ thi
- `getSubmissionsByExam(examId)` - Lấy submissions theo exam
- `getSubmissionsByExaminer(examinerId)` - Lấy submissions theo examiner

#### 2. Manager Dashboard (`src/pages/ManagerDashboard.js`)

**Tính năng:**
- Hiển thị danh sách tất cả giảng viên dạng card
- Mỗi giảng viên có button "Assign bài chấm"
- Modal upload file RAR với form:
  - Chọn giảng viên (auto-filled)
  - Chọn kỳ thi (dropdown)
  - Upload file RAR
  - Hiển thị thông tin file (tên, size)
- Upload progress với 3 trạng thái:
  - Uploading: Spinner + message
  - Success: Check icon + chi tiết kết quả
  - Error: X icon + danh sách lỗi
- Statistics: Số giảng viên, số kỳ thi

#### 3. Styling (`src/pages/ManagerDashboard.css`)

- Responsive design
- Modern gradient buttons
- Card hover effects
- Modal với backdrop blur
- Loading states với animations
- Color-coded status (success: green, error: red, uploading: blue)

#### 4. Routing (`src/App.js`)

**Thêm route mới:**
```javascript
<Route 
  path="/manager" 
  element={
    user && user.role === 'manager' ? (
      <ManagerDashboard user={user} onLogout={handleLogout} />
    ) : (
      <Navigate to="/" />
    )
  } 
/>
```

**Cập nhật login redirect:**
- Admin → /admin
- Manager → /manager
- Teacher → /teacher

#### 5. API Configuration (`src/config/api.js`)

**Thêm endpoints:**
```javascript
USERS: {
  GET_ALL: '/api/v1/Users',
  GET_TEACHERS: '/api/v1/Users/teachers',
  GET_BY_ID: (id) => `/api/v1/Users/${id}`
},
FILES: {
  EXTRACT_RAR: '/api/v1/Files/extract-rar',
  IMPORT_STUDENT: '/api/v1/Files/import-student',
  IMPORT_CRITERIA: '/api/v1/Files/import-criteria'
},
EXAMS: {
  GET_ALL: '/api/v1/Exams',
  GET_BY_ID: (id) => `/api/v1/Exams/${id}`
},
SUBMISSIONS: {
  GET_ALL: '/api/v1/Submissions',
  GET_BY_ID: (id) => `/api/v1/Submissions/${id}`
}
```

## Hướng dẫn sử dụng

### 1. Chuẩn bị file RAR

File RAR phải chứa các file DOC/DOCX với format tên:
- `SE123456_NguyenVanA.docx`
- `HE654321_TranThiB.doc`
- `SA999888_LeVanC.docx`

**Lưu ý:**
- Mã sinh viên phải có 2 chữ cái + 6 chữ số (SE123456, HE123456, etc.)
- Hoặc chỉ số (6-8 chữ số)
- Hệ thống sẽ tự động parse mã sinh viên từ tên file

### 2. Đăng nhập với role Manager

Login vào hệ thống với account có role là "manager"

### 3. Truy cập Manager Dashboard

Sau khi login thành công, hệ thống tự động redirect đến `/manager`

### 4. Assign bài chấm cho giảng viên

**Bước 1:** Click button "Assign bài chấm" trên card của giảng viên

**Bước 2:** Trong modal:
- Tên giảng viên tự động điền
- Chọn kỳ thi từ dropdown
- Click "Upload file RAR" và chọn file

**Bước 3:** Kiểm tra thông tin:
- Tên file hiển thị
- Dung lượng file (MB)
- Format hướng dẫn

**Bước 4:** Click "Upload & Assign"

**Bước 5:** Theo dõi progress:
- Upload đang xử lý (spinner)
- Thành công: Hiển thị số file thành công/thất bại
- Lỗi: Hiển thị chi tiết lỗi

### 5. Xem kết quả

Kết quả hiển thị trong modal:
```
✓ Upload thành công!
Tổng file: 10
Thành công: 8
Thất bại: 2
```

Nếu có lỗi:
```
✗ Upload thất bại
Chi tiết lỗi:
• Không thể trích xuất mã sinh viên từ file: BadFile.docx
• Lỗi xử lý file ABC.doc: Invalid format
```

## Xử lý lỗi

### Backend Errors

1. **File không phải RAR**
   - Message: "File must be a .rar archive"
   - Solution: Chỉ upload file .rar

2. **Không parse được StudentId**
   - Message: "Không thể trích xuất mã sinh viên từ file: [filename]"
   - Solution: Đổi tên file theo format đúng

3. **Exam không tồn tại**
   - Message: "Exam not found or existed!"
   - Solution: Chọn exam hợp lệ

### Frontend Errors

1. **Không có giảng viên**
   - Hiển thị: Empty state với icon Users
   - Solution: Thêm users với role Teacher

2. **Không có kỳ thi**
   - Hiển thị: Dropdown trống
   - Solution: Tạo exams trong hệ thống

3. **File quá lớn**
   - Timeout: 2 phút (120 seconds)
   - Solution: Chia nhỏ file RAR

## Testing

### Test Backend API

```bash
# 1. Test get teachers
curl -X GET http://localhost:5000/api/v1/Users/teachers

# 2. Test upload RAR
curl -X POST http://localhost:5000/api/v1/Files/extract-rar \
  -F "RARFile=@submissions.rar" \
  -F "ExamId=your-exam-guid" \
  -F "ExaminerId=your-teacher-guid"
```

### Test Frontend

1. Login với role manager
2. Verify redirect to /manager
3. Check teachers list loading
4. Click assign button
5. Select exam and upload RAR
6. Verify success/error messages

## Database Schema

### Submissions Table
```sql
CREATE TABLE Submissions (
    SubmissionId GUID PRIMARY KEY,
    ExamId GUID NOT NULL,
    StudentId VARCHAR(450),
    ExaminerId GUID NOT NULL,
    SecondExaminerId GUID NULL,
    OriginalFileName VARCHAR(500) NOT NULL,
    FilePath VARCHAR(1000) NOT NULL,
    GradingStatus VARCHAR(20) DEFAULT 'Pending',
    TotalScore DECIMAL(18,2) NULL,
    UploadedAt TIMESTAMP NOT NULL,
    IsApproved BIT DEFAULT 0,
    
    FOREIGN KEY (ExamId) REFERENCES Exams(ExamId),
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId)
);
```

## Troubleshooting

### Issue: Không thấy giảng viên

**Kiểm tra:**
1. Database có users với RoleId không null?
2. API endpoint `/api/v1/Users/teachers` trả về data?
3. CORS settings cho phép request?

**Fix:**
```sql
-- Thêm role Teacher
INSERT INTO Roles (RoleId, RoleName) VALUES (NEWID(), 'Teacher');

-- Assign role cho user
UPDATE Users SET RoleId = 'role-guid-here' WHERE UserId = 'user-guid';
```

### Issue: Upload thất bại

**Kiểm tra:**
1. File size > giới hạn server?
2. Temp folder có permission write?
3. Cloudinary credentials đúng?
4. SharpCompress package installed?

**Fix:**
```bash
# Backend: Install SharpCompress
dotnet add package SharpCompress

# Backend: Increase max file size in appsettings.json
"Kestrel": {
  "Limits": {
    "MaxRequestBodySize": 104857600  // 100 MB
  }
}
```

### Issue: Parse StudentId thất bại

**Kiểm tra format tên file:**
- ✓ SE123456_Name.docx
- ✓ HE123456_Name.doc
- ✓ 12345678_Name.docx
- ✗ Name_Only.docx
- ✗ SE12345_Name.docx (thiếu 1 số)

## Performance Considerations

1. **Large RAR files:**
   - Timeout: 120 seconds
   - Consider chunked upload for files > 50MB
   - Monitor memory usage during extraction

2. **Many files in RAR:**
   - Process sequentially to avoid memory issues
   - Log progress for monitoring
   - Consider batch processing for 100+ files

3. **Database:**
   - Index on ExamId, StudentId, ExaminerId
   - Use transactions for bulk inserts
   - Clean up temp files immediately

## Future Enhancements

1. **Progress tracking:**
   - WebSocket for real-time progress
   - Queue system for large batches

2. **Validation:**
   - Pre-check student IDs against database
   - Validate file content (malware scan)
   - Duplicate detection

3. **Reporting:**
   - Export assignment summary
   - Email notifications to teachers
   - Statistics dashboard

4. **UI/UX:**
   - Drag-and-drop file upload
   - Bulk assignment (multiple teachers)
   - Assignment history view
