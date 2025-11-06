# Script tự động tạo cấu trúc Microservices
# Chạy script này trong PowerShell

$rootPath = "d:\K8\PRN232\PRN232_Final_BE"

Write-Host "=== BẮT ĐẦU TẠO CẤU TRÚC MICROSERVICES ===" -ForegroundColor Green

# ============================================
# PHẦN 1: TẠO THƯ MỤC CHO ACADEMICSERVICE
# ============================================
Write-Host "`n[1/3] Tạo thư mục cho AcademicService..." -ForegroundColor Yellow

$academicFolders = @(
    "AcademicService.API\Controllers",
    "AcademicService.API\Constants",
    "AcademicService.API\Middlewares",
    "AcademicService.API\Extensions",
    "AcademicService.API\Properties",
    "AcademicService.BLL\Services\Implements",
    "AcademicService.BLL\Services\Interfaces",
    "AcademicService.BLL\Utils",
    "AcademicService.DAL\Models",
    "AcademicService.DAL\Repositories\Interfaces",
    "AcademicService.DAL\Repositories\Implements",
    "AcademicService.DAL\Data\Requests\Exams",
    "AcademicService.DAL\Data\Requests\Semesters",
    "AcademicService.DAL\Data\Requests\Students",
    "AcademicService.DAL\Data\Requests\Subjects",
    "AcademicService.DAL\Data\Requests\Files",
    "AcademicService.DAL\Data\Responses\Exams",
    "AcademicService.DAL\Data\Responses\Semesters",
    "AcademicService.DAL\Data\Responses\Students",
    "AcademicService.DAL\Data\Responses\Subjects",
    "AcademicService.DAL\Data\Responses\Files",
    "AcademicService.DAL\Data\MetaDatas",
    "AcademicService.DAL\Data\Exceptions",
    "AcademicService.DAL\Mappers"
)

foreach ($folder in $academicFolders) {
    $fullPath = Join-Path $rootPath $folder
    if (-not (Test-Path $fullPath)) {
        New-Item -ItemType Directory -Path $fullPath -Force | Out-Null
        Write-Host "  ✓ Created: $folder" -ForegroundColor Gray
    }
}

# ============================================
# PHẦN 2: COPY FILES CHO IDENTITYSERVICE
# ============================================
Write-Host "`n[2/3] Copy files cho IdentityService..." -ForegroundColor Yellow

# Controllers
Copy-Item "$rootPath\EzyFix.API\Controllers\AuthController.cs" "$rootPath\IdentityService.API\Controllers\" -Force
Copy-Item "$rootPath\EzyFix.API\Controllers\RolesController.cs" "$rootPath\IdentityService.API\Controllers\" -Force
Copy-Item "$rootPath\EzyFix.API\Controllers\BaseController.cs" "$rootPath\IdentityService.API\Controllers\" -Force

# Constants
Copy-Item "$rootPath\EzyFix.API\Constants\*" "$rootPath\IdentityService.API\Constants\" -Force -Recurse

# Middlewares  
Copy-Item "$rootPath\EzyFix.API\Middlewares\*" "$rootPath\IdentityService.API\Middlewares\" -Force -Recurse

# Extensions
Copy-Item "$rootPath\EzyFix.API\Extensions\*" "$rootPath\IdentityService.API\Extensions\" -Force -Recurse

# Services
$identityServices = @("AuthService.cs", "RoleService.cs", "RefreshTokensService.cs", "BaseService.cs")
foreach ($service in $identityServices) {
    if (Test-Path "$rootPath\EzyFix.Service\Services\Implements\$service") {
        Copy-Item "$rootPath\EzyFix.Service\Services\Implements\$service" "$rootPath\IdentityService.BLL\Services\Implements\" -Force
    }
}

