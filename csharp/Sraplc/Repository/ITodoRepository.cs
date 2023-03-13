using Sraplc.Models;

namespace Sraplc.Repository;

public interface ITodoRepository
{
    IEnumerable<Todo> GetAll();
    Todo GetBy(int id);
    int Create(Todo todo);
    int Update(Todo todo);
    int Delete(int id);
}