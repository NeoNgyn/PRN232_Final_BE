# Script tự động update namespaces từ EzyFix sang IdentityService/AcademicService
# Chạy script này SAU KHI đã chạy Create-Microservices-Structure.ps1

$rootPath = "d:\K8\PRN232\PRN232_Final_BE"

Write-Host "=== BẮT ĐẦU UPDATE NAMESPACES ===" -ForegroundColor Green

function Update-NamespacesInFile {
    param(
        [string]$FilePath,
        [string]$ServiceName  # "Identity" hoặc "Academic"
    )
    
    if (-not (Test-Path $FilePath)) {
        return
    }
    
    $content = Get-Content $FilePath -Raw
    
    # Replace namespaces
    $content = $content -replace 'namespace EzyFix\.API', "namespace ${ServiceName}Service.API"
    $content = $content -replace 'namespace EzyFix\.BLL', "namespace ${ServiceName}Service.BLL"
    $content = $content -replace 'namespace EzyFix\.DAL', "namespace ${ServiceName}Service.DAL"
    
    # Replace using statements
    $content = $content -replace 'using EzyFix\.API', "using ${ServiceName}Service.API"
    $content = $content -replace 'using EzyFix\.BLL', "using ${ServiceName}Service.BLL"
    $content = $content -replace 'using EzyFix\.DAL', "using ${ServiceName}Service.DAL"
    
    # Replace DbContext
    if ($ServiceName -eq "Identity") {
        $content = $content -replace 'AppDbContext', 'IdentityDbContext'
        $content = $content -replace 'IUnitOfWork<AppDbContext>', 'IUnitOfWork<IdentityDbContext>'
    } elseif ($ServiceName -eq "Academic") {
        $content = $content -replace 'AppDbContext', 'AcademicDbContext'
        $content = $content -replace 'IUnitOfWork<AppDbContext>', 'IUnitOfWork<AcademicDbContext>'
    }
    
    Set-Content -Path $FilePath -Value $content -NoNewline
}

# ============================================
# UPDATE IDENTITY SERVICE
# ============================================
Write-Host "`n[1/2] Updating IdentityService namespaces..." -ForegroundColor Yellow

$identityFiles = Get-ChildItem -Path "$rootPath\IdentityService.*" -Recurse -Include *.cs
foreach ($file in $identityFiles) {
    Update-NamespacesInFile -FilePath $file.FullName -ServiceName "Identity"
    Write-Host "  ✓ Updated: $($file.Name)" -ForegroundColor Gray
}

# ============================================
# UPDATE ACADEMIC SERVICE
# ============================================
Write-Host "`n[2/2] Updating AcademicService namespaces..." -ForegroundColor Yellow

$academicFiles = Get-ChildItem -Path "$rootPath\AcademicService.*" -Recurse -Include *.cs
foreach ($file in $academicFiles) {
    Update-NamespacesInFile -FilePath $file.FullName -ServiceName "Academic"
    Write-Host "  ✓ Updated: $($file.Name)" -ForegroundColor Gray
}

Write-Host "`n=== HOÀN THÀNH UPDATE NAMESPACES ===" -ForegroundColor Green
Write-Host "`nLưu ý:" -ForegroundColor Cyan
Write-Host "- Một số references có thể cần update thủ công"
Write-Host "- Kiểm tra lại các DbContext references"
Write-Host "- Build từng project để phát hiện lỗi còn sót"
