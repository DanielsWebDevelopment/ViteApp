using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListBackEnd.Context;
using ToDoListBackEnd.Models;

namespace ToDoListBackEnd.Controllers;

[Route("api/ToDoItems")]
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private readonly DatabaseContext _context;

    public ToDoItemsController(DatabaseContext context)
    {
        _context = context;
    }

    // GET: api/ToDoItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDoItemDTO>>> GetToDoItems()
    {
        return await _context.ToDoItems.Select(x => ItemToDTO(x)).ToListAsync();
    }

    // GET: api/ToDoItems/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ToDoItemDTO>> GetToDoItem(long id)
    {
        var toDoItem = await _context.ToDoItems.FindAsync(id);

        if (toDoItem == null)
        {
            return NotFound();
        }

        return ItemToDTO(toDoItem);
    }

    // PUT: api/ToDoItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutToDoItem(long id, ToDoItemDTO toDoItemDTO)
    {
        if (id != toDoItemDTO.Id)
        {
            return BadRequest();
        }

        var toDoItem = await _context.ToDoItems.FindAsync(id);

        if (toDoItem == null)
        {
            return NotFound();
        }

        toDoItem.Name = toDoItemDTO.Name;
        toDoItem.IsComplete = toDoItemDTO.IsComplete;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!ToDoItemExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/ToDoItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ToDoItemDTO>> PostToDoItem(ToDoItemDTO toDoItemDTO)
    {
        var toDoItem = new ToDoItem
        {
            Name = toDoItemDTO.Name,
            IsComplete = toDoItemDTO.IsComplete
        };

        _context.ToDoItems.Add(toDoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetToDoItem), new { id = toDoItem.Id }, ItemToDTO(toDoItem));

    }

    // DELETE: api/ToDoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteToDoItem(long id)
    {
        var toDoItem = await _context.ToDoItems.FindAsync(id);
        if (toDoItem == null)
        {
            return NotFound();
        }

        _context.ToDoItems.Remove(toDoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ToDoItemExists(long id)
    {
        return _context.ToDoItems.Any(e => e.Id == id);
    }

    private static ToDoItemDTO ItemToDTO(ToDoItem todoItem) =>
        new()
        {
            Id = todoItem.Id,
            Name = todoItem.Name,
            IsComplete = todoItem.IsComplete
        };
    }

