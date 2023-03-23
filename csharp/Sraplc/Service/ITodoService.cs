using Sraplc.Models;

namespace Sraplc.Service;

public interface ITodoService
{
    Task<IEnumerable<Todo>> GetAllAsync();
    Task<Todo> GetByAsync(int id);
    Task<int> CreateAsync(Todo todo);
    Task<int> UpdateAsync(Todo todo);
    Task<int> DeleteAsync(int id);
}