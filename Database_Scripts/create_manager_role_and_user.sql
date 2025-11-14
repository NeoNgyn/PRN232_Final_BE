-- Script để tạo Role Manager và User Manager trong database

-- 1. Tạo Role Manager (nếu chưa có)
INSERT INTO "Roles" ("RoleId", "RoleName", "Description")
VALUES 
    (gen_random_uuid(), 'Manager', 'Manager role for assigning grading tasks to teachers')
ON CONFLICT DO NOTHING;

-- 2. Tạo User Manager (thay đổi thông tin nếu cần)
-- Lưu ý: Thay YOUR_EMAIL@example.com bằng email thực
DO $$
DECLARE
    manager_role_id UUID;
BEGIN
    -- Lấy RoleId của Manager
    SELECT "RoleId" INTO manager_role_id 
    FROM "Roles" 
    WHERE "RoleName" = 'Manager' 
    LIMIT 1;
    
    -- Tạo User với role Manager
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
        'Manager User',
        'manager@fpt.edu.vn',  -- Thay bằng email Google của bạn nếu dùng Google Login
        'hashed_password_here',  -- Password hash (nếu dùng login thường)
        'MNG001',
        true,
        true,
        manager_role_id,
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
    r."RoleName"
FROM "Users" u
LEFT JOIN "Roles" r ON u."RoleId" = r."RoleId"
WHERE r."RoleName" = 'Manager';
