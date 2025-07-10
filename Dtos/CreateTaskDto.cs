namespace Gestion_de_Tâches.Dtos
{
    public class CreateTaskDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int? AssignedToUserId { get; set; }
        public int CreatedByUserId { get; set; }
    }
}
