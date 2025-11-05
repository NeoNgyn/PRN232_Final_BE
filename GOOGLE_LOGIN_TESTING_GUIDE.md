# Google Login Testing Guide

## H??ng d?n test Google Login v?i OAuth 2.0 Playground

### B??c 1: C?u hình Google Cloud Console

1. Truy c?p [Google Cloud Console](https://console.cloud.google.com/)
2. Ch?n ho?c t?o project
3. Vào **APIs & Services** > **Credentials**
4. Tìm OAuth 2.0 Client ID v?i Client ID: `16223097955-590qu6bf56773fj6one2s0pobbactglg.apps.googleusercontent.com`
5. Thêm `https://developers.google.com/oauthplayground` vào **Authorized redirect URIs**

### B??c 2: L?y ID Token t? OAuth 2.0 Playground

1. Truy c?p [https://developers.google.com/oauthplayground/](https://developers.google.com/oauthplayground/)

2. Click vào **Settings icon** (??) ? góc trên bên ph?i

3. Trong **OAuth 2.0 Configuration**:
   - ? Check **Use your own OAuth credentials**
   - **OAuth Client ID**: `16223097955-590qu6bf56773fj6one2s0pobbactglg.apps.googleusercontent.com`
   - **OAuth Client secret**: (L?y t? Google Cloud Console)

4. Ch?n scopes c?n thi?t:
   ```
   https://www.googleapis.com/auth/userinfo.email
   https://www.googleapis.com/auth/userinfo.profile
   ```

5. Click **Authorize APIs**

6. ??ng nh?p v?i tài kho?n Google c?a b?n

7. Sau khi authorize, click **Exchange authorization code for tokens**

8. Copy **id_token** t? response (không ph?i access_token)

### B??c 3: Test API v?i Swagger/Postman

#### S? d?ng Swagger UI (https://localhost:8686/swagger)

1. M? Swagger UI t?i `https://localhost:8686/swagger`

2. Tìm endpoint **POST /api/v1/auth/google-login**

3. Click **Try it out**

4. Paste ID Token vào request body:
   ```json
   {
     "idToken": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjE4MmEw..."
   }
   ```

5. Click **Execute**

6. Ki?m tra response:
   ```json
   {
     "statusCode": 200,
     "message": "Google login successful",
     "isSuccess": true,
     "data": {
       "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
       "email": "user@gmail.com",
       "name": "User Name",
       "role": "Student",
       "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
       "refreshToken": "base64encodedtoken...",
       "isNewUser": true
     }
   }
   ```

#### S? d?ng Postman

1. **Method**: POST
2. **URL**: `https://localhost:8686/api/v1/auth/google-login`
3. **Headers**: 
   ```
   Content-Type: application/json
   ```
4. **Body** (raw JSON):
   ```json
   {
     "idToken": "YOUR_GOOGLE_ID_TOKEN_HERE"
   }
   ```

5. Send request và ki?m tra response

### B??c 4: S? d?ng Access Token

Sau khi login thành công, b?n s? nh?n ???c `accessToken`. S? d?ng token này ?? g?i các API c?n authentication:

**Headers**:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### L?u ý quan tr?ng ??

1. **ID Token có th?i h?n ng?n** (kho?ng 1 gi?), b?n c?n l?y token m?i n?u h?t h?n

2. **Không s? d?ng Access Token t? Google Playground**, ph?i dùng **ID Token**

3. Token structure:
   - Google ID Token: `eyJhbGciOiJSUzI1NiIsImtpZCI...` (r?t dài, kho?ng 800-1000 characters)
   - Google Access Token: `ya29.a0AfH6SMB...` (ng?n h?n)

4. **Client ID ph?i kh?p** gi?a:
   - appsettings.json
   - Google Cloud Console
   - OAuth Playground settings

### Troubleshooting

#### L?i: "Invalid Google token"
- ? Ki?m tra ?ã copy ?úng **id_token** (không ph?i access_token)
- ? Token ch?a expired
- ? Client ID trong appsettings.json kh?p v?i Google Cloud Console

#### L?i: "Token không h?p l?"
- ? ??m b?o s? d?ng HTTPS trong OAuth Playground redirect URI
- ? Ki?m tra scope có bao g?m email và profile

#### L?i: "User already exists"
- Không ph?i l?i! ?ây là flow bình th??ng khi user ?ã ??ng ký tr??c ?ó
- `isNewUser` s? là `false`

### Test Flow hoàn ch?nh

```
1. L?y ID Token t? Google OAuth Playground
   ?
2. Call POST /api/v1/auth/google-login v?i ID Token
   ?
3. Nh?n ???c Access Token (JWT) t? backend
   ?
4. S? d?ng Access Token ?? g?i protected APIs
   ?
5. N?u token h?t h?n, dùng Refresh Token ?? l?y token m?i
```

### Các endpoint liên quan

- **Google Login**: `POST /api/v1/auth/google-login`
- **Refresh Token**: `POST /api/v1/auth/refresh-token`
- **Delete Refresh Token**: `DELETE /api/v1/auth/delete-refresh-token?refreshToken=...`

### Ví d? cURL

```bash
curl -X POST "https://localhost:8686/api/v1/auth/google-login" \
  -H "Content-Type: application/json" \
  -d '{
    "idToken": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjE4MmEw..."
  }'
```

### Database Changes

Khi user login l?n ??u qua Google, h? th?ng s? t? ??ng:
1. T?o user m?i trong b?ng `users`
2. Set role m?c ??nh là "Student"
3. ?ánh d?u `EmailConfirmed = true` (vì Google ?ã verify)
4. Không set password (user ch? login qua Google)

---

## Security Notes

- ? ID Token ???c validate v?i Google servers
- ? Email ???c verify b?i Google
- ? JWT Token có th?i h?n (30 phút theo config)
- ? Refresh Token có th?i h?n (7 ngày theo config)
- ? Password không ???c l?u cho Google users

**Chúc b?n test thành công! ??**
