namespace Gestion_de_Tâches.Dtos
{
    public class AssignUserDto
    {
        public int? AssignedToUserId { get; set; }  
        public int ChangedByUserId { get; set; }
    }
}
