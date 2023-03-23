using System.Net;
using Microsoft.AspNetCore.Mvc;
using Sraplc.Models;
using Sraplc.Service;

namespace Sraplc.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{
    private readonly ITodoService _service;

    public TodoController(ITodoService service)
    {
        _service = service;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Todo>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync() => await Task.FromResult(Ok(await _service.GetAllAsync()));

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Todo), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByAsync(int id) => await Task.FromResult(Ok(await _service.GetByAsync(id)));

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync(Todo todo) => await Task.FromResult(Created("", await _service.CreateAsync(todo)));

    [HttpPut]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync(Todo todo) => await Task.FromResult(Ok(await _service.UpdateAsync(todo)));

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(int), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAsync(int id) 
    {
        await _service.DeleteAsync(id);        
        return await Task.FromResult(NoContent());
    }
}
