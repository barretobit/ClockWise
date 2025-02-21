using ClockWise.Api.Models;

namespace ClockWise.Api.Repositories.Interfaces
{
    public interface ITickLogsInterface
    {
        Task<List<TickLog>> GetAllTickLogsByEmployeeIdAsync(int employeeId);

        Task<TickLog> GetTickLogByIdAsync(int id);

        Task CreateTickLogAsync(TickLog tickLog);

        Task UpdateTickLogAsync(TickLog tickLog);

        Task DeleteTickLogAsync(TickLog tickLog);

        Task<bool> TickLogExistsAsync(int id);
    }
}