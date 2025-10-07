// Path: EzyFix.DAL/Data/EzyFixDbContext.cs
using System;
using Microsoft.EntityFrameworkCore;

namespace EzyFix.DAL.Data;

public class EzyFixDbContext : DbContext
{
    public EzyFixDbContext(DbContextOptions<EzyFixDbContext> options) : base(options) {}

    // DbSets
   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("ezyfix");

        
        
    }
}
