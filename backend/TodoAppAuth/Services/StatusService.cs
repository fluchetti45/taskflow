using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAppAuth.Context;
using TodoAppAuth.Interfaces;
using TodoAppAuth.Models;

namespace TodoAppAuth.Services
{
    public class StatusService : IStatus
    {
        private readonly TodoContext _todoContext;

        public StatusService(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }
        public Task<ActionResult> DeleteStatus(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> EditStatus(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<AppStatus> CreateStatus(string statusName)
        {
            AppStatus status = new AppStatus
            {
                StatusName = statusName,
                CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                IsActive = true

            };
            _todoContext.Statuses.Add(status);
            await _todoContext.SaveChangesAsync();
            return status;
        }

        public Task<List<AppStatus>> GetAllStatus()
        {
            var resultado = _todoContext.Statuses.ToListAsync();
            return resultado;
        }

        public async Task<AppStatus> GetStatus(int id)
        {
            AppStatus status = await _todoContext.Statuses.FirstOrDefaultAsync(s => s.Id == id);
            return status;
        }

        public async Task<AppStatus> GetStatus(string statusName)
        {
            AppStatus status = await _todoContext.Statuses.FirstOrDefaultAsync(s => s.StatusName == statusName);
            return status;
        }
    }
}
