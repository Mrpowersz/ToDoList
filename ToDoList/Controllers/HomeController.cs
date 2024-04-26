using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context) 
        {
            _context = context; 
        }

        public IActionResult Index(string due = "all", string status = "all")
        {
            ViewBag.DueFilters = new Dictionary<string, string>
            {
                {"all", "All"},
                {"future", "Future"},
                {"past", "Past"},
                {"today", "Today"}
            };
            ViewBag.Statuses = _context.Statuses.ToList();
            ViewBag.SelectedDueFilter = due;
            ViewBag.SelectedStatusId = status;

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

            tasks = tasks.OrderBy(t => t.DueDate);

            return View(tasks.ToList());
        }

        [HttpGet]
        public IActionResult Add() 
        {
            ViewBag.Statuses = _context.Statuses.ToList();
            var task = new ToDoListing { StatusId = "open" };

            return View(task);
        }

        [HttpPost]
        public IActionResult Add(ToDoListing task) 
        {
            if (ModelState.IsValid) 
            {
                _context.ToDos.Add(task);
                _context.SaveChanges();

                return RedirectToAction("Index");   
            }
            else 
            {
                ViewBag.Statuses = _context.Statuses.ToList();

                return View(task);
            }
        }

        [HttpPost]
        public IActionResult Filter(string due, string status)
        {
            return RedirectToAction("Index", new { due = due, status = status });
        }

        [HttpPost]
        public IActionResult MarkComplete([FromRoute] string id, ToDoListing selected) 
        {
            selected = _context.ToDos.Find(selected.Id)!;

            if(selected != null) 
            {
                selected.StatusId = "closed";
                _context.SaveChanges();
            }

            return RedirectToAction("Index", new { Id = id });
        }
		
		[HttpPost]
		public IActionResult Delete(int id) 
		{
			var task = _context.ToDos.Find(id);
			if (task != null)
			{
				_context.ToDos.Remove(task);
				_context.SaveChanges();
				return RedirectToAction("Index");
			}
			return NotFound(); 
		}

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var task = _context.ToDos.Find(id);
            if (task == null)
            {
                return NotFound();
            }
            ViewBag.Statuses = _context.Statuses.ToList();  
            return View(task);
        }

        [HttpPost]
        public IActionResult Edit(ToDoListing task)
        {
            if (ModelState.IsValid)
            {
                _context.Update(task);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Statuses = _context.Statuses.ToList(); 
            return View(task);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ToggleStatus(int id)
        {
            var task = _context.ToDos.Include(t => t.Status).FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            task.StatusId = task.StatusId == "closed" ? "open" : "closed";
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                
                return StatusCode(500, "An error occurred while updating the status. Please check the status IDs and database constraints.");
            }

            _context.Entry(task).Reference(t => t.Status).Load();

            return Json(new { success = true, newStatus = task.Status.Name, newStatusId = task.StatusId });
        }
    }
}
