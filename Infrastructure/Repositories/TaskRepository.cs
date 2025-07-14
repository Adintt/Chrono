using Chrono.Domain.Entities;
using Chrono.Domain.Interfaces;
using Chrono.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Chrono.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync(bool includeTimes)
        {
            return includeTimes
                ? await _context.Tasks.Include(t => t.Times).ToListAsync()
                : await _context.Tasks.ToListAsync();
        }

        public Task<TaskItem> GetByIdAsync(Guid id, bool includeTimes) =>
            includeTimes
                ? _context.Tasks.Include(t => t.Times).FirstOrDefaultAsync(t => t.Id == id)
                : _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

        public async Task AddAsync(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await _context.Tasks.Include(t => t.Times).FirstOrDefaultAsync(t => t.Id == id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddTimeEntryAsync(Guid taskId, TimeEntry entry)
        {
            entry.TaskId = taskId;
            _context.Times.Add(entry);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TimeEntry>> GetTimesByTaskIdAsync(Guid taskId)
        {
            return await _context.Times
                .Where(te => te.TaskId == taskId)
                .ToListAsync();
        }

        public async Task UpdateTimeEntryAsync(TimeEntry entry)
        {
            _context.Times.Update(entry);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTimeEntryAsync(Guid taskId, Guid timeId)
        {
            var entry = await _context.Times.FirstOrDefaultAsync(t => t.Id == timeId && t.TaskId == taskId);
            if (entry != null)
            {
                _context.Times.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }
    }
}
