using System.ComponentModel.DataAnnotations;
namespace Hexa_CSTS.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        [Required,MaxLength(200)]
        public string Title { get; set; }= string.Empty;
        [Required,MaxLength(1200)]
        public string Description { get; set; } = string.Empty;
        public TicketPriority Priority { get; set; } = TicketPriority.Low;
        public TicketStatus Status { get; set; } = TicketStatus.New;
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public int? AssignedToId { get; set; }
        public User? AssignedToAgnt { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();


    }

    public enum TicketStatus
    {
        New=1,
        Assigned=2,
        InProgress=3,
        Resolved=4,
        Closed =5
    }
    public enum TicketPriority
    {
        Low=1,
        Medium = 2,
        High = 3
    }
}