$identityInterfaces = @("IAuthService.cs", "IRoleService.cs", "IRefreshTokensService.cs")
foreach ($interface in $identityInterfaces) {
    if (Test-Path "$rootPath\EzyFix.Service\Services\Interfaces\$interface") {
        Copy-Item "$rootPath\EzyFix.Service\Services\Interfaces\$interface" "$rootPath\IdentityService.BLL\Services\Interfaces\" -Force
    }
}

# Utils
Copy-Item "$rootPath\EzyFix.Service\Utils\*" "$rootPath\IdentityService.BLL\Utils\" -Force -Recurse

# Repositories
Copy-Item "$rootPath\EzyFix.Data\Repositories\Interfaces\*" "$rootPath\IdentityService.DAL\Repositories\Interfaces\" -Force -Recurse
Copy-Item "$rootPath\EzyFix.Data\Repositories\Implements\*" "$rootPath\IdentityService.DAL\Repositories\Implements\" -Force -Recurse

# Data - Requests/Responses cho Auth & Roles
Copy-Item "$rootPath\EzyFix.Data\Data\Requests\Auth\*" "$rootPath\IdentityService.DAL\Data\Requests\Auth\" -Force -Recurse
Copy-Item "$rootPath\EzyFix.Data\Data\Requests\Roles\*" "$rootPath\IdentityService.DAL\Data\Requests\Roles\" -Force -Recurse
Copy-Item "$rootPath\EzyFix.Data\Data\Responses\Auth\*" "$rootPath\IdentityService.DAL\Data\Responses\Auth\" -Force -Recurse
Copy-Item "$rootPath\EzyFix.Data\Data\Responses\Roles\*" "$rootPath\IdentityService.DAL\Data\Responses\Roles\" -Force -Recurse
Copy-Item "$rootPath\EzyFix.Data\Data\MetaDatas\*" "$rootPath\IdentityService.DAL\Data\MetaDatas\" -Force -Recurse
Copy-Item "$rootPath\EzyFix.Data\Data\Exceptions\*" "$rootPath\IdentityService.DAL\Data\Exceptions\" -Force -Recurse

Write-Host "  ✓ Copied files for IdentityService" -ForegroundColor Gray

# ============================================
# PHẦN 3: COPY FILES CHO ACADEMICSERVICE
# ============================================
Write-Host "`n[3/3] Copy files cho AcademicService..." -ForegroundColor Yellow

# Controllers
$academicControllers = @("ExamsController.cs", "SemestersController.cs", "StudentsController.cs", "SubjectsController.cs", "FilesController.cs", "BaseController.cs")
foreach ($controller in $academicControllers) {
    Copy-Item "$rootPath\EzyFix.API\Controllers\$controller" "$rootPath\AcademicService.API\Controllers\" -Force
}

# Constants, Middlewares, Extensions
Copy-Item "$rootPath\EzyFix.API\Constants\*" "$rootPath\AcademicService.API\Constants\" -Force -Recurse
Copy-Item "$rootPath\EzyFix.API\Middlewares\*" "$rootPath\AcademicService.API\Middlewares\" -Force -Recurse
Copy-Item "$rootPath\EzyFix.API\Extensions\*" "$rootPath\AcademicService.API\Extensions\" -Force -Recurse

# Services
$academicServices = @("ExamService.cs", "SemesterService.cs", "StudentService.cs", "SubjectService.cs", "FileService.cs", "CloudinaryService.cs", "BaseService.cs")
foreach ($service in $academicServices) {
    if (Test-Path "$rootPath\EzyFix.Service\Services\Implements\$service") {
        Copy-Item "$rootPath\EzyFix.Service\Services\Implements\$service" "$rootPath\AcademicService.BLL\Services\Implements\" -Force
    }
}

# All service interfaces
Copy-Item "$rootPath\EzyFix.Service\Services\Interfaces\*" "$rootPath\AcademicService.BLL\Services\Interfaces\" -Force -Recurse

