# HƯỚNG DẪN HOÀN THÀNH MICROSERVICES MIGRATION

## ✅ Đã Hoàn Thành

### IdentityService
- [x] Tạo cấu trúc thư mục
- [x] Tạo Models (User, Role, RefreshTokens)
- [x] Tạo IdentityDbContext
- [x] Tạo .csproj files (API, BLL, DAL)
- [x] Tạo Program.cs
- [x] Tạo appsettings.json
- [x] Tạo RoleConstants

### AcademicService
- [x] Tạo cấu trúc thư mục cơ bản
- [x] Tạo .csproj files (API, BLL, DAL)
- [x] Tạo Program.cs
- [x] Tạo appsettings.json

### Scripts
- [x] Create-Microservices-Structure.ps1
- [x] Update-Namespaces.ps1

---

## 🔄 CẦN THỰC HIỆN

### Bước 1: Chạy Scripts để Copy Files

Mở PowerShell trong thư mục `d:\K8\PRN232\PRN232_Final_BE`:

```powershell
# Bước 1: Copy tất cả files cần thiết
.\scripts\Create-Microservices-Structure.ps1

# Bước 2: Update namespaces
.\scripts\Update-Namespaces.ps1
```

---

### Bước 2: Tạo AcademicDbContext

File: `AcademicService.DAL\Models\AcademicDbContext.cs`

```csharp
#nullable disable
using System;
using Microsoft.EntityFrameworkCore;

namespace AcademicService.DAL.Models
{
    public partial class AcademicDbContext : DbContext
    {
        public AcademicDbContext()
        {
        }

        public AcademicDbContext(DbContextOptions<AcademicDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public virtual DbSet<Criteria> Criteria { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<Semester> Semesters { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Submission> Submissions { get; set; }
        public virtual DbSet<TeacherAssignment> TeacherAssignments { get; set; }
        public virtual DbSet<Violation> Violations { get; set; }
        public virtual DbSet<ScoreColumn> ScoreColumns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Copy toàn bộ configuration từ EzyFix.Data\Models\AppDbContext.cs
            // NHƯNG BỎ QUA các entities liên quan đến Identity (User, Role, RefreshTokens)
            
            // Example: Student configuration
            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.StudentId).HasDefaultValueSql("gen_random_uuid()");
                entity.HasIndex(e => e.StudentMSSV).IsUnique();
            });

            // ... tiếp tục với các entities khác

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
```

---

### Bước 3: Tạo launchSettings.json

**IdentityService:** `IdentityService.API\Properties\launchSettings.json`

```json
{
  "profiles": {
    "IdentityService.API": {
      "commandName": "Project",
      "launchBrowser": true,
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "applicationUrl": "https://localhost:5001;http://localhost:5000"
    }
  }
}
```

**AcademicService:** `AcademicService.API\Properties\launchSettings.json`

```json
{
  "profiles": {
    "AcademicService.API": {
      "commandName": "Project",
      "launchBrowser": true,
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "applicationUrl": "https://localhost:5002;http://localhost:5003"
    }
  }
}
```

---

### Bước 4: Update Solution File

Thêm 6 projects mới vào `EzyFix.sln`:

```powershell
# Mở terminal tại root folder
cd d:\K8\PRN232\PRN232_Final_BE

# Add projects vào solution
dotnet sln add IdentityService.API\IdentityService.API.csproj
dotnet sln add IdentityService.BLL\IdentityService.BLL.csproj
dotnet sln add IdentityService.DAL\IdentityService.DAL.csproj
dotnet sln add AcademicService.API\AcademicService.API.csproj
dotnet sln add AcademicService.BLL\AcademicService.BLL.csproj
dotnet sln add AcademicService.DAL\AcademicService.DAL.csproj
```

---

### Bước 5: Fix Namespaces Thủ Công

Một số files có thể cần update thủ công:

1. **BaseService.cs** - cần generic DbContext parameter
2. **GenericRepository.cs** - update DbContext reference
3. **UnitOfWork.cs** - update DbContext reference
4. **Các Mapper files** - update namespaces

---

### Bước 6: Cấu hình Database

**Option 1: Shared Database (Đơn giản hơn)**

Cả 2 services dùng chung database `EzyFix`:
- IdentityService chỉ access: Users, Roles, RefreshTokens
- AcademicService chỉ access: các tables còn lại

```json
// appsettings.json cho cả 2 services
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=EzyFix;Username=postgres;Password=123456"
}
```

**Option 2: Separate Databases (Best Practice)**

Tạo 2 databases riêng:

```sql
-- Create Identity Database
CREATE DATABASE EzyFix_Identity;

-- Create Academic Database  
CREATE DATABASE EzyFix_Academic;
```

Update connection strings:

```json
// IdentityService/appsettings.json
"DefaultConnection": "...;Database=EzyFix_Identity;..."

// AcademicService/appsettings.json
"DefaultConnection": "...;Database=EzyFix_Academic;..."
```

---

### Bước 7: Migration Database

**Cho IdentityService:**

```powershell
cd IdentityService.API
dotnet ef migrations add InitialCreate --project ..\IdentityService.DAL
dotnet ef database update
```

**Cho AcademicService:**

```powershell
cd AcademicService.API
dotnet ef migrations add InitialCreate --project ..\AcademicService.DAL
dotnet ef database update
```

---

### Bước 8: Build và Test

```powershell
# Build toàn bộ solution
dotnet build

# Run IdentityService
cd IdentityService.API
dotnet run

# Run AcademicService (terminal khác)
cd AcademicService.API
dotnet run
```

Test endpoints:
- IdentityService: https://localhost:5001/swagger
- AcademicService: https://localhost:5002/swagger

---

## 🐛 Troubleshooting

### Lỗi Namespace

Nếu còn lỗi về namespace:
1. Mở file có lỗi
2. Replace `EzyFix` → `IdentityService` hoặc `AcademicService`
3. Replace `AppDbContext` → `IdentityDbContext` hoặc `AcademicDbContext`

### Lỗi Reference

Nếu thiếu reference:
```powershell
dotnet add reference ..\OtherProject\OtherProject.csproj
```

### Lỗi Migration

Nếu migration fail:
1. Xóa folder Migrations
2. Tạo lại migration: `dotnet ef migrations add InitialCreate`

---

## 📝 Checklist Cuối Cùng

### IdentityService
- [ ] Build thành công
- [ ] Swagger UI hoạt động
- [ ] Test Google Login endpoint
- [ ] Test Get Roles endpoint
- [ ] Test RefreshToken endpoint

### AcademicService  
- [ ] Build thành công
- [ ] Swagger UI hoạt động
- [ ] Test Get Exams endpoint
- [ ] Test Get Students endpoint
- [ ] Test Get Subjects endpoint
- [ ] Test Get Semesters endpoint

### Integration
- [ ] AcademicService có thể validate JWT từ IdentityService
- [ ] CORS configuration đúng
- [ ] Database connections hoạt động

---

## 🚀 Next Steps (Optional)

1. **API Gateway**: Setup Ocelot hoặc YARP
2. **Docker**: Tạo Dockerfile cho mỗi service
3. **Docker Compose**: Orchestrate tất cả services
4. **Health Checks**: Add health check endpoints
5. **Logging**: Centralized logging (Serilog + Seq)
6. **Service Discovery**: Consul hoặc Eureka

---

## 📧 Support

Nếu gặp vấn đề:
1. Check file `MICROSERVICES_STRUCTURE.md`
2. Check build errors trong VS Code
3. Check logs trong terminal
4. Đảm bảo tất cả dependencies đã được restore: `dotnet restore`

Good luck! 🎉
