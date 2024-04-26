using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;


namespace ToDoList.Models
{
    public class ToDoListing 
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a description.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter due date.")]
        [FutureDate(ErrorMessage = "Due date must be in the future.")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Please select status.")]
        public string StatusId { get; set; } = string.Empty;
		[ValidateNever]
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
}
