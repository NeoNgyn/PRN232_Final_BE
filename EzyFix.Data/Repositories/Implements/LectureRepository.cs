using EzyFix.DAL.Models;
using EzyFix.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EzyFix.DAL.Repositories.Implements;

public class LectureRepository : ILectureRepository
{
    private AppDbContext _context;
    
    public LectureRepository(AppDbContext context)
    {
        _context = context;
    }
    
    
    public async Task<Lecturer> CreateUserAsync(Lecturer user)
    {
        await _context.Lecturers.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }
    
    
    public async Task<Lecturer?> GetUserByIdAsync(Guid id)
    {
        return await _context.Lecturers.FindAsync(id);
    }

    public async Task<Lecturer?> GetUserByEmailAsync(string email)
    {
        return await _context.Lecturers.FirstOrDefaultAsync(m => m.Email.ToLower() == email.ToLower());
    }
    
    public async Task<bool> UpdateUserAsync(Lecturer user)
    {
        _context.Lecturers.Update(user);
        await _context.SaveChangesAsync();
        return true;
    }
    
    
}