namespace Gestion_de_Tâches.Dtos
{
    public class UpdateStatusDto
    {
        public Gestion_de_Tâches.Models.TaskStatus NewStatus { get; set; }
        public int ChangedByUserId { get; set; }
    }
}
