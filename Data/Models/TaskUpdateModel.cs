using Etna_Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etna_Data.Models
{
    public class TaskUpdateModel
    {

        public int taskId { get; set; }
        public string title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int priority { get; set; }
        public bool isCompleted { get; set; } = false;
        public bool isDeleted { get; set; } = false;
        public DateTime createDate { get; set; } = DateTime.MinValue;
        public DateTime updateDate { get; set; } = DateTime.MinValue;
        public int userId { get; set; }
        public int categoryId { get; set; }
        public CategoryModel category { get; set; }

    }
}
