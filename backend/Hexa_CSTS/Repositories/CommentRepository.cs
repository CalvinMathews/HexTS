using Hexa_CSTS.Data;
using Hexa_CSTS.Models;
using Microsoft.EntityFrameworkCore;


namespace Hexa_CSTS.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly HexaDbContext _context;

        public CommentRepository(HexaDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task DeleteAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment); 
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Comment> GetByIdAsync(int id) =>
            await _context.Comments
                .AsNoTracking()
                .Include(c => c.User) 
                .FirstOrDefaultAsync(c => c.CommentId == id);

        public async Task<List<Comment>> GetByTicket(int ticketId) =>
            await _context.Comments
                .AsNoTracking()
                .Where(c => c.TicketId == ticketId) 
                .Include(c => c.User) 
                .ToListAsync();

        public async Task UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }
    }
}