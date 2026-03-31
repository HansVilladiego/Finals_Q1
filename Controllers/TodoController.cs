using Finals_Q1.Data;
using Finals_Q1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Finals_Q1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly AppDbContext _db;

    public TodoController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/todo
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var todos = await _db.Todos.ToListAsync();
        return Ok(todos);
    }

    // GET: api/todo/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var todo = await _db.Todos.FindAsync(id);
        if (todo == null) return NotFound();
        return Ok(todo);
    }

    // POST: api/todo
    [HttpPost]
    public async Task<IActionResult> Create(TodoItem item)
    {
        _db.Todos.Add(item);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    // PUT: api/todo/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TodoItem updated)
    {
        var todo = await _db.Todos.FindAsync(id);
        if (todo == null) return NotFound();

        todo.Title = updated.Title;
        todo.IsCompleted = updated.IsCompleted;
        await _db.SaveChangesAsync();
        return Ok(todo);
    }

    // DELETE: api/todo/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var todo = await _db.Todos.FindAsync(id);
        if (todo == null) return NotFound();

        _db.Todos.Remove(todo);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}