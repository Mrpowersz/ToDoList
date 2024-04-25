using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Models
{
    public class ToDoListing  // Rename from ToDoList to ToDoItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a description.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter due date.")]
        [FutureDate(ErrorMessage = "Due date must be in the future.")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Please select status.")]
        public string StatusId { get; set; } = string.Empty;
        public Status Status { get; set; } = null!;
        public bool Overdue => StatusId == "open" && DueDate < DateTime.Today;
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dt;
            return value != null && DateTime.TryParse(value.ToString(), out dt) && dt >= DateTime.Today;
        }
    }

    //dizains krutaks, 
}