# Models
$academicModels = @("Exam.cs", "Semester.cs", "Student.cs", "Subject.cs", "Criteria.cs", "Grade.cs", "ScoreColumn.cs", "Submission.cs", "TeacherAssignment.cs", "Violation.cs")
foreach ($model in $academicModels) {
    if (Test-Path "$rootPath\EzyFix.Data\Models\$model") {
        Copy-Item "$rootPath\EzyFix.Data\Models\$model" "$rootPath\AcademicService.DAL\Models\" -Force
    }
}

# Repositories
Copy-Item "$rootPath\EzyFix.Data\Repositories\Interfaces\*" "$rootPath\AcademicService.DAL\Repositories\Interfaces\" -Force -Recurse
Copy-Item "$rootPath\EzyFix.Data\Repositories\Implements\*" "$rootPath\AcademicService.DAL\Repositories\Implements\" -Force -Recurse

# Data - Requests/Responses
Copy-Item "$rootPath\EzyFix.Data\Data\Requests\Exams\*" "$rootPath\AcademicService.DAL\Data\Requests\Exams\" -Force -Recurse
Copy-Item "$rootPath\EzyFix.Data\Data\Requests\Semesters\*" "$rootPath\AcademicService.DAL\Data\Requests\Semesters\" -Force -Recurse
Copy-Item "$rootPath\EzyFix.Data\Data\Requests\Students\*" "$rootPath\AcademicService.DAL\Data\Requests\Students\" -Force -Recurse
Copy-Item "$rootPath\EzyFix.Data\Data\Requests\Subjects\*" "$rootPath\AcademicService.DAL\Data\Requests\Subjects\" -Force -Recurse
Copy-Item "$rootPath\EzyFix.Data\Data\Requests\Files\*" "$rootPath\AcademicService.DAL\Data\Requests\Files\" -Force -Recurse

Copy-Item "$rootPath\EzyFix.Data\Data\Responses\Exams\*" "$rootPath\AcademicService.DAL\Data\Responses\Exams\" -Force -Recurse
Copy-Item "$rootPath\EzyFix.Data\Data\Responses\Semesters\*" "$rootPath\AcademicService.DAL\Data\Responses\Semesters\" -Force -Recurse
Copy-Item "$rootPath\EzyFix.Data\Data\Responses\Students\*" "$rootPath\AcademicService.DAL\Data\Responses\Students\" -Force -Recurse
Copy-Item "$rootPath\EzyFix.Data\Data\Responses\Subjects\*" "$rootPath\AcademicService.DAL\Data\Responses\Subjects\" -Force -Recurse
Copy-Item "$rootPath\EzyFix.Data\Data\Responses\Files\*" "$rootPath\AcademicService.DAL\Data\Responses\Files\" -Force -Recurse

Copy-Item "$rootPath\EzyFix.Data\Data\MetaDatas\*" "$rootPath\AcademicService.DAL\Data\MetaDatas\" -Force -Recurse
Copy-Item "$rootPath\EzyFix.Data\Data\Exceptions\*" "$rootPath\AcademicService.DAL\Data\Exceptions\" -Force -Recurse

# Mappers
Copy-Item "$rootPath\EzyFix.Data\Mappers\MappingProfile.cs" "$rootPath\AcademicService.DAL\Mappers\" -Force

Write-Host "  ✓ Copied files for AcademicService" -ForegroundColor Gray

# ============================================
# HOÀN THÀNH
# ============================================
Write-Host "`n=== HOÀN THÀNH CẤU TRÚC CƠ BẢN ===" -ForegroundColor Green
Write-Host "`nCác bước tiếp theo:" -ForegroundColor Cyan
Write-Host "1. Chạy script: .\Update-Namespaces.ps1 để update namespaces"
Write-Host "2. Tạo Program.cs cho mỗi service"
Write-Host "3. Tạo .csproj files cho các project còn lại"
Write-Host "4. Update EzyFix.sln để add 6 projects mới"
Write-Host "5. Build và test từng service"
Write-Host "`nXem file MICROSERVICES_STRUCTURE.md để biết chi tiết!" -ForegroundColor Yellow
