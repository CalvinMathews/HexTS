using Hexa_CSTS.Models;


namespace Hexa_CSTS.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment> AddAsync(Comment comment);
        Task<Comment> GetByIdAsync(int id);
        Task<List<Comment>> GetByTicket(int ticketId);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(int id);
    }
}