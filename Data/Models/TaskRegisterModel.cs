namespace Task_Manager.Models
{
    public class TaskRegisterModel
    {

        public int ?taskId { get; set; }
        public string title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int priority { get; set; }
        public bool ?isCompleted { get; set; } = false;
        public bool ?isDeleted { get; set; } = false;
        public DateTime ?createDate { get; set; } = DateTime.MinValue;
        public DateTime ?updateDate { get; set; } = DateTime.MinValue;
        public int userId { get; set; }
        public int categoryId { get; set; }

    }
}
