using Microsoft.EntityFrameworkCore;
using Hexa_CSTS.Models;


namespace Hexa_CSTS.Data
{
    public class HexaDbContext : DbContext
    {
        public HexaDbContext(DbContextOptions<HexaDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.UserId);
            });

            modelBuilder.Entity<Ticket>(entity => {
                entity.ToTable("Tickets");
                entity.HasKey(t => t.TicketId);
                entity.HasOne(t => t.CreatedBy).WithMany(u => u.CreatedTickets).HasForeignKey(t => t.CreatedById).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.AssignedToAgnt).WithMany(u => u.AssignedTickets).HasForeignKey(t => t.AssignedToId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comments");
                entity.HasKey(c => c.CommentId);
                entity.HasOne(c => c.Ticket).WithMany(t => t.Comments).HasForeignKey(c => c.TicketId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(c => c.User).WithMany(u => u.Comments).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);
            });
        } }
}
