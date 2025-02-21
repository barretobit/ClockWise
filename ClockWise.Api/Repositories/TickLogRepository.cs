using ClockWise.Api.Data;
using ClockWise.Api.Models;
using ClockWise.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClockWise.Api.Repositories
{
    public class TickLogRepository : ITickLogsInterface
    {
        private readonly ClockWiseDbContext _context;

        public TickLogRepository(ClockWiseDbContext context)
        {
            _context = context;
        }

        public async Task<List<TickLog>> GetAllTickLogsByEmployeeIdAsync(int employeeId)
        {
            return await _context.TickLogs.Where(tl => tl.EmployeeId == employeeId).ToListAsync();
        }

        public async Task<TickLog> GetTickLogByIdAsync(int id)
        {
            return await _context.TickLogs.FindAsync(id);
        }

        public async Task CreateTickLogAsync(TickLog tickLog)
        {
            _context.TickLogs.Add(tickLog);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTickLogAsync(TickLog tickLog)
        {
            _context.TickLogs.Update(tickLog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTickLogAsync(TickLog tickLog)
        {
            tickLog.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> TickLogExistsAsync(int id)
        {
            return await _context.TickLogs.AnyAsync(e => e.Id == id);
        }
    }
}
