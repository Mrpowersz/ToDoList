namespace ToDoList.Models
{
    public class Filters
    {
        public Filters(string filterString)
        {
            FilterString = filterString ?? "all-all";
            string[] filter = FilterString.Split("-");

            if (filter.Length >= 2)
            {
                Due = filter[0];
                StatusId = filter[1];
            }
            else
            {
                Due = "all";
                StatusId = "all";
            }
        }
        public string FilterString { get; }
        public string Due { get; }
        public string StatusId { get; }
        public bool HasDue => Due.ToLower() != "all";
        public bool hasStatus => StatusId.ToLower() != "all";
        public static Dictionary<string, string> DueFilterValues =>
            new Dictionary<string, string> {
             { "future", "Future" },
             {"past", "Past" },
             {"today", "Today" }
         };

        public bool IsPast => Due.ToLower() == "past"; 
        public bool IsFuture => Due.ToLower() == "future";
        public bool IsToday => Due.ToLower() == "today";
    }
}


