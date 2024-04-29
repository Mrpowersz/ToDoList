using ToDoList.Data;
using ToDoList.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Services
{
    public class HomeService : IHomeService
    {
        private readonly ApplicationDbContext _context;

        public HomeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Status> GetStatuses()
        {
            return _context.Statuses.ToList();
        }

        public ToDoListing? GetTask(int id)
        {
            return _context.ToDos.Include(t => t.Status).SingleOrDefault(t => t.Id == id);
        }

        public IQueryable<ToDoListing> FilterTasks(string due, string status)
        {
            var tasks = _context.ToDos.Include(t => t.Status).AsQueryable();

            if (due != "all")
            {
                var today = DateTime.Today;
                tasks = due switch
                {
                    "future" => tasks.Where(t => t.DueDate > today),
                    "past" => tasks.Where(t => t.DueDate < today),
                    "today" => tasks.Where(t => t.DueDate == today),
                    _ => tasks
                };
            }

            if (status != "all")
            {
                tasks = tasks.Where(t => t.Status.StatusId == status);
            }

            return tasks.OrderBy(t => t.DueDate);
        }

        public void Add(ToDoListing task)
        {
            _context.ToDos.Add(task);
            _context.SaveChanges();
        }

        public void Update(ToDoListing task)
        {
            _context.Update(task);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var task = _context.ToDos.Find(id);
            if (task != null)
            {
                _context.ToDos.Remove(task);
                _context.SaveChanges();
            }
        }

        public void MarkComplete(int id)
        {
            var task = _context.ToDos.Find(id);
            if (task != null && task.StatusId != "closed")
            {
                task.StatusId = "closed";
                _context.SaveChanges();
            }
        }

        public void ToggleStatus(int id)
        {
            var task = _context.ToDos.Include(t => t.Status).SingleOrDefault(t => t.Id == id);
            if (task == null)
            {
                throw new InvalidOperationException("Task not found");
            }

            task.StatusId = task.StatusId == "closed" ? "open" : "closed";

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("An error occurred while updating the status. Please check the status IDs and database constraints.", ex);
            }
        }
    }
}
