using Hexa_CSTS.Models;

namespace Hexa_CSTS.Repositories
{
    public interface ITicketRepository
    {
        Task<Ticket> AddAsync(Ticket ticket);
        Task<Ticket> GetByIdAsync(int id);
        Task<List<Ticket>> GetAllAsync();
        Task UpdateAsync(Ticket ticket);
        Task DeleteAsync(int id);
        Task<List<Ticket>> GetByUser(int userId);       
        Task<List<Ticket>> GetByAgent(int agentId);        
        Task<List<Ticket>> GetByStatus(TicketStatus status);
        Task<bool> ExistsAsync(int id);
        Task<List<Ticket>> GetUnassignedAsync();
    }
}