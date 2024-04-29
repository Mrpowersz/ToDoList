using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
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
            ViewBag.Statuses = _homeService.GetStatuses();
            ViewBag.SelectedDueFilter = due;
            ViewBag.SelectedStatusId = status;

            var tasks = _homeService.FilterTasks(due, status).ToList();
            return View(tasks);
        }

        public IActionResult Add()
        {
            ViewBag.Statuses = _homeService.GetStatuses();
            var task = new ToDoListing();
            return View(task);
        }

        [HttpPost]
        public IActionResult Add(ToDoListing task)
        {
            if (ModelState.IsValid)
            {
                _homeService.Add(task);
                return RedirectToAction("Index");
            }
            ViewBag.Statuses = _homeService.GetStatuses();
            return View(task);
        }

        public IActionResult Edit(int id)
        {
            var task = _homeService.GetTask(id);
            if (task == null)
            {
                return NotFound();
            }
            ViewBag.Statuses = _homeService.GetStatuses();
            return View(task);
        }

        [HttpPost]
        public IActionResult Edit(ToDoListing task)
        {
            if (ModelState.IsValid)
            {
                _homeService.Update(task);
                return RedirectToAction("Index");
            }
            ViewBag.Statuses = _homeService.GetStatuses();
            return View(task);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _homeService.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult MarkComplete(int id)
        {
            _homeService.MarkComplete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Filter(string due, string status)
        {
            return RedirectToAction("Index", new { due = due, status = status });
        }

        [HttpPost]
        public IActionResult ToggleStatus(int id)
        {
            try
            {
                _homeService.ToggleStatus(id);
                var task = _homeService.GetTask(id); 
                return Json(new { success = true, newStatus = task.Status.Name, newStatusId = task.StatusId });
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("Task not found"))
                {
                    return NotFound();
                }
                return StatusCode(500, ex.Message);
            }
        }
    }
}
