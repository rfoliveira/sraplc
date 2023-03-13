using Sraplc.Models;

namespace Sraplc.Repository;

public interface ITodoRepository
{
    int Create(Todo todo);
    IEnumerable<Todo> ReadAll();
    Todo Read(int id);
    int Update(Todo todo);
    int Delete(int id);
}