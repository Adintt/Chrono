using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Chrono.Domain.Entities
{
    public class TimeEntry
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string Description { get; set; } = "";
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }

        [JsonIgnore]
        public TaskItem? Task { get; set; }
    }
}