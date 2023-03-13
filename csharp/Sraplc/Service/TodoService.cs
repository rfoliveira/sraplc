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

    public int Create(Todo todo) => _repo.Create(todo);

    public int Delete(int id) => _repo.Delete(id);

    public IEnumerable<Todo> GetAll() => _repo.GetAll();

    public Todo GetBy(int id) => _repo.GetBy(id);

    public int Update(Todo todo) => _repo.Update(todo);
}