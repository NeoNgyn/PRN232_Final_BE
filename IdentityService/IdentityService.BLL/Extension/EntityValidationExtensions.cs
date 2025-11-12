using IdentityService.DAL.Data.Exceptions;

namespace IdentityService.BLL.Extension;

public static class EntityValidationExtensions
{
    public static TEntity ValidateExists<TEntity>(
        this TEntity? entity,           
        Guid? id = null,                
        string? entityName = null,      
        string? customMessage = null)    
        where TEntity : class
    {
        if (entity == null)
        {
            var typeName = entityName ?? typeof(TEntity).Name; 
            var message = customMessage ?? $"{typeName}{(id.HasValue ? $" with ID {id}" : "")} are not found";
            throw new NotFoundException(message);
        }
        return entity;
    }
}