using ToDoList.Models;

namespace ToDoList.Services
{
    public interface IHomeService
    {
        List<Status> GetStatuses();
        ToDoListing GetTask(int id);
        IQueryable<ToDoListing> FilterTasks(string due, string status);
        void Add(ToDoListing task);
        void Update(ToDoListing task);
        void Delete(int id);
        void MarkComplete(int id);
        void ToggleStatus(int id);
    }
}
