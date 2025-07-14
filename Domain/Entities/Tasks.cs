using System.ComponentModel.DataAnnotations;

namespace Chrono.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; }
        public string Customer { get; set; } = "";
        public ICollection<TimeEntry> Times { get; set; } = new List<TimeEntry>();
    }
}
