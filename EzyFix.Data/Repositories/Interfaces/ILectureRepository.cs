using EzyFix.DAL.Models;

namespace EzyFix.DAL.Repositories.Interfaces;

public interface ILectureRepository
{
    Task<Lecturer?> GetUserByEmailAsync(string email);
    Task<Lecturer> CreateUserAsync(Lecturer user);
    Task<bool> UpdateUserAsync(Lecturer user);
}