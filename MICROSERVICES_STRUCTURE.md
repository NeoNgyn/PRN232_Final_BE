# Cấu Trúc Microservices - EzyFix

## Tổng Quan
Project đã được tái cấu trúc thành 2 microservices:

### 1. IdentityService (Port: 5001)
Quản lý xác thực, phân quyền và người dùng

**Controllers:**
- AuthController - Google Login, RefreshToken
- RolesController - CRUD Roles

**Services:**
- AuthService - Xác thực Google OAuth
- RoleService - Quản lý vai trò
- RefreshTokensService - Quản lý refresh tokens

**Models:**
- User
- Role  
- RefreshTokens

---

### 2. AcademicService (Port: 5002)
Quản lý học vụ, bài thi, sinh viên, môn học

**Controllers:**
- ExamsController - Quản lý bài thi
- SemestersController - Quản lý học kỳ
- StudentsController - Quản lý sinh viên
- SubjectsController - Quản lý môn học
- FilesController - Upload/Export files

**Services:**
- ExamService
- SemesterService
- StudentService
- SubjectService
- FileService
- CloudinaryService

**Models:**
- Exam
- Semester
- Student
- Subject
- Criteria
- Grade
- ScoreColumn
- Submission
- TeacherAssignment
- Violation

---

## Cấu Trúc Thư Mục

```
PRN232_Final_BE/
│
├── IdentityService.API/          # API Layer cho Identity
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── RolesController.cs
│   │   └── BaseController.cs
│   ├── Constants/
│   ├── Middlewares/
│   ├── Extensions/
│   ├── Properties/
│   ├── Program.cs
│   ├── appsettings.json
│   └── IdentityService.API.csproj
│
├── IdentityService.BLL/          # Business Logic cho Identity
│   ├── Services/
│   │   ├── Implements/
│   │   │   ├── AuthService.cs
│   │   │   ├── RoleService.cs
│   │   │   └── RefreshTokensService.cs
│   │   ├── Interfaces/
│   │   │   ├── IAuthService.cs
│   │   │   ├── IRoleService.cs
│   │   │   └── IRefreshTokensService.cs
│   │   └── BaseService.cs
│   ├── Utils/
│   │   ├── JwtUtil.cs
│   │   ├── IJwtUtil.cs
│   │   ├── PasswordUtil.cs
│   │   └── OtpUtil.cs
│   └── IdentityService.BLL.csproj
│
├── IdentityService.DAL/          # Data Access cho Identity
│   ├── Models/
│   │   ├── User.cs
│   │   ├── Role.cs
│   │   ├── RefreshTokens.cs
│   │   └── IdentityDbContext.cs
│   ├── Repositories/
│   │   ├── Interfaces/
│   │   └── Implements/
│   ├── Data/
│   │   ├── Requests/
│   │   │   ├── Auth/
│   │   │   └── Roles/
│   │   ├── Responses/
│   │   │   ├── Auth/
│   │   │   └── Roles/
│   │   ├── MetaDatas/
│   │   └── Exceptions/
│   ├── Mappers/
│   └── IdentityService.DAL.csproj
│
├── AcademicService.API/          # API Layer cho Academic
│   ├── Controllers/
│   │   ├── ExamsController.cs
│   │   ├── SemestersController.cs
│   │   ├── StudentsController.cs
│   │   ├── SubjectsController.cs
│   │   ├── FilesController.cs
│   │   └── BaseController.cs
│   ├── Constants/
│   ├── Middlewares/
│   ├── Extensions/
│   ├── Properties/
│   ├── Program.cs
│   ├── appsettings.json
│   └── AcademicService.API.csproj
│
├── AcademicService.BLL/          # Business Logic cho Academic
│   ├── Services/
│   │   ├── Implements/
│   │   │   ├── ExamService.cs
│   │   │   ├── SemesterService.cs
│   │   │   ├── StudentService.cs
│   │   │   ├── SubjectService.cs
│   │   │   ├── FileService.cs
│   │   │   └── CloudinaryService.cs
│   │   ├── Interfaces/
│   │   └── BaseService.cs
│   └── AcademicService.BLL.csproj
│
├── AcademicService.DAL/          # Data Access cho Academic  
│   ├── Models/
│   │   ├── Exam.cs
│   │   ├── Semester.cs
│   │   ├── Student.cs
│   │   ├── Subject.cs
│   │   ├── Criteria.cs
│   │   ├── Grade.cs
│   │   ├── ScoreColumn.cs
│   │   ├── Submission.cs
│   │   ├── TeacherAssignment.cs
│   │   ├── Violation.cs
│   │   └── AcademicDbContext.cs
│   ├── Repositories/
│   ├── Data/
│   ├── Mappers/
│   └── AcademicService.DAL.csproj
│
└── EzyFix.sln                    # Solution file (updated)
```

