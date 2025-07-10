using System;

namespace Gestion_de_Tâches.Models
{
    public enum ChangeType
    {
        StatusChange,
        AssignmentChange,
        Creation
    }

    public class TaskHistory
    {
        public int ID { get; set; }
        public int TaskId { get; set; }
        public int ChangedByUserId { get; set; }
        public ChangeType ChangeType { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime ChangeDate { get; set; } = DateTime.UtcNow;

        // Navigation property (optionnelle)
        public Task Task { get; set; }
    }
}
