using Chrono.Domain.Entities;
using Chrono.Domain.Interfaces;

namespace Chrono.Application
{
    public class TaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<object>> GetAllTasksAsync(bool expanded)
        {
            var tasks = await _repository.GetAllAsync(expanded);
            return tasks.Select(t => new
            {
                t.Id,
                t.Name,
                t.Description,
                t.Customer,
                Times = expanded ? t.Times.Select(time => new
                {
                    time.Id,
                    time.Description,
                    time.BeginDate,
                    time.EndDate,
                    SpentTime = ((time.EndDate ?? DateTime.UtcNow) - time.BeginDate).TotalHours
                }) : null
            });
        }

        public Task<TaskItem> GetByIdAsync(Guid id, bool expanded) => _repository.GetByIdAsync(id, expanded);

        public Task CreateAsync(TaskItem task) => _repository.AddAsync(task);

        public Task DeleteAsync(Guid id) => _repository.DeleteAsync(id);

        public Task<IEnumerable<TimeEntry>> GetTimesByTaskIdAsync(Guid id) => _repository.GetTimesByTaskIdAsync(id);

        public Task AddTimeEntryAsync(Guid id, TimeEntry entry) => _repository.AddTimeEntryAsync(id, entry);

        public Task UpdateTimeEntryAsync(TimeEntry entry) => _repository.UpdateTimeEntryAsync(entry);

        public Task DeleteTimeEntryAsync(Guid taskId, Guid timeId) => _repository.DeleteTimeEntryAsync(taskId, timeId);
    }

}