---

## Các Bước Đã Thực Hiện

### ✅ Bước 1: Tạo Cấu Trúc IdentityService
- [x] Tạo thư mục IdentityService.API, IdentityService.BLL, IdentityService.DAL
- [x] Tạo Models: User, Role, RefreshTokens
- [x] Tạo IdentityDbContext với 3 tables
- [x] Tạo file .csproj cho IdentityService.DAL

### 🔄 Bước 2-12: Cần Hoàn Thành

**Bước 2:** Copy Controllers (AuthController, RolesController, BaseController)
**Bước 3:** Copy Services và Interfaces
**Bước 4:** Copy Repositories (Generic Repository, UnitOfWork)
**Bước 5:** Copy Request/Response DTOs
**Bước 6:** Copy Mappers (MappingProfile)
**Bước 7:** Copy Utils (JwtUtil, PasswordUtil, OtpUtil)
**Bước 8:** Copy Constants, Middlewares, Extensions
**Bước 9:** Tạo Program.cs với DI Container
**Bước 10:** Tạo appsettings.json
**Bước 11:** Tạo IdentityService.API.csproj, IdentityService.BLL.csproj
**Bước 12:** Lặp lại cho AcademicService

---

## Hướng Dẫn Tiếp Theo

### 1. Hoàn Thành IdentityService

Bạn cần copy các files từ EzyFix sang IdentityService với những thay đổi namespace:
- `EzyFix.API` → `IdentityService.API`
- `EzyFix.BLL` → `IdentityService.BLL`
- `EzyFix.DAL` → `IdentityService.DAL`
- `AppDbContext` → `IdentityDbContext`

### 2. Tạo AcademicService

Tương tự như IdentityService nhưng với:
- Controllers: Exams, Semesters, Students, Subjects, Files
- Models: Tất cả models còn lại
- `AppDbContext` → `AcademicDbContext`

### 3. Cập Nhật Connection Strings

**IdentityService - appsettings.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=EzyFix_Identity;..."
  }
}
```

**AcademicService - appsettings.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=EzyFix_Academic;..."
  }
}
```

### 4. Communication Between Services

Khi AcademicService cần xác thực user:
- Gọi API IdentityService để validate JWT token
- Hoặc dùng shared JWT validation middleware

---

## Ports Configuration

- **IdentityService:** http://localhost:5001
- **AcademicService:** http://localhost:5002
- **API Gateway (optional):** http://localhost:5000

---

## Database Strategy

**Option 1: Shared Database (Easier)**
- Cả 2 services dùng chung 1 database
- Mỗi service chỉ access tables của mình

**Option 2: Separate Databases (Microservices Best Practice)**
- IdentityService: `EzyFix_Identity` database
- AcademicService: `EzyFix_Academic` database
- Sync data qua Events/Message Queue nếu cần

---

## Checklist Hoàn Thành

### IdentityService
- [ ] Copy tất cả files cần thiết
- [ ] Update namespaces
- [ ] Tạo Program.cs với DI
- [ ] Tạo appsettings.json
- [ ] Test Auth endpoints
- [ ] Test Role endpoints

### AcademicService  
- [ ] Copy tất cả files cần thiết
- [ ] Update namespaces
- [ ] Tạo Program.cs với DI
- [ ] Tạo appsettings.json
- [ ] Add JWT authentication
- [ ] Test tất cả endpoints

### Solution
- [ ] Add 6 projects mới vào .sln file
- [ ] Configure project dependencies
- [ ] Test build toàn bộ solution
- [ ] Update docker-compose (if needed)

---

## Notes

1. **Shared Code:** Có thể tạo thêm 1 project `Shared` cho code dùng chung (Constants, Extensions, Middlewares)

2. **API Gateway:** Xem xét dùng Ocelot hoặc YARP để tạo API Gateway thống nhất

3. **Service Discovery:** Nếu deploy lên Kubernetes, có thể dùng Service Discovery

4. **Testing:** Tạo riêng test projects cho mỗi service

---

Tôi đã tạo cấu trúc cơ bản cho **IdentityService**. Bạn muốn tôi tiếp tục tạo các files còn lại không?
