using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.Metrics;
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

        public IActionResult Index(string id)
        {
            var filter = new Filters(id);
            ViewBag.Filter = filter;

            ViewBag.Statuses = _context.Statuses.ToList();
            ViewBag.DueFilters = Filters.DueFilterValues;
            ViewBag.SelectedDueFilter = filter.Due;

            IQueryable<ToDoListing> query = _context.ToDos
                .Include(t => t.Status);

            if(filter.hasStatus) 
            {
                query = query.Where(t => t.StatusId == filter.StatusId);
            }

            if (filter.HasDue) 
            {
                var today = DateTime.Today;
                if(filter.IsPast) 
                {
                    query = query.Where(t => t.DueDate < today);
                }
                else if (filter.IsFuture)
                {
                    query = query.Where(t => t.DueDate > today);
                }
                else if (filter.IsToday) 
                {
                    query = query.Where(t => t.DueDate == today);
                }
            }

            var tasks = query.OrderBy(t => t.DueDate).ToList();

            return View(tasks);
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
        public IActionResult Filter(string[] filter) 
        {
            string id = string.Join("-", filter);

            return RedirectToAction("Index", new { Id = id});
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
        public IActionResult DeleteComplete(string id) 
        {
            var toDelete = _context.ToDos.Where(t => t.StatusId == "closed").ToList();

            foreach(var task in toDelete) 
            {
                _context.ToDos.Remove(task);
            }
            _context.SaveChanges();

            return RedirectToAction("Index", new {Id = id});
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
