using Finals_Q1.Data;
using Finals_Q1.Models;
using Finals_Q1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Finals_Q1.Controllers;

[ApiController]
[Route("api/todo")]
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
        var todos = await _db.Todos.OrderBy(t => t.Id).ToListAsync();
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
        var todos = await _db.Todos.OrderBy(t => t.Id).ToListAsync();

        // Link to previous block
        item.PreviousHash = todos.Count > 0 ? todos.Last().Hash : "0000000000000000";

        // Temporarily assign an Id by saving first without hash
        item.Hash = string.Empty;
        _db.Todos.Add(item);
        await _db.SaveChangesAsync();

        // Now compute hash with the real Id
        item.Hash = BlockchainService.ComputeHash(item);
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

        // Recompute hash after update
        todo.Hash = BlockchainService.ComputeHash(todo);
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

    // GET: api/todo/verify
    [HttpGet("verify")]
    public async Task<IActionResult> Verify()
    {
        var todos = await _db.Todos.OrderBy(t => t.Id).ToListAsync();
        var isValid = BlockchainService.VerifyChain(todos);

        if (isValid)
            return Ok(new { status = "valid", message = "Chain integrity confirmed." });

        return Conflict(new { status = "tampered", message = "Chain integrity compromised." });
    }
}