using Gestion_de_Tâches.Data;
using Gestion_de_Tâches.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gestion_de_Tâches.Dtos;


[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    // GET /api/tasks
    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        var tasks = await _context.Tasks
            .Include(t => t.AssignedToUser)
            .ToListAsync();

        return Ok(tasks);
    }

    // GET /api/tasks/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        var task = await _context.Tasks
            .Include(t => t.AssignedToUser)
            .Include(t => t.History)
            .FirstOrDefaultAsync(t => t.ID == id);

        if (task == null) return NotFound();

        return Ok(task);
    }

    // POST /api/tasks
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
    {
        var task = new Gestion_de_Tâches.Models.Task
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = Gestion_de_Tâches.Models.TaskStatus.ToDo,
            AssignedToUserId = dto.AssignedToUserId,
            CreatedByUserId = dto.CreatedByUserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        _context.TaskHistories.Add(new TaskHistory
        {
            TaskId = task.ID,
            ChangedByUserId = dto.CreatedByUserId,
            ChangeType = ChangeType.Creation,
            NewValue = "Tâche créée",
            ChangeDate = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTask), new { id = task.ID }, task);
    }

    // PUT /api/tasks/{id}/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusDto dto)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();

        if (!IsValidStatusTransition(task.Status, dto.NewStatus))
            return BadRequest("Transition de statut invalide.");

        var oldStatus = task.Status;
        task.Status = dto.NewStatus;
        task.UpdatedAt = DateTime.UtcNow;

        _context.TaskHistories.Add(new TaskHistory
        {
            TaskId = id,
            ChangedByUserId = dto.ChangedByUserId,
            ChangeType = ChangeType.StatusChange,
            OldValue = oldStatus.ToString(),
            NewValue = dto.NewStatus.ToString(),
            ChangeDate = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return NoContent();
    }


    // PUT /api/tasks/{id}/assign
    [HttpPut("{id}/assign")]
    public async Task<IActionResult> AssignTask(int id, [FromBody] AssignUserDto dto)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();

        var oldAssigned = task.AssignedToUserId;
        task.AssignedToUserId = dto.AssignedToUserId;
        task.UpdatedAt = DateTime.UtcNow;

        _context.TaskHistories.Add(new TaskHistory
        {
            TaskId = id,
            ChangedByUserId = dto.ChangedByUserId,
            ChangeType = ChangeType.AssignmentChange,
            OldValue = oldAssigned?.ToString(),
            NewValue = dto.AssignedToUserId?.ToString(),
            ChangeDate = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool IsValidStatusTransition(Gestion_de_Tâches.Models.TaskStatus oldStatus, Gestion_de_Tâches.Models.TaskStatus newStatus)
    {
        return (oldStatus == Gestion_de_Tâches.Models.TaskStatus.ToDo && newStatus == Gestion_de_Tâches.Models.TaskStatus.InProgress) ||
               (oldStatus == Gestion_de_Tâches.Models.TaskStatus.InProgress && newStatus == Gestion_de_Tâches.Models.TaskStatus.Done);
    }

}
