using EzyFix.DAL.Models;
using EzyFix.DAL.Repositories.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Repositories.Interfaces
{
    public interface ISemesterRepository : IGenericRepository<Semester>
    {
        // Thêm các phương thức truy vấn riêng cho Semester nếu cần
        // Ví dụ: Task<Semester> GetSemesterByNameAsync(string name);
    }    
}
