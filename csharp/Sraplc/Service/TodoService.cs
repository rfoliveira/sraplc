using Sraplc.Models;
using Sraplc.Repository;

namespace Sraplc.Service;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _repo;

    public TodoService(ITodoRepository repo)
    {
        _repo = repo;
    }

    public async Task<int> CreateAsync(Todo todo) => await _repo.CreateAsync(todo);

    public async Task<int> DeleteAsync(int id) => await _repo.DeleteAsync(id);

    public async Task<IEnumerable<Todo>> GetAllAsync() => await _repo.GetAllAsync();

    public async Task<Todo> GetByAsync(int id) => await _repo.GetByAsync(id);

    public async Task<int> UpdateAsync(Todo todo) => await _repo.UpdateAsync(todo);
}