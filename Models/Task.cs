using System;
using System.Collections.Generic;

namespace Gestion_de_Tâches.Models
{
    public enum TaskStatus
    {
        ToDo,
        InProgress,
        Done
    }

    public class Task
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.ToDo;
        public int? AssignedToUserId { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public User? AssignedToUser { get; set; }
        public List<TaskHistory> History { get; set; } = new List<TaskHistory>();
    }
}
