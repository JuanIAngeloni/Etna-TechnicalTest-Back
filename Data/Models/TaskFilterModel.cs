namespace Task_Manager.Models
{
    public class TaskFilterModel
    {
        public int? taskId { get; set; }
        public string? text { get; set; }
        public bool? isCompleted { get; set; }
    }
}
