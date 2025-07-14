using Chrono.Domain.Entities;

namespace Chrono.Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync(bool includeTimes);
        Task<TaskItem> GetByIdAsync(Guid id, bool includeTimes);
        Task AddAsync(TaskItem task);
        Task DeleteAsync(Guid id);
        Task AddTimeEntryAsync(Guid taskId, TimeEntry entry);
        Task<IEnumerable<TimeEntry>> GetTimesByTaskIdAsync(Guid taskId);
        Task UpdateTimeEntryAsync(TimeEntry entry);
        Task DeleteTimeEntryAsync(Guid taskId, Guid timeId);
    }

}
