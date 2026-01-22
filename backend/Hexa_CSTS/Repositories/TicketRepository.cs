using Hexa_CSTS.Data;
using Hexa_CSTS.Models;
using Microsoft.EntityFrameworkCore;


namespace Hexa_CSTS.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly HexaDbContext _context;

        public TicketRepository(HexaDbContext context)
        {
            _context = context;
        }

        public async Task<Ticket> AddAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task DeleteAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Ticket>> GetAllAsync() =>
            await _context.Tickets
                .AsNoTracking()
                .Include(t => t.CreatedBy)
                .ToListAsync();

        public async Task<Ticket> GetByIdAsync(int id) =>
            await _context.Tickets
                .AsNoTracking()
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedToAgnt)
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.TicketId == id);


        public async Task<List<Ticket>> GetByAgent(int agentId) =>
            await _context.Tickets
                .AsNoTracking()
                .Where(t => t.AssignedToId == agentId)
                .Include(t => t.CreatedBy)
                .ToListAsync();

        public async Task<List<Ticket>> GetByUser(int userId) =>
            await _context.Tickets
                .AsNoTracking()
                .Where(t => t.CreatedById == userId)
             .Include(t => t.CreatedBy)
                .ToListAsync();
        public async Task<List<Ticket>> GetUnassignedAsync() =>
        await _context.Tickets
            .AsNoTracking()
            .Where(t => t.AssignedToId == null)
            .Include(t => t.CreatedBy)
            .ToListAsync();
        public async Task<List<Ticket>> GetByStatus(TicketStatus status) =>
            await _context.Tickets
                .AsNoTracking()
                .Where(t => t.Status == status)
                .Include(t => t.CreatedBy)
                .ToListAsync();

        public async Task UpdateAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id) =>
        await _context.Tickets.AnyAsync(t => t.TicketId == id);
    }
}