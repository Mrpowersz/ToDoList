using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class Status
    {
        [Key]
        public string StatusId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}

