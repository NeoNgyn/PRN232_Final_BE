-- Script để tạo Role Teacher và một số giảng viên mẫu

-- 1. Tạo Role Teacher (nếu chưa có)
INSERT INTO "Roles" ("RoleId", "RoleName", "Description")
VALUES 
    (gen_random_uuid(), 'Teacher', 'Teacher role for grading student submissions')
ON CONFLICT DO NOTHING;

-- 2. Tạo một số giảng viên mẫu
DO $$
DECLARE
    teacher_role_id UUID;
BEGIN
    -- Lấy RoleId của Teacher
    SELECT "RoleId" INTO teacher_role_id 
    FROM "Roles" 
    WHERE "RoleName" = 'Teacher' 
    LIMIT 1;
    
    -- Tạo giảng viên 1
    INSERT INTO "Users" (
        "UserId", 
        "Name", 
        "Email", 
        "Password", 
        "LecturerCode", 
        "EmailConfirmed", 
        "IsActive", 
        "RoleId",
        "CreatedAt",
        "UpdatedAt"
    )
    VALUES (
        gen_random_uuid(),
        'Nguyễn Văn A',
        'nguyenvana@fpt.edu.vn',
        '$2a$11$hashed_password',  -- Password hash
        'LEC001',
        true,
        true,
        teacher_role_id,
        NOW(),
        NOW()
    )
    ON CONFLICT ("Email") DO NOTHING;
    
    -- Tạo giảng viên 2
    INSERT INTO "Users" (
        "UserId", 
        "Name", 
        "Email", 
        "Password", 
        "LecturerCode", 
        "EmailConfirmed", 
        "IsActive", 
        "RoleId",
        "CreatedAt",
        "UpdatedAt"
    )
    VALUES (
        gen_random_uuid(),
        'Trần Thị B',
        'tranthib@fpt.edu.vn',
        '$2a$11$hashed_password',
        'LEC002',
        true,
        true,
        teacher_role_id,
        NOW(),
        NOW()
    )
    ON CONFLICT ("Email") DO NOTHING;
    
    -- Tạo giảng viên 3
    INSERT INTO "Users" (
        "UserId", 
        "Name", 
        "Email", 
        "Password", 
        "LecturerCode", 
        "EmailConfirmed", 
        "IsActive", 
        "RoleId",
        "CreatedAt",
        "UpdatedAt"
    )
    VALUES (
        gen_random_uuid(),
        'Lê Văn C',
        'levanc@fpt.edu.vn',
        '$2a$11$hashed_password',
        'LEC003',
        true,
        true,
        teacher_role_id,
        NOW(),
        NOW()
    )
    ON CONFLICT ("Email") DO NOTHING;
    
    -- Tạo giảng viên 4
    INSERT INTO "Users" (
        "UserId", 
        "Name", 
        "Email", 
        "Password", 
        "LecturerCode", 
        "EmailConfirmed", 
        "IsActive", 
        "RoleId",
        "CreatedAt",
        "UpdatedAt"
    )
    VALUES (
        gen_random_uuid(),
        'Phạm Thị D',
        'phamthid@fpt.edu.vn',
        '$2a$11$hashed_password',
        'LEC004',
        true,
        true,
        teacher_role_id,
        NOW(),
        NOW()
    )
    ON CONFLICT ("Email") DO NOTHING;
END $$;

-- 3. Kiểm tra kết quả
SELECT 
    u."UserId",
    u."Name",
    u."Email",
    u."LecturerCode",
    r."RoleName",
    u."IsActive"
FROM "Users" u
LEFT JOIN "Roles" r ON u."RoleId" = r."RoleId"
WHERE r."RoleName" = 'Teacher'
ORDER BY u."LecturerCode";
