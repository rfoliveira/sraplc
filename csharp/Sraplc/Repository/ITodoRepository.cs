using Sraplc.Models;

namespace Sraplc.Repository;

public interface ITodoRepository
{
    Task<IEnumerable<Todo>> GetAllAsync();
    Task<Todo> GetByAsync(int id);
    Task<int> CreateAsync(Todo todo);
    Task<int> UpdateAsync(Todo todo);
    Task<int> DeleteAsync(int id);
}