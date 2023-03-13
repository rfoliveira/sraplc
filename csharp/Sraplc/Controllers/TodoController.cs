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
    public IActionResult GetAll() => Ok(_service.GetAll());

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Todo), StatusCodes.Status200OK)]
    public IActionResult GetBy(int id) => Ok(_service.GetBy(id));

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    public IActionResult Create(Todo todo) => Created("", _service.Create(todo));

    [HttpPut]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public IActionResult Update(Todo todo) => Ok(_service.Update(todo));

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(int), StatusCodes.Status204NoContent)]
    public IActionResult Delete(int id) 
    {
        _service.Delete(id);        
        return NoContent();
    }
}
