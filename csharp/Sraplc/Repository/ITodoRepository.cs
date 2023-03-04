using Sraplc.Models;

namespace Sraplc.Repository;

public interface ITodoRepository
{
    Task<int> CreateAsync(Todo todo);
    Task<Todo> ReadAsync(int id);
    Task<int> UpdateAsync(Todo todo);
    Task<int> DeleteAsync(int id);
}