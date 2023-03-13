using Sraplc.Models;

namespace Sraplc.Service;

public interface ITodoService
{
    IEnumerable<Todo> GetAll();
    Todo GetBy(int id);
    int Create(Todo todo);
    int Update(Todo todo);
    int Delete(int id);
}